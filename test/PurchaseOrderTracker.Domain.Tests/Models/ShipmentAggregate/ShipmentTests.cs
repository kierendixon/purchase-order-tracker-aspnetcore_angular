using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Tests.Builders;

namespace PurchaseOrderTracker.Domain.Tests.Models.ShipmentAggregate
{
    public class ShipmentTests
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void Always_AssignsValues()
            {
                var shipment = new ShipmentBuilder().Build();

                Assert.That(shipment.TrackingId, Is.EqualTo("shipmentTrackingId"));
                Assert.That(shipment.Company, Is.EqualTo("shipmentCompany"));
                // compare date component only (ignore time) to ensure this test always passes
                // otherwise the property would need to be abstracted so that we can
                // always set the same value as part of this test case
                Assert.That(shipment.EstimatedArrivalDate.Value.Date, Is.EqualTo(DateTime.Now.Date));
                Assert.That(shipment.ShippingCost, Is.EqualTo(999));
                Assert.That(shipment.DestinationAddress, Is.EqualTo("shipmentDestinationAddress"));
                Assert.That(shipment.Comments, Is.EqualTo("shipmentComments"));
            }

            [Test]
            public void Always_DefaultIdentifier()
            {
                var shipment = new ShipmentBuilder().Build();

                Assert.That(shipment.Id, Is.EqualTo(default(int)));
            }

            [Test]
            public void Always_DefaultPurchaseOrdersAsEmptyCollection()
            {
                var shipment = new ShipmentBuilder().Build();

                Assert.That(shipment.PurchaseOrders, Is.Empty);
            }

            [Test]
            public void Always_DefaultStatusToOpen()
            {
                var shipment = new ShipmentBuilder().Build();

                Assert.That(shipment.Status.CurrentState, Is.EqualTo(ShipmentStatus.State.Open));
            }

