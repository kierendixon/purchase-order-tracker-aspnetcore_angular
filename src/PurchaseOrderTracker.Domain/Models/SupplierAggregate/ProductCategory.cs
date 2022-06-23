using System;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;

namespace PurchaseOrderTracker.Domain.Models.SupplierAggregate;

public class ProductCategory : Entity
{
    // Required for Entity Framework
    private ProductCategory()
    {
    }

    public ProductCategory(ProductCategoryName name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public int? SupplierId { get; private set; }
    public ProductCategoryName Name { get; set; }

    public override string ToString()
    {
        return $"{nameof(SupplierId)}: {SupplierId}, {nameof(Name)}: {Name}";
    }
}
