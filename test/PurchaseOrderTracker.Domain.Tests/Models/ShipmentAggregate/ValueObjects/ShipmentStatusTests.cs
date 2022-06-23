using NUnit.Framework;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate.ValueObjects;

namespace PurchaseOrderTracker.Domain.Tests.Models.ShipmentAggregate.ValueObjects;

public class ShipmentStatusTests
{
    [TestFixture]
    public class Constructor
    {
        [Test]
        public void defaults_to_open_status()
        {
            var status = new ShipmentStatus();
            Assert.That(status.CurrentState, Is.EqualTo(ShipmentStatus.State.Open));
        }
    }
}
