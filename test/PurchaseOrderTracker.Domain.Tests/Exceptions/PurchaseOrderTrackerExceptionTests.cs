using NUnit.Framework;
using PurchaseOrderTracker.Domain.Exceptions;

namespace PurchaseOrderTracker.Domain.Tests.Exceptions;

[TestFixture]
public class PurchaseOrderTrackerExceptionTests
{
    [Test]
    public void constructor_assigns_values_when_there_is_one_param()
    {
        var exMessage = "message";
        var ex = new PurchaseOrderTrackerException(exMessage);

        Assert.That(ex.Message, Is.EqualTo(exMessage));
    }

    [Test]
    public void constructor_assigns_values_when_there_are_two_params()
    {
        var innerMessage = "Inner Message";
        var outerMessage = "Outer Message";
        var innerEx = new PurchaseOrderTrackerException(innerMessage);
        var outerEx = new PurchaseOrderTrackerException(outerMessage, innerEx);

        Assert.That(outerEx.InnerException, Is.SameAs(innerEx));
        Assert.That(outerEx.Message, Is.EqualTo(outerMessage));
    }
}