            [Test]
            public void NullCompany_ThrowsArgumentNullException()
            {
                try
                {
                    var shipment = new ShipmentBuilder().Company(null).Build();
                    Assert.Fail("Expected exception to be thrown");
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.InstanceOf<ArgumentNullException>());
                    Assert.That(ex.Message.ToLower(), Contains.Substring("company"));
                }
            }

            [Test]
            public void NullTrackingId_ThrowsArgumentNullException()
            {
                try
                {
                    var shipment = new ShipmentBuilder().TrackingId(null).Build();
                    Assert.Fail("Expected exception to be thrown");
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.InstanceOf<ArgumentNullException>());
                    Assert.That(ex.Message.ToLower(), Contains.Substring("trackingid"));
                }
            }
        }

        [TestFixture]
        public class AddPurchaseOrderMethod
        {
            [Test]
            public void OrderAlreadyAssignedToShipment_ExceptionThrown()
            {
                var shipment = new ShipmentBuilder().ShipmentId(123).Build();
                var poShipment = new ShipmentBuilder().ShipmentId(789).Build();
                var purchaseOrder = new PurchaseOrderBuilder().Shipment(poShipment).Build();
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);

                Assert.Throws<PurchaseOrderTrackerException>(() => shipment.AddPurchaseOrder(purchaseOrder));
            }

            [Test]
            public void OrderStatusCancelled_ExceptionThrown()
            {
                var shipment = new ShipmentBuilder().Build();
                var purchaseOrder = new PurchaseOrderBuilder().Build();
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Cancelled);

                Assert.Throws<PurchaseOrderTrackerException>(() => shipment.AddPurchaseOrder(purchaseOrder));
            }

            [Test]
            public void OrderStatusDelivered_ExceptionThrown()
            {
                var shipment = new ShipmentBuilder().Build();
                var purchaseOrder = new PurchaseOrderBuilder().Build();
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.PendingApproval);
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Shipped);
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Delivered);

                Assert.Throws<PurchaseOrderTrackerException>(() => shipment.AddPurchaseOrder(purchaseOrder));
            }

            [Test]
            public void OrderStatusDraft_ExceptionThrown()
            {
                var shipment = new ShipmentBuilder().Build();
                var purchaseOrder = new PurchaseOrderBuilder().Build();

                Assert.Throws<PurchaseOrderTrackerException>(() => shipment.AddPurchaseOrder(purchaseOrder));
            }

            [Test]
            public void OrderStatusIsApproved_OrderAdded()
            {
                var shipment = new ShipmentBuilder().Build();
                var purchaseOrder = new PurchaseOrderBuilder().Build();
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);

                shipment.AddPurchaseOrder(purchaseOrder);

                Assert.That(shipment.PurchaseOrders.Contains(purchaseOrder), Is.True);
            }

            [Test]
            public void OrderStatusPendingApproval_ExceptionThrown()
            {
                var shipment = new ShipmentBuilder().Build();
                var purchaseOrder = new PurchaseOrderBuilder().Build();
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.PendingApproval);

                Assert.Throws<PurchaseOrderTrackerException>(() => shipment.AddPurchaseOrder(purchaseOrder));
            }

            [Test]
            public void OrderStatusShipped_ExceptionThrown()
            {
                var shipment = new ShipmentBuilder().Build();
                var purchaseOrder = new PurchaseOrderBuilder().Build();
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.PendingApproval);
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Shipped);

                Assert.Throws<PurchaseOrderTrackerException>(() => shipment.AddPurchaseOrder(purchaseOrder));
            }
        }

        [TestFixture]
        public class IsDelayedMethod
        {
            [Test]
            public void Delayed_ReturnsTrue()
            {
                var shipment = new ShipmentBuilder()
                    .EstimatedArrivalDate(DateTime.Today.AddDays(-1))
                    .Build();

                Assert.That(shipment.IsDelayed(), Is.True);
            }

            [Test]
            public void DelayedButDelivered_ReturnsFalse()
            {
                var shipment = new ShipmentBuilder()
                    .EstimatedArrivalDate(DateTime.Today.AddDays(-1))
                    .Build();
                shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);
                shipment.UpdateStatus(ShipmentStatus.Trigger.Delivered);

                Assert.That(shipment.IsDelayed(), Is.False);
            }

            [Test]
            public void NotDelayed_ReturnsFalse()
            {
                var shipment = new ShipmentBuilder()
                    .EstimatedArrivalDate(DateTime.Today.AddDays(1))
                    .Build();

                Assert.That(shipment.IsDelayed(), Is.False);
            }
        }

        [TestFixture]
        public class IsDelayedMoreThan7DaysMethod
        {
            [Test]
            public void Delayed_ReturnsTrue()
            {
                var shipment = new ShipmentBuilder()
                    .EstimatedArrivalDate(DateTime.Today.AddDays(-8))
                    .Build();

                Assert.That(shipment.IsDelayedMoreThan7Days(), Is.True);
            }

            [Test]
            public void DelayedButDelivered_ReturnsFalse()
            {
                var shipment = new ShipmentBuilder()
                    .EstimatedArrivalDate(DateTime.Today.AddDays(-8))
                    .Build();
                shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);
                shipment.UpdateStatus(ShipmentStatus.Trigger.Delivered);

                Assert.That(shipment.IsDelayedMoreThan7Days(), Is.False);
            }

            [Test]
            public void DelayedExactly7Days_ReturnsFalse()
            {
                var shipment = new ShipmentBuilder()
                    .EstimatedArrivalDate(DateTime.Today.AddDays(-7))
                    .Build();

                Assert.That(shipment.IsDelayedMoreThan7Days(), Is.False);
            }

            [Test]
            public void DelayedLessThan7Days_ReturnsFalse()
            {
                var shipment = new ShipmentBuilder()
                    .EstimatedArrivalDate(DateTime.Today.AddDays(-1))
                    .Build();

                Assert.That(shipment.IsDelayedMoreThan7Days(), Is.False);
            }

            [Test]
            public void NotDelayed_ReturnsFalse()
            {
                var shipment = new ShipmentBuilder()
                    .EstimatedArrivalDate(DateTime.Today.AddDays(1))
                    .Build();

                Assert.That(shipment.IsDelayedMoreThan7Days(), Is.False);
            }
        }

        [TestFixture]
        public class IsScheduledForDeliveryTodayMethod
        {
            [Test]
            public void ScheduledToday_ReturnsTrue()
            {
                var shipment = new ShipmentBuilder()
                    .EstimatedArrivalDate(DateTime.Today)
                    .Build();

                Assert.That(shipment.IsScheduledForDeliveryToday(), Is.True);
            }

            [Test]
            public void ScheduledTommorrow_ReturnsFalse()
            {
                var shipment = new ShipmentBuilder()
                    .EstimatedArrivalDate(DateTime.Today.AddDays(1))
                    .Build();

                Assert.That(shipment.IsScheduledForDeliveryToday(), Is.False);
            }

            [Test]
            public void ScheduledYesterday_ReturnsFalse()
            {
                var shipment = new ShipmentBuilder()
                    .EstimatedArrivalDate(DateTime.Today.AddDays(-1))
                    .Build();

                Assert.That(shipment.IsScheduledForDeliveryToday(), Is.False);
            }
        }

        [TestFixture]
        public class UpdateStatusMethod
        {
            [Test]
            public void UpdateToDelivered_UpdatesAllPurchaseOrdersToDelivered()
            {
                var purchaseOrder1 = new PurchaseOrderBuilder().Build();
                purchaseOrder1.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var purchaseOrder2 = new PurchaseOrderBuilder().Build();
                purchaseOrder2.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var shipment = new ShipmentBuilder()
                    .PurchaseOrders(new List<PurchaseOrder>(new[]
                    {
                        purchaseOrder1,
                        purchaseOrder2
                    }))
                    .Build();
                shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);

                shipment.UpdateStatus(ShipmentStatus.Trigger.Delivered);

                Assert.That(
                    shipment.PurchaseOrders.All(p => p.Status.CurrentState == PurchaseOrderStatus.State.Delivered),
                    Is.True);
            }

            [Test]
            public void UpdateToShipped_UpdatesAllPurchaseOrdersToShipped()
            {
                var purchaseOrder1 = new PurchaseOrderBuilder().Build();
                purchaseOrder1.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var purchaseOrder2 = new PurchaseOrderBuilder().Build();
                purchaseOrder2.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                var shipment = new ShipmentBuilder()
                    .PurchaseOrders(new List<PurchaseOrder>(new[]
                    {
                        purchaseOrder1,
                        purchaseOrder2
                    }))
                    .Build();

                shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);

                Assert.That(
                    shipment.PurchaseOrders.All(p => p.Status.CurrentState == PurchaseOrderStatus.State.Shipped),
                    Is.True);
            }
        }

        [TestFixture]
        public class CanBeDeletedMethod
        {
            [Test]
            public void AwaitingShippingStatus_ReturnsTrue()
            {
                var shipment = new ShipmentBuilder().Build();
                shipment.UpdateStatus(ShipmentStatus.Trigger.AwaitingShipping);

                Assert.That(shipment.CanBeDeleted, Is.True);
            }

            [Test]
            public void DeliveredStatus_ReturnsFalse()
            {
                var shipment = new ShipmentBuilder().Build();
                shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);
                shipment.UpdateStatus(ShipmentStatus.Trigger.Delivered);

                Assert.That(shipment.CanBeDeleted, Is.False);
            }

            [Test]
            public void OpenStatus_ReturnsTrue()
            {
                var shipment = new ShipmentBuilder().Build();

                Assert.That(shipment.CanBeDeleted, Is.True);
            }

            [Test]
            public void ShippedStatus_ReturnsTrue()
            {
                var shipment = new ShipmentBuilder().Build();
                shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);

                Assert.That(shipment.CanBeDeleted, Is.True);
            }
        }
    }
}
