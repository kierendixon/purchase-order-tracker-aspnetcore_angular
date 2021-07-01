//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace PurchaseOrderTracker.WebUI.Admin.Controllers
//{
//    // TODO change return types to IActionResult
//    // set authentication requirements TODO
//    [ApiController]
//        [Authorize]
//    [Route("admin/user")] // TODO shuld be just "users"
//    public class UserController : ControllerBase
//    {
//        private PurchaseOrderTrackerHttpClient _httpClient;

//        public UserController(PurchaseOrderTrackerHttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        // don't really need this. if any api call from the frontend returns 401 unauthorized then redirect user to /account page
//        // this handles non-admin user as well as session timeout
//        [Route("currentUser")]
//        public ActionResult GetUser()
//        {
//            var role = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
//            return role == "admin" ? Ok() : Unauthorized();
//        }

//        // TODO paginate
//        [HttpGet]
//        [Authorize("Administrator")] // move into constant
//        public async Task<ActionResult<string>> Users()
//        {
//            return await _httpClient.GetUsers();
//        }

//        [HttpGet("{userId}")]
//        public async Task<ActionResult<string>> Get(string userId)
//        {
//            return userId;
//            //return await _client.GetUsers();
//        }

//        [HttpPut]
//        public async Task<ActionResult<string>> Create(CreateUserDto user)
//        {
//            return await _httpClient.CreateUser(user.Username, user.Password);
//        }

//        // todo ui validation
//        public class CreateUserDto{
//            [Required]
//            public string Username { get; set; }

//            [Required]
//            public string Password { get; set; }
//        }
//    }
//}
