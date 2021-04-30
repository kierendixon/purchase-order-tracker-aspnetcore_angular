using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;
using PurchaseOrderTracker.Identity.Persistence;
using PurchaseOrderTracker.Identity.Persistence.Initialization;

namespace PurchaseOrderTracker.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // TODO log if startup fails
            var host = CreateHostBuilder(args).Build();
            InitializeDatabase(host);
            host.Run();
        }

        // TODO: don't use default builder
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void InitializeDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    logger.LogInformation("Initializing the identity database...");
                    var context = services.GetRequiredService<IdentityDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    IdentityDbInitializer.Initialize(context, userManager);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred initializing the database");
                    throw;
                }
            }
        }
    }
}
