using System;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Domain.Tests.Builders;

namespace PurchaseOrderTracker.Domain.Tests.Models.SupplierAggregate;

public class ProductCategoryTests
{
    [TestFixture]
    public class Constructor
    {
        [Test]
        public void assigns_values()
        {
            var category = new ProductCategoryBuilder().Build();
            Assert.That(category.Name.Value, Is.EqualTo("furniture"));
        }

        [Test]
        public void defaults_id()
        {
            var category = new ProductCategoryBuilder().Build();
            Assert.That(category.Id, Is.EqualTo(default(int)));
            Assert.That(category.SupplierId, Is.Null);
        }

        [Test]
        public void throws_ex_when_name_is_null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ProductCategory(null));
            Assert.That(ex.Message.ToLower(), Contains.Substring("name"));
        }
    }
}
