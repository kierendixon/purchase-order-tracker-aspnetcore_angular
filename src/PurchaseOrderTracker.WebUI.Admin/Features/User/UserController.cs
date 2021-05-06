using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.WebUI.Admin.Controllers
{
    // set authentication requirements TODO
    [ApiController]
    [Route("admin/users")] // TODO shuld be just "users"
    public class UserController : ControllerBase
    {
        private PurchaseOrderTrackerHttpClient _client;

        public UserController(PurchaseOrderTrackerHttpClient client)
        {
            _client = client;
        }

        // TODO paginate
        [HttpGet]
        [Authorize("Administrator")] // move into constant
        public async Task<string> Users()
        {
            return await _client.GetUsers();
        }

        [HttpGet("{userId}")]
        public async Task<string> Get(string userId)
        {
            return userId;
            //return await _client.GetUsers();
        }

        [HttpPut]
        public async Task<string> Create(CreateUserDto user)
        {
            return await _client.CreateUser(user.Username, user.Password);
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
