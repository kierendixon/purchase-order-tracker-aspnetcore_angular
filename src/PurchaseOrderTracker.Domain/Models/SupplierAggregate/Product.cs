using System;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;

namespace PurchaseOrderTracker.Domain.Models.SupplierAggregate
{
    public class Product : Entity
    {
        // Required for Entity Framework
        private Product()
        {
        }

        public Product(ProductCode productCode, ProductName name, ProductCategory category, decimal price)
        {
            ProductCode = productCode ?? throw new ArgumentNullException(nameof(productCode));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Category = category;
            Price = price;
        }

        public int? SupplierId { get; private set; }
        public ProductCode ProductCode { get; set; }
        public ProductName Name { get; set; }
        public ProductCategory Category { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(ProductCode)}: {ProductCode}, {nameof(Name)}: {Name}, {nameof(Category)}: {Category}, {nameof(Price)}: {Price}, {nameof(SupplierId)}: {SupplierId}";
        }
    }
}
