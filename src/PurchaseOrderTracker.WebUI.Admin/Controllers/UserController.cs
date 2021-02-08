using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.WebUI.Admin.Controllers
{
    // set authentication requirements TODO
    [ApiController]
    [Route("admin/[controller]")]
    public class UserController : ControllerBase
    {
        private PurchaseOrderTrackerHttpClient _client;

        public UserController(PurchaseOrderTrackerHttpClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            return await _client.GetUsers();
        }

        [HttpGet("[action]")]
        [Authorize("Administrators")] // why i need to specify and default not working...
        public async Task<string> GetTwo()
        {
            return "authorized..." + HttpContext.User.Identity.Name;
        }
    }
}
