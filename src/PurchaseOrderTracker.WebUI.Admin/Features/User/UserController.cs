using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.WebUI.Admin.Controllers
{
    // TODO change return types to IActionResult
    // set authentication requirements TODO
    [ApiController]
    [Route("admin/users")] // TODO shuld be just "users"
    public class UserController : ControllerBase
    {
        private PurchaseOrderTrackerHttpClient _httpClient;

        public UserController(PurchaseOrderTrackerHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // TODO paginate
        [HttpGet]
        [Authorize("Administrator")] // move into constant
        public async Task<ActionResult<string>> Users()
        {
            return await _httpClient.GetUsers();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<string>> Get(string userId)
        {
            return userId;
            //return await _client.GetUsers();
        }

        [HttpPut]
        public async Task<ActionResult<string>> Create(CreateUserDto user)
        {
            return await _httpClient.CreateUser(user.Username, user.Password);
        }

        // todo ui validation
        public class CreateUserDto{
            [Required]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }
        }
    }
}
