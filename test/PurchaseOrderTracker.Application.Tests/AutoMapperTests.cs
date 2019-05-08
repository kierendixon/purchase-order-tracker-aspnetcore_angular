using AutoMapper;
using NUnit.Framework;
using PurchaseOrderTracker.Application.Features.Supplier.Commands;

namespace PurchaseOrderTracker.Application.Tests
{
    [TestFixture]
    public class AutoMapperTests
    {
        [Test]
        public void AutoMapperConfigurationAssertion_DoesNotThrowException()
        {
            Mapper.Initialize(cfg => cfg.AddProfiles(typeof(CreateCommand)));
            Mapper.AssertConfigurationIsValid();
        }
    }
}
