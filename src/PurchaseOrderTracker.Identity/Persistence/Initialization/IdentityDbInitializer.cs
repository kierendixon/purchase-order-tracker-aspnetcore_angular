using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;

namespace PurchaseOrderTracker.Identity.Persistence.Initialization
{
    public static class IdentityDbInitializer
    {
        private const string BasicUserUserName = "basic";
        private const string BasicUserPassword = "basic";
        private const string SuperUserUserName = "super";
        private const string SuperUserPassword = "super";

        // TODO IUserManager was previously IUserManager<ApplicationUser> when using the aspnetcore identity version
        public static void Initialize(IdentityDbContext context, UserManager<ApplicationUser> userManager)
        {
            // uncomment to delete database on every startup
            //context.Database.EnsureDeleted();
            var created = DbInitializerHelper.EnsureDatabaseCreated(context);

            if (created)
            {
                Task.Run(async () =>
                {
                    await CreateUser(userManager, BasicUserUserName, BasicUserPassword);
                    await CreateUser(userManager, SuperUserUserName, SuperUserPassword);

                    // TODO temp
                    for (var i = 0; i < 50; i++)
                    {
                        await CreateUser(userManager, $"TestUser{i}", $"testuser{1}");
                    }
                }).GetAwaiter().GetResult();
            }
        }

        private static async Task CreateUser(UserManager<ApplicationUser> userManager, string username, string password)
        {
            var user = new ApplicationUser(username);
            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                throw new PurchaseOrderTrackerException($"Failed to create user while initializing identity database: {errors}");
            }
        }
    }
}
