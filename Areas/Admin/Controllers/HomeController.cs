using Microsoft.AspNetCore.Mvc;

namespace Group1Blackboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        // admin dashboard
        public IActionResult Index()
        {
            return View();
        }
    }
}
