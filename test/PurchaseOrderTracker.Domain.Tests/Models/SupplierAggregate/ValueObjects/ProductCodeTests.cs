using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;

namespace PurchaseOrderTracker.Domain.Tests.Models.SupplierAggregate.ValueObjects
{
    public class ProductCodeTests
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void Always_AssignsValues()
            {
                var productCodeValue = "100386";
                var productCode = new ProductCode(productCodeValue);

                Assert.That(productCode.Value, Is.EqualTo(productCodeValue));
            }

            [Test]
            public void NullValue_ThrowsValidationException()
            {
                Assert.Throws<ValidationException>(() => new ProductCode(null));
            }

            [Test]
            public void LessThan3Chars_ThrowsValidationException()
            {
                var productCodeValue = new string('1', 2);
                Assert.Throws<ValidationException>(() => new ProductCode(productCodeValue));
            }

            [Test]
            public void MoreThan20Chars_ThrowsValidationException()
            {
                var productCodeValue = new string('1', 21);
                Assert.Throws<ValidationException>(() => new ProductCode(productCodeValue));
            }

            [Test]
            public void is_equal_to_another_product_code_with_the_same_value_when_using_equals_method()
            {
                Assert.That(new ProductCode("1234").Equals(new ProductCode("1234")));
            }

            [Test]
            public void is_equal_to_another_product_code_with_the_same_value_when_using_equality_operator()
            {
                Assert.That(new ProductCode("1234") == new ProductCode("1234"));
            }
        }
    }
}
