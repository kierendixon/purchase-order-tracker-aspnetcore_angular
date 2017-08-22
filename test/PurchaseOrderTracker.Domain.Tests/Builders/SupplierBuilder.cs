using System.Collections.Generic;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;

namespace PurchaseOrderTracker.Domain.Tests.Builders
{
    public class SupplierBuilder
    {
        private List<ProductCategory> _categories = new List<ProductCategory>();
        private int? _id;
        private string _name = "supplierName";
        private List<Product> _products = new List<Product>();

        public SupplierBuilder Id(int id)
        {
            _id = id;
            return this;
        }

        public SupplierBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        public SupplierBuilder Products(List<Product> products)
        {
            _products = products;
            return this;
        }

        public SupplierBuilder ProductCategories(List<ProductCategory> categories)
        {
            _categories = categories;
            return this;
        }

        public Supplier Build()
        {
            var supplier = new Supplier(_name);
            if (_id != null)
                supplier.SetPrivatePropertyValue(nameof(supplier.Id), _id);
            supplier.AddCategorys(_categories);
            supplier.AddProducts(_products);

            return supplier;
        }
    }
}