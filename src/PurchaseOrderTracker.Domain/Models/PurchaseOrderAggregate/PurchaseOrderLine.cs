using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using System;

namespace PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;

public class PurchaseOrderLine : Entity
{
    // Required for EntityFramework
    private PurchaseOrderLine()
    {
    }

    public PurchaseOrderLine(Product product, decimal purchasePrice, int purchaseQty)
    {
        Product = product ?? throw new ArgumentNullException(nameof(product));
        PurchasePrice = purchasePrice;
        PurchaseQty = purchaseQty;
    }

    public Product Product { get; set; }
    public decimal PurchasePrice { get; set; }
    public int PurchaseQty { get; set; }
}
