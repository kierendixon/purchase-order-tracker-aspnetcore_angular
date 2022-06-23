using System;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Tests.Builders;

namespace PurchaseOrderTracker.Domain.Tests.Models.SupplierAggregate;

public class ProductTests
{
    [TestFixture]
    public class Constructor
    {
        [Test]
        public void assigns_values()
        {
            var category = new ProductCategoryBuilder().Build();
            var product = new Product(new ProductCode("code1"), new ProductName("name2"), category, 999);

            Assert.That(product.ProductCode.Value, Is.EqualTo("code1"));
            Assert.That(product.Name.Value, Is.EqualTo("name2"));
            Assert.That(product.Category, Is.SameAs(category));
            Assert.That(product.Price, Is.EqualTo(999));
        }

        [Test]
        public void defaults_identifier()
        {
            var category = new ProductCategoryBuilder().Build();
            var product = new Product(new ProductCode("code1"), new ProductName("name2"), category, 999);

            Assert.That(product.Id, Is.EqualTo(default(int)));
            Assert.That(product.SupplierId, Is.Null);
        }

        [Test]
        public void throws_ex_when_name_is_null()
        {
            var category = new ProductCategoryBuilder().Build();
            var ex = Assert.Throws<ArgumentNullException>(() => new Product(new ProductCode("code1"), null, category, 999));
            Assert.That(ex.Message.ToLower(), Contains.Substring("name"));
        }

        [Test]
        public void throws_ex_when_product_code_is_null()
        {
            var category = new ProductCategoryBuilder().Build();
            Assert.Throws<ArgumentNullException>(() =>
                new Product(null, new ProductName("name2"), category, 999));
        }
    }
}
