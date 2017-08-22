using System;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
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
                var product = new Product("code1", "name2", category, 999);

                Assert.That(product.ProdCode, Is.EqualTo("code1"));
                Assert.That(product.Name, Is.EqualTo("name2"));
                Assert.That(product.Category, Is.SameAs(category));
                Assert.That(product.Price, Is.EqualTo(999));
            }

            [Test]
            public void Always_DefaultIdentifier()
            {
                var category = new ProductCategoryBuilder().Build();
                var product = new Product("code1", "name2", category, 999);

                Assert.That(product.Id, Is.EqualTo(default(int)));
                Assert.That(product.SupplierId, Is.Null);
            }

            [Test]
            public void NullName_ThrowsArgumentNullException()
            {
                try
                {
                    var category = new ProductCategoryBuilder().Build();
                    var product = new Product("code1", null, category, 999);
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
                try
                {
                    var category = new ProductCategoryBuilder().Build();
                    var product = new Product(null, "name2", category, 999);
                    Assert.Fail("Expected exception to be thrown");
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.InstanceOf<ArgumentNullException>());
                    Assert.That(ex.Message.ToLower(), Contains.Substring("prodcode"));
                }
            }
        }
    }
}