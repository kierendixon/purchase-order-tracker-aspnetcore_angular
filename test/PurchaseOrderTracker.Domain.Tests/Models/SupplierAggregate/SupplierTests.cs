using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Domain.Tests.Builders;

namespace PurchaseOrderTracker.Domain.Tests.Models.SupplierAggregate
{
    public class SupplierTests
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void Always_AssignsValues()
            {
                var supplier = new SupplierBuilder().Build();

                Assert.That(supplier.Name, Is.EqualTo("supplierName"));
            }

            [Test]
            public void Always_DefaultIdentifier()
            {
                var supplier = new SupplierBuilder().Build();

                Assert.That(supplier.Id, Is.EqualTo(default(int)));
            }

            [Test]
            public void Always_DefaultProductCategoriesAsEmptyCollection()
            {
                var supplier = new SupplierBuilder().Build();

                Assert.That(supplier.ProductCategories, Is.Empty);
            }

            [Test]
            public void Always_DefaultProductsAsEmptyCollection()
            {
                var supplier = new SupplierBuilder().Build();

                Assert.That(supplier.Products, Is.Empty);
            }

            [Test]
            public void NullName_ThrowsArgumentNullException()
            {
                try
                {
                    var supplier = new SupplierBuilder().Name(null).Build();
                    Assert.Fail("Expected exception to be thrown");
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.InstanceOf<ArgumentNullException>());
                    Assert.That(ex.Message.ToLower(), Contains.Substring("name"));
                }
            }
        }

        [TestFixture]
        public class AddProductMethod
        {
            [Test]
            public void ProductCategoryBelongsToDifferentSupplier_ExceptionThrown()
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
            public void ProductCategoryIsSameSupplier_ProductAdded()
            {
                var category = new ProductCategoryBuilder()
                    .SupplierId(123)
                    .Build();
                var supplier = new SupplierBuilder()
                    .Id(123)
                    .ProductCategories(new List<ProductCategory>(new[] {category}))
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
            public void ProductBelongsToDifferentSupplier_ExceptionThrown()
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
            public void ProductBelongsToSupplier_ProductRemoved()
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
                    .ProductCategories(new List<ProductCategory>(new[] {productCategory}))
                    .Products(new List<Product>(new[] {product}))
                    .Build();

                supplier.RemoveProduct(product);

                Assert.That(supplier.Products.Any(), Is.False);
            }

            [Test]
            public void ProductBelongsToSupplierButNotInProductsList_ExceptionThrown()
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
}