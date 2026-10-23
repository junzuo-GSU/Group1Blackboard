using Microsoft.AspNetCore.Mvc;

namespace Group1Blackboard.Controllers
{
    public class CatalogController : Controller
    {
        public IActionResult Index()
        {
            // Content() is a quick way to test the routing in this phase
            return Content("Student (main) area, Catalog controller, Index action");
        }
    }
}
