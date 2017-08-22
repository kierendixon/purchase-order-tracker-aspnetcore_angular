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
            public void Always_AssignsValues()
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
            public void NullProduct_ThrowsArgumentNullException()
            {
                try
                {
                    var line = new PurchaseOrderLineBuilder().Product(null).Build();
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