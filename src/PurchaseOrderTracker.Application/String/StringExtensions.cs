#pragma warning disable CA1716 // Identifiers should not match keywords
namespace PurchaseOrderTracker.Application.String
#pragma warning restore CA1716 // Identifiers should not match keywords
{
    public static class StringExtensions
    {
        public static string FirstCharacterToLower(this string str)
        {
            return string.IsNullOrEmpty(str) || char.IsLower(str, 0) ? str : char.ToLower(str[0]) + str[1..];
        }
    }
}
