using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;

namespace PurchaseOrderTracker.Domain.Tests.Models.PurchaseOrderAggregate.ValueObjects;

public class OrderNoTests
{
    [TestFixture]
    public class Constructor
    {
        [Test]
        public void constructor_assigns_values_when_there_is_one_param()
        {
            var orderNoValue = "orderNo";
            var orderNo = new OrderNo(orderNoValue);

            Assert.That(orderNo.Value, Is.EqualTo(orderNoValue));
        }

        [Test]
        public void constructor_throws_validation_ex_when_value_is_null()
        {
            Assert.Throws<ValidationException>(() => new OrderNo(null));
        }

        [Test]
        public void constructor_throws_validation_ex_when_value_is_less_than_3_chars()
        {
            var orderNoValue = new string('o', 2);
            Assert.Throws<ValidationException>(() => new OrderNo(orderNoValue));
        }

        [Test]
        public void constructor_throws_validation_ex_when_value_is_more_than_150_chars()
        {
            var orderNoValue = new string('o', 151);
            Assert.Throws<ValidationException>(() => new OrderNo(orderNoValue));
        }
    }

    [TestFixture]
    public class Equality
    {
        [Test]
        public void is_equal_to_another_order_no_with_the_same_value_when_using_equals_method()
        {
            Assert.That(new OrderNo("1234").Equals(new OrderNo("1234")));
        }

        [Test]
        public void is_not_equal_to_another_order_no_with_a_different_value_when_using_equals_method()
        {
            Assert.That(new OrderNo("1234").Equals(new OrderNo("12345")), Is.False);
        }

        [Test]
        public void is_equal_to_another_order_no_with_the_same_value_when_using_equality_operator()
        {
            Assert.That(new OrderNo("1234") == new OrderNo("1234"));
        }

        [Test]
        public void is_not_equal_to_another_order_no_with_a_different_value_when_using_equality_operator()
        {
            Assert.That(new OrderNo("1234") == new OrderNo("12345"), Is.False);
        }
    }
}
