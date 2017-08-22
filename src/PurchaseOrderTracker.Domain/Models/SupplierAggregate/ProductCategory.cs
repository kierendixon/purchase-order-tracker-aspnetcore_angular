using System;

namespace PurchaseOrderTracker.Domain.Models.SupplierAggregate
{
    public class ProductCategory : Entity
    {
        // Required for Entity Framework
        private ProductCategory()
        {
        }

        public ProductCategory(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public int? SupplierId { get; private set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{nameof(SupplierId)}: {SupplierId}, {nameof(Name)}: {Name}";
        }
    }
}