using System.Collections.Generic;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;

namespace PurchaseOrderTracker.Domain.Tests.Builders;

public class PurchaseOrderBuilder
{
    private List<PurchaseOrderLine> _lineItems = new();
    private string _orderNo = "orderNo";
    private Shipment _shipment;
    private Supplier _supplier = new SupplierBuilder().Build();

    public PurchaseOrderBuilder OrderNo(string orderNo)
    {
        _orderNo = orderNo;
        return this;
    }

    public PurchaseOrderBuilder Supplier(Supplier supplier)
    {
        _supplier = supplier;
        return this;
    }

    public PurchaseOrderBuilder Shipment(Shipment shipment)
    {
        _shipment = shipment;
        return this;
    }

    public PurchaseOrderBuilder LineItems(List<PurchaseOrderLine> lineItems)
    {
        _lineItems = lineItems;
        return this;
    }

    public PurchaseOrder Build()
    {
        var purchaseOrder = new PurchaseOrder(new OrderNo(_orderNo), _supplier);
        purchaseOrder.AddLineItems(_lineItems);

        if (_shipment != null)
        {
            purchaseOrder.SetPrivatePropertyValue(nameof(purchaseOrder.Shipment), _shipment);
        }

        return purchaseOrder;
    }
}
