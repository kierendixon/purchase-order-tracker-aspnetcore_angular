using System;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using PurchaseOrderTracker.DAL;

namespace PurchaseOrderTracker.Web.Tests
{
    public class BaseTestHostTest
    {
        protected HttpClient _client;

        [SetUp]
        public void OnInit()
        {
            string solutionRelativeTargetProjectParentDir = "src";
            var startupAssembly = typeof(Startup).GetTypeInfo().Assembly;
            var contentRoot = TestHelper.GetProjectPath(solutionRelativeTargetProjectParentDir, startupAssembly);

            var host = WebHost.CreateDefaultBuilder()
                .UseEnvironment("Testing")
                .UseContentRoot(contentRoot)
                .UseStartup<Startup>();
            var server = new TestServer(host);
            RunPoTrackerDbInitializer(server.Host);

            _client = server.CreateClient();
        }

        private static void RunPoTrackerDbInitializer(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<PoTrackerDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
        }
    }
}
