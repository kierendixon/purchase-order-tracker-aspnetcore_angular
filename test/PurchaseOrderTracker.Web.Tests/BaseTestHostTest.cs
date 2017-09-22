using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

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

            var server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseEnvironment("Testing")
                .UseContentRoot(contentRoot)
                .UseStartup<Startup>());

            _client = server.CreateClient();
        }
    }
}
