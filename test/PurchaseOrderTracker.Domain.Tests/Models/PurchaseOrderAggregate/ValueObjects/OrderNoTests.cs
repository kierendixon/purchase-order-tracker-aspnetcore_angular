using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;

namespace PurchaseOrderTracker.Domain.Tests.Models.PurchaseOrderAggregate.ValueObjects
{
    public class OrderNoTests
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void Always_AssignsValues()
            {
                var orderNoValue = "orderNo";
                var orderNo = new OrderNo(orderNoValue);

                Assert.That(orderNo.Value, Is.EqualTo(orderNoValue));
            }

            [Test]
            public void NullValue_ThrowsValidationException()
            {
                Assert.Throws<ValidationException>(() => new OrderNo(null));
            }

            [Test]
            public void LessThan3Chars_ThrowsValidationException()
            {
                var orderNoValue = new string('o', 2);
                Assert.Throws<ValidationException>(() => new OrderNo(orderNoValue));
            }

            [Test]
            public void MoreThan150Chars_ThrowsValidationException()
            {
                var orderNoValue = new string('o', 151);
                Assert.Throws<ValidationException>(() => new OrderNo(orderNoValue));
            }
        }
    }
}
