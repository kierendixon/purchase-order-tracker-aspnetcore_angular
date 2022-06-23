using System;
using System.Collections.Generic;
using System.Linq;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate.ValueObjects;

namespace PurchaseOrderTracker.Domain.Models.ShipmentAggregate;

/// <summary>
///     Aggregate root
/// </summary>
public class Shipment : Entity
{
    // Required for Entity Framework
    private Shipment()
    {
    }

    public Shipment(string trackingId, string company, DateTime estimatedArrivalDate,
        decimal shippingCost, string destinationAddress, string comments)
    {
        TrackingId = trackingId ?? throw new ArgumentNullException(nameof(trackingId));
        Company = company ?? throw new ArgumentNullException(nameof(company));
        EstimatedArrivalDate = estimatedArrivalDate;
        ShippingCost = shippingCost;
        DestinationAddress = destinationAddress;
        Comments = comments;

        Status = new ShipmentStatus();
    }

    public string TrackingId { get; set; }
    public string Company { get; set; }
    public DateTime? EstimatedArrivalDate { get; set; }
    public string Comments { get; set; }
    public decimal? ShippingCost { get; set; }
    public string DestinationAddress { get; set; }
    public ShipmentStatus Status { get; private set; }
    public ICollection<PurchaseOrder> PurchaseOrders { get; private set; } = new List<PurchaseOrder>();

    /* Business Logic */

    public bool CanBeDeleted => Status.CurrentState != ShipmentStatus.State.Delivered;
    public bool IsDelivered => Status.CurrentState == ShipmentStatus.State.Delivered;
    public bool CanTransitionToAwaitingShipping => Status.PermittedTriggers.Contains(ShipmentStatus.Trigger.AwaitingShipping);
    public bool CanTransitionToShipped => Status.PermittedTriggers.Contains(ShipmentStatus.Trigger.Shipped);
    public bool CanTransitionToDelivered => Status.PermittedTriggers.Contains(ShipmentStatus.Trigger.Delivered);

    public bool CanBeAssignedToPurchaseOrder => Status.CurrentState is not ShipmentStatus.State.Shipped and
                                                not ShipmentStatus.State.Delivered;

    public void AddPurchaseOrder(PurchaseOrder order)
    {
        if (order.Shipment != null)
        {
            throw new PurchaseOrderTrackerException(
                $"Purchase order already assigned to a shipment. Shipment id: {order.Shipment.Id}");
        }

        if (order.Status.CurrentState != PurchaseOrderStatus.State.Approved)
        {
            throw new PurchaseOrderTrackerException(
                $"Only approved purchase orders can be added to a shipment. Purchase order status: '{order.Status.CurrentState}'");
        }

        PurchaseOrders.Add(order);
    }

    public void AddPurchaseOrders(List<PurchaseOrder> orders)
    {
        foreach (var order in orders)
        {
            AddPurchaseOrder(order);
        }
    }

    public bool IsDelayed()
    {
        return Status.CurrentState != ShipmentStatus.State.Delivered
               && EstimatedArrivalDate.HasValue && EstimatedArrivalDate.Value.Date < DateTime.Today;
    }

    public bool IsDelayedMoreThan7Days()
    {
        return Status.CurrentState != ShipmentStatus.State.Delivered
               && EstimatedArrivalDate.HasValue && EstimatedArrivalDate.Value.Date < DateTime.Today.AddDays(-7);
    }

    public bool IsScheduledForDeliveryToday()
    {
        return EstimatedArrivalDate.HasValue && EstimatedArrivalDate.Value.Date.Equals(DateTime.Today);
    }

    public void UpdateStatus(ShipmentStatus.Trigger trigger)
    {
        Status.Fire(trigger);

        if (trigger == ShipmentStatus.Trigger.Shipped)
        {
            HandleShippedStatusChange();
        }
        else if (trigger == ShipmentStatus.Trigger.Delivered)
        {
            HandleDeliveredStatusChange();
        }
    }

    private void HandleShippedStatusChange()
    {
        foreach (var order in PurchaseOrders)
        {
            order.UpdateStatus(PurchaseOrderStatus.Trigger.Shipped);
        }
    }

    private void HandleDeliveredStatusChange()
    {
        foreach (var order in PurchaseOrders)
        {
            order.UpdateStatus(PurchaseOrderStatus.Trigger.Delivered);
        }
    }
}
