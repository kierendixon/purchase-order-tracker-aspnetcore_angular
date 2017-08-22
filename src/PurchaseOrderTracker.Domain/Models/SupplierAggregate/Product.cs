using System;

namespace PurchaseOrderTracker.Domain.Models.SupplierAggregate
{
    public class Product : Entity
    {
        // Required for Entity Framework
        private Product()
        {
        }

        public Product(string prodCode, string name, ProductCategory category, double price)
        {
            ProdCode = prodCode ?? throw new ArgumentNullException(nameof(prodCode));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Category = category;
            Price = price;
        }

        public int? SupplierId { get; private set; }
        public string ProdCode { get; set; }
        public string Name { get; set; }
        public ProductCategory Category { get; set; }
        public double? Price { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(ProdCode)}: {ProdCode}, {nameof(Name)}: {Name}, {nameof(Category)}: {Category}, {nameof(Price)}: {Price}, {nameof(SupplierId)}: {SupplierId}";
        }
    }
}