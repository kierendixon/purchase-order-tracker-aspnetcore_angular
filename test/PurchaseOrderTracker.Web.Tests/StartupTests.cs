using AutoMapper;
using NUnit.Framework;

namespace PurchaseOrderTracker.Web.Tests
{
    [TestFixture]
    public class StartupTests
    {
        [Test]
        public void AutoMapperConfigurationAssertion_DoesNotThrowException()
        {
            Mapper.AssertConfigurationIsValid();
        }
    }
}