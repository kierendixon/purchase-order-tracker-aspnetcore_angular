using NUnit.Framework;
using PurchaseOrderTracker.Domain.Exceptions;

namespace PurchaseOrderTracker.Domain.Tests.Exceptions
{
    public class PurchaseOrderTrackerExceptionTests
    {
        [TestFixture]
        public class ConstructorWithOneParam
        {
            [Test]
            public void Always_AssignsValues()
            {
                var exMessage = "message";
                var ex = new PurchaseOrderTrackerException(exMessage);

                Assert.That(ex.Message, Is.EqualTo(exMessage));
            }
        }

        [TestFixture]
        public class ConstructorWithTwoParams
        {
            [Test]
            public void Always_AssignsValues()
            {
                var innerMessage = "Inner Message";
                var outerMessage = "Outer Message";
                var innerEx = new PurchaseOrderTrackerException(innerMessage);
                var outerEx = new PurchaseOrderTrackerException(outerMessage, innerEx);

                Assert.That(outerEx.InnerException, Is.SameAs(innerEx));
                Assert.That(outerEx.Message, Is.EqualTo(outerMessage));
            }
        }
    }
}
