using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PurchaseOrderTracker.DAL;

namespace PurchaseOrderTracker.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            RunPoTrackerDbInitializer(host);

            host.Run();
        }

        private static void RunPoTrackerDbInitializer(IWebHost host)
        {
            var services = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));
            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<PoTrackerDbContext>();
                DbInitializer.Initialize(context);
            }
        }
    }
}
