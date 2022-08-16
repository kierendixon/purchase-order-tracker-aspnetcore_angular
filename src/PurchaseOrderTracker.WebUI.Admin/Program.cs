using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using PurchaseOrderTracker.AspNet.Common.DataProtection;
using PurchaseOrderTracker.AspNet.Common.HealthChecks;

namespace PurchaseOrderTracker.WebUI.Admin;

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
        services.AddCustomRazorPages();
        services.AddControllersWithViews();
        services.AddCustomHeaderPropagation();
        //services.AddCustomHttpClients();
        services.AddCustomAuthentication();
        services.AddCustomAuthorization();
        services.AddCustomDataProtection(
            env,
            config.GetSection(DataProtectionOptions.Position).Get<DataProtectionOptions>());
        services.AddCustomHealthChecks(config);
    }

    private static void ConfigureApp(WebApplication app)
    {
        app.UseHttpLogging();
        app.UseCustomErrorHandler(app.Environment);
        app.UseStaticFiles();
        app.UseHeaderPropagation();

        //TODO
        // check if user is admin
        //app.Use(async (context, next) =>
        //{
        //    if (!context.User.Identity.IsAuthenticated)
        //    {
        //        var uri = "/account";
        //        // TODO Angular SPA performs a local redirect instead of back to this app
        //        //if (context.Request.Path.HasValue)
        //        //{
        //        //    uri = QueryHelpers.AddQueryString(uri, "returnUrl", context.Request.Path);
        //        //}

        //        context.Response.Redirect(uri);
        //        //await context.ChallengeAsync("oidc");
        //    }
        //    // TODO check against existing policy instead of defining it again AddCustomAuthorization()
        //    else if (context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && c.Value == "admin") != null)
        //    {
        //        await next();
        //    }
        //    else
        //    {
        //        // authenticated but not admin
        //        context.Response.Redirect("/main-site");
        //    }
        //});

        app.UseCustomEndpoints();

        if (!app.Environment.IsDevelopment())
        {
            app.MapFallbackToFile("index.html");
        }
    }
}

public static class ServiceCollectionExtensions
{
    //private static readonly string _executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

    public static void AddCustomRazorPages(this IServiceCollection services)
    {
        services.AddRazorPages(opt =>
        {
            opt.RootDirectory = "/Features"; // TODO error page is still in Pages folder
        });
    }

    public static void AddCustomHeaderPropagation(this IServiceCollection services)
    {
        services.AddHeaderPropagation(opt =>
        {
            opt.Headers.Add(HeaderNames.Referer);
            // TODO get guid from Activity class instead?
            opt.Headers.Add("X-Correlation-ID", ctx => new StringValues(Guid.NewGuid().ToString()));
        });
    }

    //public static void AddCustomHttpClients(this IServiceCollection services)
    //{
    //    services.AddHttpClient<PurchaseOrderTrackerHttpClient>(c =>
    //    {
    //        c.BaseAddress = new Uri("http://localhost:4202/api/");
    //        c.DefaultRequestHeaders.Add(HeaderNames.UserAgent, _executingAssemblyName);
    //    }).AddHeaderPropagation();
    //}

    public static void AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddUrlGroup(new Uri(configuration["WebApi.Url"] + "/health"), "PurchaseOrderTracker.WebApi", timeout: TimeSpan.FromSeconds(5));
    }
}

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
            options.AddPolicy("Administrator",
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
                //context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
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
                //context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0045:Convert to conditional expression", Justification = "env.IsDevelopment() statements are easier to read without this")]
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

        app.MapControllers();
        app.MapRazorPages();
        app.MapHealthChecks("/health", new HealthCheckOptions()
        {
            ResponseWriter = HealthCheckResponseWriter.WriteDetailedJsonResponse
        }).AllowAnonymous();
    }
}
