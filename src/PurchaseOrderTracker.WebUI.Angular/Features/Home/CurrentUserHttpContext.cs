using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PurchaseOrderTracker.Application.Identity;

namespace PurchaseOrderTracker.WebUI.Angular.Features.Home
{
    public class CurrentUserHttpContext : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserHttpContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // TODO don't throw a null reference exception if claim doesn't exist
        public string Username => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
