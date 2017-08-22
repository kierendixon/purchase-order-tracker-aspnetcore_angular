using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

namespace PurchaseOrderTracker.Web.Tests
{
    [TestFixture]
    public class StartupTests
    {
        [Test]
        public void AutoMapperConfigurationAssertion_DoesNotThrowException()
        {
            TestHelper.InitAutoMapper();
            Mapper.AssertConfigurationIsValid();
        }
    }
}