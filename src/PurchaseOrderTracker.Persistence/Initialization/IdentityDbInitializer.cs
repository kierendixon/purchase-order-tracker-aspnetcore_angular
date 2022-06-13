using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;

namespace PurchaseOrderTracker.Persistence.Initialization
{
    public static class IdentityDbInitializer
    {
        private const string BasicUserUserName = "basic";
        private const string BasicUserPassword = "basic";
        private const string SuperUserUserName = "super";
        private const string SuperUserPassword = "super";

        public static void Initialize(IdentityDbContext context, UserManager<ApplicationUser> userManager)
        {
            // uncomment to delete database on every startup
            // context.Database.EnsureDeleted();
            var created = DbInitializerHelper.EnsureDatabaseCreated(context);

            if (created)
            {
                CreateUser(userManager, BasicUserUserName, BasicUserPassword).Wait();
                CreateUser(userManager, SuperUserUserName, SuperUserPassword, true).Wait();

                for (var i = 0; i < 50; i++)
                {
                    CreateUser(userManager, $"TestUser{i}", $"testuser{1}").Wait();
                }
            }
        }

        private static async Task CreateUser(UserManager<ApplicationUser> userManager, string username, string password, bool isAdmin = false)
        {
            var user = new ApplicationUser(username)
            {
                IsAdmin = isAdmin
            };
            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                throw new PurchaseOrderTrackerException($"Failed to create user while initializing identity database: {errors}");
            }
        }
    }
}
