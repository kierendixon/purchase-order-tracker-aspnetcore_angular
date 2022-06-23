using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Tests.Builders;

namespace PurchaseOrderTracker.Domain.Tests.Models.ShipmentAggregate;

public class ShipmentTests
{
    [TestFixture]
    public class Constructor
    {
        [Test]
        public void assigns_values()
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
        public void defaults_id()
        {
            var shipment = new ShipmentBuilder().Build();

            Assert.That(shipment.Id, Is.EqualTo(default(int)));
        }

        [Test]
        public void defaults_purchase_orders_as_empty_collection()
        {
            var shipment = new ShipmentBuilder().Build();

            Assert.That(shipment.PurchaseOrders, Is.Empty);
        }

        [Test]
        public void defaults_status_to_open()
        {
            var shipment = new ShipmentBuilder().Build();

            Assert.That(shipment.Status.CurrentState, Is.EqualTo(ShipmentStatus.State.Open));
        }

        [Test]
        public void throws_arg_null_ex_when_company_is_null()
        {
            try
            {
                new ShipmentBuilder().Company(null).Build();
                Assert.Fail("Expected exception to be thrown");
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.InstanceOf<ArgumentNullException>());
                Assert.That(ex.Message.ToLower(), Contains.Substring("company"));
            }
        }

        [Test]
        public void throws_arg_null_ex_when_tracking_id_is_null()
        {
            try
            {
                new ShipmentBuilder().TrackingId(null).Build();
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
        public void throws_ex_when_order_is_already_assigned_to_a_shipment()
        {
            var shipment = new ShipmentBuilder().ShipmentId(123).Build();
            var poShipment = new ShipmentBuilder().ShipmentId(789).Build();
            var purchaseOrder = new PurchaseOrderBuilder().Shipment(poShipment).Build();
            purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);

            Assert.Throws<PurchaseOrderTrackerException>(() => shipment.AddPurchaseOrder(purchaseOrder));
        }

        [Test]
        public void throws_ex_when_order_status_is_cancelled()
        {
            var shipment = new ShipmentBuilder().Build();
            var purchaseOrder = new PurchaseOrderBuilder().Build();
            purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Cancelled);

            Assert.Throws<PurchaseOrderTrackerException>(() => shipment.AddPurchaseOrder(purchaseOrder));
        }

        [Test]
        public void throws_ex_when_order_status_is_delivered()
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
        public void throws_ex_when_order_status_is_draft()
        {
            var shipment = new ShipmentBuilder().Build();
            var purchaseOrder = new PurchaseOrderBuilder().Build();

            Assert.Throws<PurchaseOrderTrackerException>(() => shipment.AddPurchaseOrder(purchaseOrder));
        }

        [Test]
        public void adds_order_when_order_status_is_approved()
        {
            var shipment = new ShipmentBuilder().Build();
            var purchaseOrder = new PurchaseOrderBuilder().Build();
            purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);

            shipment.AddPurchaseOrder(purchaseOrder);

