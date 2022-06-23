using System;
using System.Collections.Generic;
using System.Linq;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;

namespace PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;

/// <summary>
///     Aggregate root
/// </summary>
public class PurchaseOrder : Entity
{
    private readonly ICollection<PurchaseOrderLine> _lineItems = new List<PurchaseOrderLine>();

    public PurchaseOrder(OrderNo orderNo, Supplier supplier)
    {
        OrderNo = orderNo ?? throw new ArgumentNullException(nameof(orderNo));
        Supplier = supplier ?? throw new ArgumentNullException(nameof(supplier));

        Status = new PurchaseOrderStatus();
    }

    // Required for EntityFramework
    private PurchaseOrder()
    {
    }

    public OrderNo OrderNo { get; set; }
    public DateTime CreatedDate { get; private set; } = DateTime.Now;
    public Supplier Supplier { get; private set; }
    public Shipment Shipment { get; set; }
    public PurchaseOrderStatus Status { get; private set; }

    public IEnumerable<PurchaseOrderLine> LineItems => _lineItems.AsEnumerable();

    public bool CanBeDeleted => Status.CurrentState is not PurchaseOrderStatus.State.Shipped
                                and not PurchaseOrderStatus.State.Delivered;

    public bool IsApprovedOrLaterStatus => IsApproved || IsDelivered || IsShipped;
    public bool IsApproved => Status.CurrentState == PurchaseOrderStatus.State.Approved;
    public bool IsDelivered => Status.CurrentState == PurchaseOrderStatus.State.Delivered;
    public bool IsShipped => Status.CurrentState == PurchaseOrderStatus.State.Shipped;
    public bool IsCancelled => Status.CurrentState == PurchaseOrderStatus.State.Cancelled;
    public bool IsOpen => !IsCancelled && !IsDelivered;

    public bool CanShipmentBeUpdated => Shipment == null ||
                                        (Shipment.Status.CurrentState != ShipmentStatus.State.Shipped &&
                                         Shipment.Status.CurrentState != ShipmentStatus.State.Delivered);

    public bool CanTransitionToPendingApproval => Status.PermittedTriggers.Contains(PurchaseOrderStatus.Trigger
        .PendingApproval);

    public bool CanTransitionToApproved => Status.PermittedTriggers.Contains(PurchaseOrderStatus.Trigger
        .Approved);

    public bool CanTransitionToCancelled => Status.PermittedTriggers.Contains(PurchaseOrderStatus.Trigger
        .Cancelled);

    public void AddLineItem(PurchaseOrderLine line)
    {
        if (!ProductIsFromSameSupplier(line.Product))
        {
            throw new PurchaseOrderTrackerException(
                $"Line item product must be from the same supplier: {line.Product}");
        }

        _lineItems.Add(line);
    }

    public void AddLineItems(IEnumerable<PurchaseOrderLine> lineItems)
    {
        foreach (var lineItem in lineItems)
        {
            AddLineItem(lineItem);
        }
    }

    public void RemoveLineItem(PurchaseOrderLine line)
    {
        if (!_lineItems.Contains(line))
        {
            throw new PurchaseOrderTrackerException($"Line item is not part of this purchase order: {line}");
        }

        _lineItems.Remove(line);
    }

    private bool ProductIsFromSameSupplier(Product product)
    {
        return product.SupplierId == Supplier.Id;
    }

    public void ChangeSupplier(Supplier supplier)
    {
        if (!Status.IsStateBeforeApproved)
        {
            throw new PurchaseOrderTrackerException(
                $"Supplier can only be changed if purchase order status is before {PurchaseOrderStatus.State.Approved} status");
        }

        _lineItems.Clear();
        Supplier = supplier;
    }

    public void UpdateStatus(PurchaseOrderStatus.Trigger trigger)
    {
        Status.Fire(trigger);

        if (trigger == PurchaseOrderStatus.Trigger.Cancelled)
        {
            HandleCancelledStatusChange();
        }
    }

    private void HandleCancelledStatusChange()
    {
        Shipment = null;
    }
}
