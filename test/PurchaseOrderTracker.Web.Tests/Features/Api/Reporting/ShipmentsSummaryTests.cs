using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Web.Features.Api.Reporting;

namespace PurchaseOrderTracker.Web.Tests.Features.Api.Reporting
{
    public class ShipmentsSummaryTests: BaseAutomapperTest
    {
        public class Handler
        {
            [TestFixture]
            public class HandleMethod : ShipmentsSummaryTests
            {
                [Test]
                public async Task HappyPath_CorrectResultsReturned()
                {
                    ShipmentsSummary.Query query = new ShipmentsSummary.Query();

                    var result = await GetResult_HappyPath_CorrectResultsReturned(query);

                    Assert.That(result.ShipmentsDelayed, Is.EqualTo(1));
                    Assert.That(result.ShipmentsDelayedMoreThan7Days, Is.EqualTo(1));
                    Assert.That(result.ShipmentsSchedForDeliveryToday, Is.EqualTo(1));
                    Assert.That(result.TotalOpenOrders, Is.EqualTo(7));
                }

                private async Task<ShipmentsSummary.Result> GetResult_HappyPath_CorrectResultsReturned(ShipmentsSummary.Query query)
                {
                    var options = TestHelper.GetDbOptions(nameof(HappyPath_CorrectResultsReturned));
                    using (var context = new PoTrackerDbContext(options))
                    {
                        DbInitializer.Initialize(context);
                        var handler = new ShipmentsSummary.Handler(context);
                        return await handler.Handle(query);
                    }
                }
            }

        }
    }

}
