using System;
using Microsoft.AspNetCore.Identity;

namespace PurchaseOrderTracker.Persistence.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser(string userName)
            : base(userName)
        {
        }

        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
    }
}
