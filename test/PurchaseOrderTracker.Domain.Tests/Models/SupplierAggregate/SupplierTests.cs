using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Domain.Tests.Builders;

namespace PurchaseOrderTracker.Domain.Tests.Models.SupplierAggregate;

public class SupplierTests
{
    [TestFixture]
    public class Constructor
    {
        [Test]
        public void assigns_values()
        {
            var supplier = new SupplierBuilder().Build();

            Assert.That(supplier.Name.Value, Is.EqualTo("supplierName"));
        }

        [Test]
        public void defaults_id()
        {
            var supplier = new SupplierBuilder().Build();

            Assert.That(supplier.Id, Is.EqualTo(default(int)));
        }

        [Test]
        public void defaults_product_categories_as_empty_collection()
        {
            var supplier = new SupplierBuilder().Build();

            Assert.That(supplier.ProductCategories, Is.Empty);
        }

        [Test]
        public void defaults_products_as_empty_collection()
        {
            var supplier = new SupplierBuilder().Build();

            Assert.That(supplier.Products, Is.Empty);
        }

        [Test]
        public void throws_ex_when_name_is_null()
        {
            Assert.Throws<ValidationException>(() =>
                new SupplierBuilder().Name(null).Build());
        }
    }

    [TestFixture]
    public class AddProductMethod
    {
        [Test]
        public void throws_ex_when_product_category_belongs_to_a_different_supplier()
        {
            var category = new ProductCategoryBuilder()
                .SupplierId(123)
                .Build();
            var supplier = new SupplierBuilder()
                .Id(789)
                .Build();
            var product = new ProductBuilder().Category(category).Build();

            Assert.Throws<PurchaseOrderTrackerException>(() => supplier.AddProduct(product));
        }

        [Test]
        public void adds_product_when_product_category_is_the_same_supplier()
        {
            var category = new ProductCategoryBuilder()
                .SupplierId(123)
                .Build();
            var supplier = new SupplierBuilder()
                .Id(123)
                .ProductCategories(new List<ProductCategory>(new[] { category }))
                .Build();
            var product = new ProductBuilder().Category(category).Build();

            supplier.AddProduct(product);
            Assert.That(supplier.Products.Contains(product), Is.True);
        }
    }

    [TestFixture]
    public class RemoveProductMethod
    {
        [Test]
        public void throws_ex_when_product_category_belongs_to_a_different_supplier()
        {
            var product = new ProductBuilder()
                .SupplierId(123)
                .Build();
            var supplier = new SupplierBuilder()
                .Id(789)
                .Build();

            Assert.Throws<PurchaseOrderTrackerException>(() => supplier.RemoveProduct(product));
        }

        [Test]
        public void adds_product_when_product_category_is_the_same_supplier()
        {
            var productCategory = new ProductCategoryBuilder()
                .SupplierId(123)
                .Build();
            var product = new ProductBuilder()
                .SupplierId(123)
                .Category(productCategory)
                .Build();
            var supplier = new SupplierBuilder()
                .Id(123)
                .ProductCategories(new List<ProductCategory>(new[] { productCategory }))
                .Products(new List<Product>(new[] { product }))
                .Build();

            supplier.RemoveProduct(product);

            Assert.That(supplier.Products.Any(), Is.False);
        }

        [Test]
        public void throws_ex_when_product_category_belongs_to_the_same_supplie_but_is_not_in_products_list()
        {
            var product = new ProductBuilder()
                .SupplierId(123)
                .Build();
            var supplier = new SupplierBuilder()
                .Id(123)
                .Build();

            Assert.Throws<PurchaseOrderTrackerException>(() => supplier.RemoveProduct(product));
        }
    }
}
