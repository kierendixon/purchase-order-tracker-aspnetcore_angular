using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderTracker.Identity.Features.Account.Models
{
    public class LoginCommandDto
    {
        // TODO add length requirements?
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
