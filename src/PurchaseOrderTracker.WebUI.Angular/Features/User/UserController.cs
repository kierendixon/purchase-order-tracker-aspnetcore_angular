using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.WebUI.Angular.Features.User
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1125:Boolean literals should not be redundant", Justification = "false-positive")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721:Property names should not match get methods", Justification = "violation not relevant here")]
        public ActionResult<UserDTO> GetUser()
        {
            var username = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var role = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            return new UserDTO(username, role == "admin" ? true : null);
        }
    }

    // TODO move class
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<Pending>")]
    public class UserDTO
    {
        public UserDTO(string username, bool? isAdmin)
        {
            Username = username;
            IsAdmin = isAdmin;
        }

        public string Username { get; }
        public bool? IsAdmin { get; }
    }
}
