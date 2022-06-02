using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;

namespace PurchaseOrderTracker.Domain.Tests.Models.SupplierAggregate.ValueObjects
{
    public class ProductNameTests
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void assigns_values()
            {
                var productNameValue = "100386";
                var productName = new ProductName(productNameValue);

                Assert.That(productName.Value, Is.EqualTo(productNameValue));
            }

            [Test]
            public void throws_validation_ex_when_value_is_null()
            {
                Assert.Throws<ValidationException>(() => new ProductName(null));
            }

            [Test]
            public void throws_validation_ex_when_value_is_less_than_3_chars()
            {
                var productNameValue = new string('1', 2);
                Assert.Throws<ValidationException>(() => new ProductName(productNameValue));
            }

            [Test]
            public void throws_validation_ex_when_value_is_more_than_150_chars()
            {
                var productNameValue = new string('1', 151);
                Assert.Throws<ValidationException>(() => new ProductName(productNameValue));
            }
        }

        [TestFixture]
        public class Equality
        {
            [Test]
            public void is_equal_to_another_product_name_with_the_same_value_when_using_equals_method()
            {
                Assert.That(new ProductName("1234").Equals(new ProductName("1234")));
            }

            [Test]
            public void is_not_equal_to_another_product_name_with_a_different_value_when_using_equals_method()
            {
                Assert.That(new ProductName("1234").Equals(new ProductName("12345")), Is.False);
            }

            [Test]
            public void is_equal_to_another_product_name_with_the_same_value_when_using_equality_operator()
            {
                Assert.That(new ProductName("1234") == new ProductName("1234"));
            }

            [Test]
            public void s_not_equal_to_another_product_name_with_a_different_value_when_using_equality_operator()
            {
                Assert.That(new ProductName("1234") == new ProductName("12345"), Is.False);
            }
        }
    }
}

