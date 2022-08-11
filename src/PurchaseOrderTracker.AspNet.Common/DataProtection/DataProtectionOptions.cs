namespace PurchaseOrderTracker.AspNet.Common.DataProtection;

public class DataProtectionOptions
{
    public static string Position { get; } = "DataProtection";

    public string KeysDirectory { get; set; }
    public string StorageBlobUri { get; set; }
    public string KeyVaultKeyIdentifier { get; set; }
}
