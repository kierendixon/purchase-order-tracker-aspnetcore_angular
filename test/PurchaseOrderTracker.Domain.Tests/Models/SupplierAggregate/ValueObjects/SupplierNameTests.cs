using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;

namespace PurchaseOrderTracker.Domain.Tests.Models.SupplierAggregate.ValueObjects
{
    public class SupplierNameTests
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void assigns_values()
            {
                var supplierNameValue = "100386";
                var supplierName = new SupplierName(supplierNameValue);

                Assert.That(supplierName.Value, Is.EqualTo(supplierNameValue));
            }

            [Test]
            public void throws_ex_when_value_is_null()
            {
                Assert.Throws<ValidationException>(() => new SupplierName(null));
            }

            [Test]
            public void throws_ex_when_value_is_less_than_3_chars()
            {
                var supplierNameValue = new string('1', 2);
                Assert.Throws<ValidationException>(() => new SupplierName(supplierNameValue));
            }

            [Test]
            public void throws_ex_when_value_is_more_than_150_chars()
            {
                var supplierNameValue = new string('1', 151);
                Assert.Throws<ValidationException>(() => new SupplierName(supplierNameValue));
            }
        }

        [TestFixture]
        public class Equality
        {
            [Test]
            public void is_equal_to_another_category_name_with_the_same_value_when_using_equals_method()
            {
                Assert.That(new SupplierName("1234").Equals(new SupplierName("1234")));
            }

            [Test]
            public void is_not_equal_to_another_category_name_with_a_different_value_when_using_equals_method()
            {
                Assert.That(new SupplierName("1234").Equals(new SupplierName("12345")), Is.False);
            }

            [Test]
            public void is_equal_to_another_category_name_with_the_same_value_when_using_equality_operator()
            {
                Assert.That(new SupplierName("1234") == new SupplierName("1234"));
            }

            [Test]
            public void is_not_equal_to_another_category_name_with_a_different_value_when_using_equality_operator()
            {
                Assert.That(new SupplierName("1234") == new SupplierName("12345"), Is.False);
            }
        }
    }
}
