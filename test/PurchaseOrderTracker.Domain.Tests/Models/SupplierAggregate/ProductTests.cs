using System;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Tests.Builders;

namespace PurchaseOrderTracker.Domain.Tests.Models.SupplierAggregate
{
    public class ProductTests
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void Always_AssignsValues()
            {
                var category = new ProductCategoryBuilder().Build();
                var product = new Product(new ProductCode("code1"), new ProductName("name2"), category, 999);

                Assert.That(product.ProductCode.Value, Is.EqualTo("code1"));
                Assert.That(product.Name.Value, Is.EqualTo("name2"));
                Assert.That(product.Category, Is.SameAs(category));
                Assert.That(product.Price, Is.EqualTo(999));
            }

            [Test]
            public void Always_DefaultIdentifier()
            {
                var category = new ProductCategoryBuilder().Build();
                var product = new Product(new ProductCode("code1"), new ProductName("name2"), category, 999);

                Assert.That(product.Id, Is.EqualTo(default(int)));
                Assert.That(product.SupplierId, Is.Null);
            }

            [Test]
            public void NullName_ThrowsArgumentNullException()
            {
                try
                {
                    var category = new ProductCategoryBuilder().Build();
                    var product = new Product(new ProductCode("code1"), null, category, 999);
                    Assert.Fail("Expected exception to be thrown");
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.InstanceOf<ArgumentNullException>());
                    Assert.That(ex.Message.ToLower(), Contains.Substring("name"));
                }
            }

            [Test]
            public void NullProdCode_ThrowsArgumentNullException()
            {
                var category = new ProductCategoryBuilder().Build();
                Assert.Throws<ArgumentNullException>(() =>
                    new Product(null, new ProductName("name2"), category, 999));
            }
        }
    }
}
