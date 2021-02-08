using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PurchaseOrderTracker.Persistence;
using PurchaseOrderTracker.Persistence.Identity;
using PurchaseOrderTracker.Persistence.Initialization;

namespace PurchaseOrderTracker.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            InitializeDatabase(host);
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }

        private static void InitializeDatabase(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    logger.LogInformation("Initializing the application database...");
                    var poTrackerDbContext = services.GetRequiredService<PoTrackerDbContext>();
                    PoTrackerDbInitializer.Initialize(poTrackerDbContext);

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
