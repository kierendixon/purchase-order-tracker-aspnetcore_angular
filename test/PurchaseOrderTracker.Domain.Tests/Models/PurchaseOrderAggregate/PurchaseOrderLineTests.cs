using System;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Tests.Builders;

namespace PurchaseOrderTracker.Domain.Tests.Models.PurchaseOrderAggregate
{
    public class PurchaseOrderLineTests
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void assigns_values()
            {
                var product = new ProductBuilder().Build();
                var line = new PurchaseOrderLineBuilder()
                    .Product(product)
                    .PurchasePrice(100)
                    .PurchaseQty(10)
                    .Build();

                Assert.That(line.Product, Is.SameAs(product));
                Assert.That(line.PurchasePrice, Is.EqualTo(100));
                Assert.That(line.PurchaseQty, Is.EqualTo(10));
            }

            [Test]
            public void throws_arg_null_ex_when_product_is_null()
            {
                try
                {
                    new PurchaseOrderLineBuilder().Product(null).Build();
                    Assert.Fail("Expected exception to be thrown");
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.InstanceOf<ArgumentNullException>());
                    Assert.That(ex.Message.ToLower(), Contains.Substring("product"));
                }
            }
        }
    }
}
