using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate;
using System;
using System.Collections.Generic;

namespace PurchaseOrderTracker.Domain.Tests.Builders;

public class ShipmentBuilder
{
    private string _comments = "shipmentComments";
    private string _company = "shipmentCompany";
    private string _destinationAddress = "shipmentDestinationAddress";
    private DateTime _estimatedArrivalDate = DateTime.Now;
    private List<PurchaseOrder> _purchaseOrders = new();
    private int? _shipmentId;
    private decimal _shippingCost = 999;
    private string _trackingId = "shipmentTrackingId";

    public ShipmentBuilder ShipmentId(int id)
    {
        _shipmentId = id;
        return this;
    }

    public ShipmentBuilder TrackingId(string trackingId)
    {
        _trackingId = trackingId;
        return this;
    }

    public ShipmentBuilder Company(string company)
    {
        _company = company;
        return this;
    }

    public ShipmentBuilder EstimatedArrivalDate(DateTime eta)
    {
        _estimatedArrivalDate = eta;
        return this;
    }

    public ShipmentBuilder ShippingCost(decimal shippingCost)
    {
        _shippingCost = shippingCost;
        return this;
    }

    public ShipmentBuilder DestinationAddress(string destAddress)
    {
        _destinationAddress = destAddress;
        return this;
    }

    public ShipmentBuilder Comments(string comments)
    {
        _comments = comments;
        return this;
    }

    public ShipmentBuilder PurchaseOrders(List<PurchaseOrder> purchaseOrders)
    {
        _purchaseOrders = purchaseOrders;
        return this;
    }

    public Shipment Build()
    {
        var shipment = new Shipment(_trackingId, _company, _estimatedArrivalDate, _shippingCost,
            _destinationAddress, _comments);
        shipment.AddPurchaseOrders(_purchaseOrders);

        if (_shipmentId != null)
        {
            shipment.SetPrivatePropertyValue(nameof(Shipment.Id), _shipmentId);
        }

        return shipment;
    }
}
