using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.Web.Features.Home
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/index.html");
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
