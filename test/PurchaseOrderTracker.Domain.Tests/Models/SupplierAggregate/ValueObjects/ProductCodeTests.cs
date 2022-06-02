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
            public void assigns_values()
            {
                var productCodeValue = "100386";
                var productCode = new ProductCode(productCodeValue);

                Assert.That(productCode.Value, Is.EqualTo(productCodeValue));
            }

            [Test]
            public void throws_ex_when_value_is_null()
            {
                Assert.Throws<ValidationException>(() => new ProductCode(null));
            }

            [Test]
            public void throws_ex_when_value_is_less_than_3_chars()
            {
                var productCodeValue = new string('1', 2);
                Assert.Throws<ValidationException>(() => new ProductCode(productCodeValue));
            }

            [Test]
            public void throws_ex_when_value_is_more_than_20_chars()
            {
                var productCodeValue = new string('1', 21);
                Assert.Throws<ValidationException>(() => new ProductCode(productCodeValue));
            }
        }

        [TestFixture]
        public class Equality
        {
            [Test]
            public void is_equal_to_another_product_code_with_the_same_value_when_using_equals_method()
            {
                Assert.That(new ProductCode("1234").Equals(new ProductCode("1234")));
            }

            [Test]
            public void is_not_equal_to_another_product_code_with_a_different_value_when_using_equals_method()
            {
                Assert.That(new ProductCode("1234").Equals(new ProductCode("12345")), Is.False);
            }

            [Test]
            public void is_equal_to_another_product_code_with_the_same_value_when_using_equality_operator()
            {
                Assert.That(new ProductCode("1234") == new ProductCode("1234"));
            }

            [Test]
            public void s_not_equal_to_another_product_code_with_a_different_value_when_using_equality_operator()
            {
                Assert.That(new ProductCode("1234") == new ProductCode("12345"), Is.False);
            }
        }
    }
}
