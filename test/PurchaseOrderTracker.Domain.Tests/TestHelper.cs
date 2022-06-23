using System.Reflection;

namespace PurchaseOrderTracker.Domain.Tests;

public static class TestHelper
{
    public static void SetPrivatePropertyValue<T>(this object obj, string propName, T val)
    {
        var property = obj.GetType().GetTypeInfo().GetProperty(propName);
        property.SetValue(obj, val);
    }
}
