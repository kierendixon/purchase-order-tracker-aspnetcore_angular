using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PurchaseOrderTracker.Web.Identity
{
    public static class DbInitializer
    {
        private const string BasicUserUserName = "basic";
        private const string BasicUserPassword = "basic";

        public static void Initialize(IdentityContext context, UserManager<IdentityUser> userManager)
        {
            try
            {
                if (context.Users.Any())
                    return;
            }
            catch (Exception)
            {
                // Ignore error - database will be created
            }

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Task.Run(async () =>
            {
                var basicUser = new IdentityUser(BasicUserUserName);
                var result = await userManager.CreateAsync(basicUser, BasicUserPassword);
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create user while initializing identity database: {result.Errors.Select(e => e.Description)}");
                }
            }).GetAwaiter().GetResult();
        }
    }
}