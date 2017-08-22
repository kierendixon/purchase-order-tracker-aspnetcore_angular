using NUnit.Framework;

namespace PurchaseOrderTracker.Web.Tests
{
    public class BaseAutomapperTest
    {
        [OneTimeSetUp]
        public void Init()
        {
            TestHelper.InitAutoMapper();
        }
    }
}
