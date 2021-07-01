using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.WebUI.Angular.Features.User
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public ActionResult<User> GetUser()
        {
            var username = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var role = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            return new User(username, role == "admin" ? true : null);
        }
    }

    public class User
    {
        public User(string username, bool? isAdmin)
        {
            Username = username;
            IsAdmin = isAdmin;
        }

        public string Username { get; }
        public bool? IsAdmin { get; }
    }
}
