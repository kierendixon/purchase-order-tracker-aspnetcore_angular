using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PurchaseOrderTracker.Persistence;
using PurchaseOrderTracker.Persistence.Initialization;

namespace PurchaseOrderTracker.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            InitializeDatabase(host);
            host.Run();
        }

        // TODO don't use default builder
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
                    logger.LogInformation("Initializing the application database...");
                    var poTrackerDbContext = services.GetRequiredService<PoTrackerDbContext>();
                    PoTrackerDbInitializer.Initialize(poTrackerDbContext);
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