            Assert.That(shipment.PurchaseOrders.Contains(purchaseOrder), Is.True);
        }

        [Test]
        public void throws_ex_when_order_status_is_pending_approval()
        {
            var shipment = new ShipmentBuilder().Build();
            var purchaseOrder = new PurchaseOrderBuilder().Build();
            purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.PendingApproval);

            Assert.Throws<PurchaseOrderTrackerException>(() => shipment.AddPurchaseOrder(purchaseOrder));
        }

        [Test]
        public void throws_ex_when_order_status_is_shipped()
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
        public void returns_true_when_shipment_is_delayed()
        {
            var shipment = new ShipmentBuilder()
                .EstimatedArrivalDate(DateTime.Today.AddDays(-1))
                .Build();

            Assert.That(shipment.IsDelayed(), Is.True);
        }

        [Test]
        public void returns_false_when_shipment_is_delayed_but_delivered()
        {
            var shipment = new ShipmentBuilder()
                .EstimatedArrivalDate(DateTime.Today.AddDays(-1))
                .Build();
            shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);
            shipment.UpdateStatus(ShipmentStatus.Trigger.Delivered);

            Assert.That(shipment.IsDelayed(), Is.False);
        }

        [Test]
        public void returns_false_when_shipment_is_not_delayed()
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
        public void returns_true_when_delayed_more_than_7_days()
        {
            var shipment = new ShipmentBuilder()
                .EstimatedArrivalDate(DateTime.Today.AddDays(-8))
                .Build();

            Assert.That(shipment.IsDelayedMoreThan7Days(), Is.True);
        }

        [Test]
        public void returns_false_when_shipment_is_delayed_but_delivered()
        {
            var shipment = new ShipmentBuilder()
                .EstimatedArrivalDate(DateTime.Today.AddDays(-8))
                .Build();
            shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);
            shipment.UpdateStatus(ShipmentStatus.Trigger.Delivered);

            Assert.That(shipment.IsDelayedMoreThan7Days(), Is.False);
        }

        [Test]
        public void returns_false_when_shipment_is_delayed_exactly_7_days()
        {
            var shipment = new ShipmentBuilder()
                .EstimatedArrivalDate(DateTime.Today.AddDays(-7))
                .Build();

            Assert.That(shipment.IsDelayedMoreThan7Days(), Is.False);
        }

        [Test]
        public void returns_false_when_shipment_is_delayed_less_than_7_days()
        {
            var shipment = new ShipmentBuilder()
                .EstimatedArrivalDate(DateTime.Today.AddDays(-1))
                .Build();

            Assert.That(shipment.IsDelayedMoreThan7Days(), Is.False);
        }

        [Test]
        public void returns_false_when_shipment_is_not_delayed()
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
        public void returns_true_when_scheduled_for_today()
        {
            var shipment = new ShipmentBuilder()
                .EstimatedArrivalDate(DateTime.Today)
                .Build();

            Assert.That(shipment.IsScheduledForDeliveryToday(), Is.True);
        }

        [Test]
        public void returns_false_when_scheduled_for_tomorrow()
        {
            var shipment = new ShipmentBuilder()
                .EstimatedArrivalDate(DateTime.Today.AddDays(1))
                .Build();

            Assert.That(shipment.IsScheduledForDeliveryToday(), Is.False);
        }

        [Test]
        public void returns_false_when_scheduled_yesterday()
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
        public void when_delivered_also_updates_all_purchase_orders_to_delivered()
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
        public void when_shipped_also_updates_all_purchase_orders_to_shipped()
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
        public void returns_true_when_awaiting_shipping()
        {
            var shipment = new ShipmentBuilder().Build();
            shipment.UpdateStatus(ShipmentStatus.Trigger.AwaitingShipping);

            Assert.That(shipment.Status.CurrentState, Is.EqualTo(ShipmentStatus.State.AwaitingShipping));
            Assert.That(shipment.CanBeDeleted, Is.True);
        }

        [Test]
        public void returns_false_when_delivered()
        {
            var shipment = new ShipmentBuilder().Build();
            shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);
            shipment.UpdateStatus(ShipmentStatus.Trigger.Delivered);

            Assert.That(shipment.Status.CurrentState, Is.EqualTo(ShipmentStatus.State.Delivered));
            Assert.That(shipment.CanBeDeleted, Is.False);
        }

        [Test]
        public void returns_true_when_open()
        {
            var shipment = new ShipmentBuilder().Build();

            Assert.That(shipment.Status.CurrentState, Is.EqualTo(ShipmentStatus.State.Open));
            Assert.That(shipment.CanBeDeleted, Is.True);
        }

        [Test]
        public void returns_true_when_shipped()
        {
            var shipment = new ShipmentBuilder().Build();
            shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);

            Assert.That(shipment.Status.CurrentState, Is.EqualTo(ShipmentStatus.State.Shipped));
            Assert.That(shipment.CanBeDeleted, Is.True);
        }
    }
}
