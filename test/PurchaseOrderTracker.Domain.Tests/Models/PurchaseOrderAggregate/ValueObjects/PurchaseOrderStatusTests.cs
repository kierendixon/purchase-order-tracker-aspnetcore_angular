using NUnit.Framework;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;

namespace PurchaseOrderTracker.Domain.Tests.Models.PurchaseOrderAggregate.ValueObjects
{
    public class PurchaseOrderStatusTests
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void Always_DefaultsStatusToDraft()
            {
                var status = new PurchaseOrderStatus();
                Assert.That(status.CurrentState, Is.EqualTo(PurchaseOrderStatus.State.Draft));
            }
        }
    }
}
