using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Web.Identity;

namespace PurchaseOrderTracker.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            InitializeDatabase(host);
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void InitializeDatabase(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    logger.LogInformation("Initializing the database...");

                    var poTrackerDbContext = services.GetRequiredService<PoTrackerDbContext>();
                    PoTrackerDbInitializer.Initialize(poTrackerDbContext);

                    var context = services.GetRequiredService<IdentityDbContext>();
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    IdentityDbInitializer.Initialize(context, userManager);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred initializing the database.");
                    throw;
                }
            }
        }
    }
}
