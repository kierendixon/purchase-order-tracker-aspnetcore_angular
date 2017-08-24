using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.PlatformAbstractions;
using PurchaseOrderTracker.DAL;

namespace PurchaseOrderTracker.Web.Tests
{
    public static class TestHelper
    {
        public static void SetPrivatePropertyValue<T>(this object obj, string propName, T val)
        {
            var property = obj.GetType().GetTypeInfo().GetProperty(propName);
            property.SetValue(obj, val);
        }

        public static DbContextOptions<PoTrackerDbContext> GetDbOptions(string dbName)
        {
            var options = new DbContextOptionsBuilder<PoTrackerDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return options;
        }

        public static void InitAutoMapper()
        {
            Mapper.Initialize(cfg => cfg.AddProfiles(typeof(Startup)));
        }

        /// https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing
        /// <summary>
        /// Gets the full path to the target project path that we wish to test
        /// </summary>
        /// <param name="solutionRelativePath">
        /// The parent directory of the target project.
        /// e.g. src, samples, test, or test/Websites
        /// </param>
        /// <param name="startupAssembly">The target project's assembly.</param>
        /// <returns>The full path to the target project.</returns>
        public static string GetProjectPath(string solutionRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            // Find the folder which contains the solution file. We then use this information to find the target
            // project which we want to test.
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, "PurchaseOrderTracker.sln"));
                if (solutionFileInfo.Exists)
                {
                    return Path.GetFullPath(Path.Combine(directoryInfo.FullName, solutionRelativePath, projectName));
                }

                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Solution root could not be located using application root {applicationBasePath}.");
        }
    }
}