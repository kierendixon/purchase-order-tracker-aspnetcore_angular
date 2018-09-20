using System.Threading.Tasks;
using NUnit.Framework;

namespace PurchaseOrderTracker.Web.Tests.Features.Api.Supplier
{
    public class SupplierControllerTests: BaseTestHostTest
    {
        [TestFixture]
        [Category("DatabaseIntegration")]
        public class GetMethod : SupplierControllerTests
        {
            [Test]
            public async Task ResultWithSingleRecord_SerialisesToJson()
            {
                var expectedResult =
                    "{\"suppliers\":[{\"id\":1,\"name\":\"Techzon\"}]}";

                var response = await _client.GetAsync("/api/supplier/1");
                var responseString = await response.Content.ReadAsStringAsync();

                response.EnsureSuccessStatusCode();
                Assert.That(responseString, Is.EqualTo(expectedResult));
            }
            
            [Test]
            public async Task ResultWithMultipleRecords_SerialisesToJson()
            {
                var expectedResult =
                    "{\"suppliers\":[{\"id\":2,\"name\":\"Furniture Max\"},{\"id\":3,\"name\":\"Office Supplies A+\"},{\"id\":1,\"name\":\"Techzon\"}]}";

                var response = await _client.GetAsync("/api/supplier");
                var responseString = await response.Content.ReadAsStringAsync();

                response.EnsureSuccessStatusCode();
                Assert.That(responseString, Is.EqualTo(expectedResult));
            }
        }
    }
}