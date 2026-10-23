using Microsoft.AspNetCore.Mvc;

namespace Group1Blackboard.Controllers
{
    public class HomeController : Controller
    {
        // student dashboard
        public IActionResult Index()
        {
            return View();
        }
    }
}
