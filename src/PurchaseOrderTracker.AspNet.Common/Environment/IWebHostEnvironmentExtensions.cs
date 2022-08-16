using System;
using Microsoft.AspNetCore.Hosting;

namespace PurchaseOrderTracker.AspNet.Common.Environment;

public static class IWebHostEnvironmentExtensions
{
    // identifies an environment running locally on a developer's machine using Docker containers
    public static bool IsLocalDocker(this IWebHostEnvironment env)
    {
        return env.EnvironmentName.Equals("LocalDocker", StringComparison.OrdinalIgnoreCase);
    }

    // identifies the Azure test environment
    public static bool IsTestAzure(this IWebHostEnvironment env)
    {
        return env.EnvironmentName.Equals("TestAzure", StringComparison.OrdinalIgnoreCase);
    }
}
