using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using PurchaseOrderTracker.Web.Features.Api.Reporting;

namespace PurchaseOrderTracker.Web.Tests.Features.Api.Reporting
{
    public class ReportingControllerTests
    {
        [TestFixture]
        public class ShipmentsSummaryMethod
        {
            [Test]
            public async Task AnyResult_IsReturnedAsObjectResult()
            {
                var result = new ShipmentsSummary.Result(1, 2, 3, 4);
                var mediator = new Mock<IMediator>();
                mediator.Setup(m => m.Send(It.IsAny<ShipmentsSummary.Query>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

                var actualResult = await new ReportingController(mediator.Object).ShipmentsSummary();

                mediator.Verify(m => m.Send(It.IsAny<ShipmentsSummary.Query>(), It.IsAny<CancellationToken>()), Times.Once());
                Assert.That(result, Is.SameAs(actualResult.Value));
            }
        }
    }
}
