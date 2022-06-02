using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Tests.Builders;

namespace PurchaseOrderTracker.Domain.Tests.Models.PurchaseOrderAggregate
{
    public class PurchaseOrderTests
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void assigns_values()
            {
                var supplier = new SupplierBuilder().Build();
                var purchaseOrder = new PurchaseOrder(new OrderNo("orderNo"), supplier);

                Assert.That(purchaseOrder.OrderNo.Value, Is.EqualTo("orderNo"));
                Assert.That(purchaseOrder.Supplier, Is.SameAs(supplier));
            }

            [Test]
            public void defaults_created_date_to_now()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();

                // compare date component only (ignore time) to ensure this test always passes
                // otherwise the CreateDate property would need to be abstracted so that we can
                // always set the same value as part of this test case
                Assert.That(purchaseOrder.CreatedDate.Date, Is.EqualTo(DateTime.Today));
            }

            [Test]
            public void defaults_id()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();

                Assert.That(purchaseOrder.Id, Is.EqualTo(default(int)));
            }

            [Test]
            public void defaults_line_items_as_empty_collection()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();

                Assert.That(purchaseOrder.LineItems, Is.Empty);
            }

            [Test]
            public void defaults_status_as_draft()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();

                Assert.That(purchaseOrder.Status.CurrentState, Is.EqualTo(PurchaseOrderStatus.State.Draft));
            }

            [Test]
            public void throws_null_arg_ex_when_order_no_is_null()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new PurchaseOrder(null, new SupplierBuilder().Build()));
                Assert.That(ex.Message.ToLower(), Contains.Substring("supplier"));
            }

            [Test]
            public void throws_null_arg_ex_when_supplier_is_null()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new PurchaseOrder(new OrderNo("orderNo"), null));
                Assert.That(ex.Message.ToLower(), Contains.Substring("supplier"));
            }
        }

        [TestFixture]
        public class AddLineItemMethod
        {
            [Test]
            public void throws_ex_when_product_is_from_a_different_supplier()
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
            public void adds_item_when_product_is_from_the_same_supplier()
            {
                var supplier = new SupplierBuilder().Id(123).Build();
                var purchaseOrder = new PurchaseOrderBuilder()
                    .Supplier(supplier)
                    .Build();
                var lineItem = new PurchaseOrderLineBuilder()
                    .Product(new ProductBuilder().SupplierId(123).Build())
                    .Build();

                Assert.DoesNotThrow(() => purchaseOrder.AddLineItem(lineItem));
            }
        }

        [TestFixture]
        public class ChangeSupplierMethod
        {
            [Test]
            public void throws_ex_when_status_is_delivered()
            {
                var purchaseOrder = new PurchaseOrderBuilder()
                    .Supplier(new SupplierBuilder().Id(111).Build())
                    .Build();
                var newSupplier = new SupplierBuilder().Id(222).Build();
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Approved);
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Shipped);
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Delivered);

                Assert.That(purchaseOrder.Status.CurrentState, Is.EqualTo(PurchaseOrderStatus.State.Delivered));
                Assert.Throws<PurchaseOrderTrackerException>(() => purchaseOrder.ChangeSupplier(newSupplier));
            }

            [Test]
            public void throws_ex_when_status_is_cancelled()
            {
                var purchaseOrder = new PurchaseOrderBuilder()
                    .Supplier(new SupplierBuilder().Id(111).Build())
                    .Build();
                var newSupplier = new SupplierBuilder().Id(222).Build();
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Cancelled);

                Assert.That(purchaseOrder.Status.CurrentState, Is.EqualTo(PurchaseOrderStatus.State.Cancelled));
                Assert.Throws<PurchaseOrderTrackerException>(() => purchaseOrder.ChangeSupplier(newSupplier));
            }

            [Test]
            public void changes_supplier_and_clears_line_items()
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
                        })
                    .Build();
                var newSupplier = new SupplierBuilder().Id(222).Build();

                purchaseOrder.ChangeSupplier(newSupplier);

                Assert.That(purchaseOrder.IsOpen);
                Assert.That(purchaseOrder.LineItems.Any(), Is.False);
                Assert.That(purchaseOrder.Supplier, Is.SameAs(newSupplier));
            }
        }

        [TestFixture]
        public class CanBeDeletedMethod
        {
            [Test]
            public void returns_false_when_status_is_shipped()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();
                // TODO: Use UpdateStatus instead of returning state machine and changing status on it
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Approved);
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Shipped);

                Assert.That(purchaseOrder.CanBeDeleted, Is.False);
            }

            [Test]
            public void returns_false_when_status_is_delivered()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Approved);
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Shipped);
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Delivered);

                Assert.That(purchaseOrder.CanBeDeleted, Is.False);
            }

            [Test]
            public void returns_true_when_status_is_draft()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();
                Assert.That(purchaseOrder.CanBeDeleted, Is.True);
            }

            [Test]
            public void returns_true_when_status_is_pending_approval()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.PendingApproval);
                Assert.That(purchaseOrder.CanBeDeleted, Is.True);
            }

            [Test]
            public void returns_true_when_status_is_pending_approved()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.PendingApproval);
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Approved);
                Assert.That(purchaseOrder.CanBeDeleted, Is.True);
            }

            [Test]
            public void returns_true_when_status_is_pending_cancelled()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();
                purchaseOrder.Status.Fire(PurchaseOrderStatus.Trigger.Cancelled);
                Assert.That(purchaseOrder.CanBeDeleted, Is.True);
            }
        }

        [TestFixture]
        public class UpdateStatusMethod
        {
            [Test]
            public void removes_shipment_reference_when_cancelled()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Shipment(new ShipmentBuilder().Build()).Build();
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Cancelled);

                Assert.That(purchaseOrder.Shipment, Is.Null);
            }
        }

        [TestFixture]
        public class CanShipmentBeUpdatedMethod
        {
            [Test]
            public void returns_true_when_shipment_is_null()
            {
                var purchaseOrder = new PurchaseOrderBuilder().Build();

                Assert.That(purchaseOrder.Shipment, Is.Null);
                Assert.That(purchaseOrder.CanShipmentBeUpdated, Is.True);
            }

            [Test]
            public void returns_false_when_shipment_is_shipped()
            {
                var shipment = new ShipmentBuilder().Build();
                var purchaseOrder = new PurchaseOrderBuilder().Shipment(shipment).Build();
                purchaseOrder.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);

                Assert.That(shipment.Status.CurrentState, Is.EqualTo(ShipmentStatus.State.Shipped));
                Assert.That(purchaseOrder.CanShipmentBeUpdated, Is.False);
            }

            [Test]
            public void returns_false_when_shipment_is_delivered()
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
