using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderTracker.Identity.Features.Account.Models
{
    public class LoginCommandDto
    {
        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }

        [Required]
        [MinLength(3)]
        public string Password { get; set; }
    }
}
