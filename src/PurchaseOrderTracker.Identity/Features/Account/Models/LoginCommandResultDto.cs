namespace PurchaseOrderTracker.Identity.Features.Account.Models;

public class LoginCommandResultDto
{
    public LoginCommandResultDto(bool isAdmin)
    {
        IsAdmin = isAdmin;
    }

    public bool IsAdmin { get; }
}
