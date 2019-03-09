using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate;
using PurchaseOrderTracker.Domain.Tests.Builders;

namespace PurchaseOrderTracker.Domain.Tests.Models.PurchaseOrderAggregate
{
    public class PurchaseOrderTests
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void Always_AssignsValues()
            {
                var supplier = new SupplierBuilder().Build();
                var purchaseOrder = new PurchaseOrder("orderNo", supplier);

                Assert.That(purchaseOrder.OrderNo, Is.EqualTo("orderNo"));
                Assert.That(purchaseOrder.Supplier, Is.SameAs(supplier));
            }

            [Test]
            public void Always_DefaultCreatedDateToNow()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();

                // compare date component only (ignore time) to ensure this test always passes
                // otherwise the CreateDate property would need to be abstracted so that we can
                // always set the same value as part of this test case
                Assert.That(purchaseOrder.CreatedDate.Date, Is.EqualTo(DateTime.Today));
            }

            [Test]
            public void Always_DefaultIdentifier()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();

                Assert.That(purchaseOrder.Id, Is.EqualTo(default(int)));
            }

            [Test]
            public void Always_DefaultLineItemsAsEmptyCollection()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();

                Assert.That(purchaseOrder.LineItems, Is.Empty);
            }

            [Test]
            public void Always_DefaultStatusToDraft()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();

                Assert.That(purchaseOrder.Status.CurrentState, Is.EqualTo(PurchaseOrderStatus.State.Draft));
            }

            [Test]
            public void NullOrderNo_ThrowsArgumentNullException()
            {
                try
                {
                    var product = new PurchaseOrder(null, new SupplierBuilder().Build());
                    Assert.Fail("Expected exception to be thrown");
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.InstanceOf<ArgumentNullException>());
                    Assert.That(ex.Message.ToLower(), Contains.Substring("orderno"));
                }
            }

            [Test]
            public void NullSupplier_ThrowsArgumentNullException()
            {
                try
                {
                    var product = new PurchaseOrder("orderNo", null);
                    Assert.Fail("Expected exception to be thrown");
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.InstanceOf<ArgumentNullException>());
                    Assert.That(ex.Message.ToLower(), Contains.Substring("supplier"));
                }
            }
        }

        [TestFixture]
        public class AddLineItemMethod
        {
            [Test]
            public void ProductIsFromDifferentSupplier_ExceptionThrown()
            {
                var supplier = new SupplierBuilder().Id(123).Build();
                var purchaseOrder = new PurchaseOrderBuilder()
                    .Supplier(supplier)
                    .Build();
                var lineItem = new PurchaseOrderLineBuilder()
                    .Product(new ProductBuilder().SupplierId(789).Build())
                    .Build();

                Assert.Throws<PurchaseOrderTrackerException>(() => purchaseOrder.AddLineItem(lineItem));
            }

            [Test]
            public void ProductIsFromSameSupplier_LineItemAddedToCollection()
            {
                var supplier = new SupplierBuilder().Id(123).Build();
                var purchaseOrder = new PurchaseOrderBuilder()
                    .Supplier(supplier)
                    .Build();
                var lineItem = new PurchaseOrderLineBuilder()
                    .Product(new ProductBuilder().SupplierId(123).Build())
                    .Build();

                purchaseOrder.AddLineItem(lineItem);
            }
        }

        [TestFixture]
        public class ChangeSupplierMethod
        {
            [Test]
            public void PurchaseOrderIsNotOpen_ExceptionThrown()
            {
                var purchaseOrder = new PurchaseOrderBuilder()
                    .Supplier(new SupplierBuilder().Id(111).Build())
                    .Build();
                var newSupplier = new SupplierBuilder().Id(222).Build();
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Approved);
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Shipped);
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Delivered);

                Assert.Throws<PurchaseOrderTrackerException>(() => purchaseOrder.ChangeSupplier(newSupplier));
            }

            [Test]
            public void PurchaseOrderIsOpen_SupplierChangedAndLineItemsCleared()
            {
                var product = new ProductBuilder().SupplierId(111).Build();
                var purchaseOrder = new PurchaseOrderBuilder()
                    .Supplier(new SupplierBuilder().Id(111).Build())
                    .LineItems(
                        new List<PurchaseOrderLine>
                        {
                            new PurchaseOrderLineBuilder().Product(product).Build(),
                            new PurchaseOrderLineBuilder().Product(product).Build(),
                            new PurchaseOrderLineBuilder().Product(product).Build()
                        }
                    )
                    .Build();
                var newSupplier = new SupplierBuilder().Id(222).Build();

                purchaseOrder.ChangeSupplier(newSupplier);

                Assert.That(purchaseOrder.LineItems.Any(), Is.False);
                Assert.That(purchaseOrder.Supplier, Is.SameAs(newSupplier));
            }
        }

        [TestFixture]
        public class CanBeDeletedMethod
        {
            [Test]
            public void StatusIsDelivered_ReturnsFalse()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Approved);
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Shipped);
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Delivered);

                Assert.That(purchaseOrder.CanBeDeleted, Is.False);
            }

            [Test]
            public void StatusIsNotShippedOrDelivered_ReturnsTrue()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();
                Assert.That(purchaseOrder.CanBeDeleted, Is.True);

                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.PendingApproval);
                Assert.That(purchaseOrder.CanBeDeleted, Is.True);

                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Approved);
                Assert.That(purchaseOrder.CanBeDeleted, Is.True);

                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Cancelled);
                Assert.That(purchaseOrder.CanBeDeleted, Is.True);
            }

            [Test]
            public void StatusIsShipped_ReturnsFalse()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();
                // TODO: Use UpdateStatus instead of returning state machine and changing status on it
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Approved);
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Shipped);

                Assert.That(purchaseOrder.CanBeDeleted, Is.False);
            }
        }

        [TestFixture]
        public class UpdateStatusMethod
        {
            [Test]
            public void CancelledStatus_RemovesShipmentReference()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Shipment(new ShipmentBuilder().Build()).Build();

                Assert.That(purchaseOrder.Shipment, Is.Not.Null);
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Cancelled);

                Assert.That(purchaseOrder.Shipment, Is.Null);
            }
        }

        [TestFixture]
        public class CanShipmentBeUpdatedMethod
        {
            [Test]
            public void ShipmentIsNull_ReturnsTrue()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();

                Assert.That(purchaseOrder.Shipment, Is.Null);
                Assert.That(purchaseOrder.CanShipmentBeUpdated, Is.True);
            }

            [Test]
            public void ShipmentStatusIsShipped_ReturnsFalse()
            {
                var shipment = new ShipmentBuilder().Build();
                var purchaseOrder = new PurchaseOrderBuilder().Shipment(shipment).Build();
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);

                Assert.That(shipment.Status.CurrentState, Is.EqualTo(ShipmentStatus.State.Shipped));
                Assert.That(purchaseOrder.CanShipmentBeUpdated, Is.False);
            }

            [Test]
            public void ShipmentIsDelivered_ReturnsFalse()
            {
                var shipment = new ShipmentBuilder().Build();
                var purchaseOrder = new PurchaseOrderBuilder().Shipment(shipment).Build();
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);
                shipment.UpdateStatus(ShipmentStatus.Trigger.Delivered);

                Assert.That(shipment.Status.CurrentState, Is.EqualTo(ShipmentStatus.State.Delivered));
                Assert.That(purchaseOrder.CanShipmentBeUpdated, Is.False);
            }
        }
    }
}
