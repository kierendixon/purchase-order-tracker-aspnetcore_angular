using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PurchaseOrderTracker.Application.Identity;

namespace PurchaseOrderTracker.WebApi.Identity
{
    public class CurrentUserHttpContext : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserHttpContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Username => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
