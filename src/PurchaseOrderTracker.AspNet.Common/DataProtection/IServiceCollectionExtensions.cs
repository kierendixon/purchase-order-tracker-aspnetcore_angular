using System;
using System.IO;
using Azure.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PurchaseOrderTracker.AspNet.Common.Environment;

namespace PurchaseOrderTracker.AspNet.Common.DataProtection;

public static class IServiceCollectionExtensions
{
    public static void AddCustomDataProtection(this IServiceCollection services, IWebHostEnvironment env, DataProtectionOptions options)
    {
        // key ring will be stored at %LOCALAPPDATA% and encrypted with data protection API (DPAPI) by default
        var dataProtection = services.AddDataProtection()
            .SetApplicationName("PurchaseOrderTrackerApp");

        if (env.IsLocalDocker())
        {
            // not secure - keys will be saved to file system unencrypted
            dataProtection.PersistKeysToFileSystem(new DirectoryInfo(options.KeysDirectory));
            // add the following to encrypt keys
            // dataProtection.ProtectKeysWithCertificate(...)
        }
        else if (env.IsTestAzure())
        {
            dataProtection
                .PersistKeysToAzureBlobStorage(new Uri(options.StorageBlobUri), new DefaultAzureCredential())
                .ProtectKeysWithAzureKeyVault(new Uri(options.KeyVaultKeyIdentifier), new ManagedIdentityCredential());
        }
    }
}
