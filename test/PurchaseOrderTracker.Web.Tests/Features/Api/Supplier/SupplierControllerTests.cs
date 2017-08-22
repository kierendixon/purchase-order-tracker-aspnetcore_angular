using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

namespace PurchaseOrderTracker.Web.Tests.Features.Api.Supplier
{
    public class SupplierControllerTests
    {
        [TestFixture]
        public class CreateMethod : SupplierControllerTests
        {
            [Test]
            [Ignore("TODO: Build using webpack before executing this test")]
            public async Task ResultWithMultipleRecords_SerialisesListObjectToJson()
            {
                var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
                var client = server.CreateClient();

                var response = await client.GetAsync("/api/supplier");
                var responseString = await response.Content.ReadAsStringAsync();

                response.EnsureSuccessStatusCode();
                Assert.That(responseString, Contains.Value("TODO"));
            }
        }
    }
}
