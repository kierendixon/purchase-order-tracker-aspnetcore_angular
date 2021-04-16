using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PurchaseOrderTracker.Application.Identity;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Persistence.Identity;

namespace PurchaseOrderTracker.Web.Features.Home
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICurrentUser _currentUser;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ICurrentUser currentUser)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _currentUser = currentUser;
        }

        [AllowAnonymous]
        public async Task Login()
        {
            await _signInManager.SignInAsync(new ApplicationUser("basic"),false);
            //_signInManager.PasswordSignInAsync
        }

        [HttpGet]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
