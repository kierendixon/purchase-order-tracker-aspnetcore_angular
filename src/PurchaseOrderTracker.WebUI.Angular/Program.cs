using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PurchaseOrderTracker.AspNet.Common.HealthChecks;

namespace PurchaseOrderTracker.WebUI.Angular;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

        var app = builder.Build();
        ConfigureApp(app);

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
    {
        services.UseCustomRazorPages();
        services.AddControllersWithViews();
        services.AddCustomAuthentication();
        services.AddCustomAuthorization();
        services.UseCustomDataProtection(config, env);
        services.UseCustomHealthChecks(config);
    }

    private static void ConfigureApp(WebApplication app)
    {
        app.UseHttpLogging();
        app.UseCustomErrorHandler(app.Environment);
        app.UseStaticFiles();
        app.UseCustomEndpoints();

        if (!app.Environment.IsDevelopment())
        {
            app.MapFallbackToFile("index.html");
        }
    }
}

public static class ServiceCollectionExtensions
{
    public static void UseCustomRazorPages(this IServiceCollection services)
    {
        services.AddRazorPages(opt =>
        {
            opt.RootDirectory = "/Features";
        });
    }

    public static void UseCustomDataProtection(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        var dataProtection = services.AddDataProtection()
            .SetApplicationName("PurchaseOrderTrackerApp");

        if (!env.IsDevelopment())
        {
            // not secure - keys will be saved to file system unencrypted
            dataProtection.PersistKeysToFileSystem(new DirectoryInfo(configuration["DataProtection:KeysDirectory"]));
        }
    }

    public static void UseCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddUrlGroup(new Uri(configuration["WebApi.Url"] + "/health"), "PurchaseOrderTracker.WebApi", timeout: TimeSpan.FromSeconds(5));
    }
}

// TODO move duplicate code to AspNet.Common
public static class IdentityServiceCollectionExtensions
{
    public static void AddCustomAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(ConfigureCookies);
    }

    public static void AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "Administrator",
                new AuthorizationPolicyBuilder()
                    .RequireClaim(ClaimTypes.Role, "admin")
                    .Build());
        });
    }

    private static readonly Action<CookieAuthenticationOptions> ConfigureCookies = opt =>
    {
        opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        opt.Cookie.Name = "pot.session";
        opt.LoginPath = new PathString("/account");

        // override default CookieAuthenticationEvents to use different IsAjaxRequest logic
        // https://github.com/dotnet/aspnetcore/blob/52eff90fbcfca39b7eb58baad597df6a99a542b0/src/Security/Authentication/Cookies/src/CookieAuthenticationHandler.cs
        // https://github.com/dotnet/aspnetcore/blob/52eff90fbcfca39b7eb58baad597df6a99a542b0/src/Security/Authentication/Cookies/src/CookieAuthenticationEvents.cs
        opt.Events.OnRedirectToLogin = context =>
        {
            if (IsAjaxRequest(context.Request))
            {
                // context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                context.Response.Redirect(context.RedirectUri);
            }

            return Task.CompletedTask;
        };

        opt.Events.OnRedirectToAccessDenied = context =>
        {
            if (IsAjaxRequest(context.Request))
            {
                // context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
            }
            else
            {
                context.Response.Redirect(context.RedirectUri);
            }

            return Task.CompletedTask;
        };

        opt.Events.OnRedirectToLogout = context =>
        {
            if (IsAjaxRequest(context.Request))
            {
                // context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
            }
            else
            {
                context.Response.Redirect(context.RedirectUri);
            }

            return Task.CompletedTask;
        };

        opt.Events.OnRedirectToReturnUrl = context =>
        {
            if (IsAjaxRequest(context.Request))
            {
                // context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
            }
            else
            {
                context.Response.Redirect(context.RedirectUri);
            }

            return Task.CompletedTask;
        };
    };

    // can alternatively check the request path: context.Request.Path.StartsWithSegments("/api")
    private static bool IsAjaxRequest(HttpRequest request)
    {
        return !(request.Headers.TryGetValue("accept", out var acceptValues)
            && acceptValues.Contains("text/html", StringComparer.InvariantCultureIgnoreCase));
    }
}

public static class WebApplicationExtensions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0045:Convert to conditional expression", Justification = "makes env.IsDevelopment() statements harder to read")]
    public static void UseCustomErrorHandler(this WebApplication app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }
    }

    public static void UseCustomEndpoints(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        // TODO controllers are not currently used but I need to change /api direct access to bff pattern
        app.MapControllers();
        app.MapRazorPages();
        app.MapHealthChecks("/health", new HealthCheckOptions()
        {
            ResponseWriter = HealthCheckResponseWriter.WriteDetailedJsonResponse
        });
    }
}
