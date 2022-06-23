using AutoMapper;
using NUnit.Framework;
using PurchaseOrderTracker.Application.Features.Supplier.Commands;

namespace PurchaseOrderTracker.Application.Tests;

[TestFixture]
public class AutoMapperTests
{
    [Test]
    public void configuration_is_valid()
    {
        Mapper.Initialize(cfg => cfg.AddProfiles(typeof(CreateCommand)));
        Mapper.AssertConfigurationIsValid();
    }
}
