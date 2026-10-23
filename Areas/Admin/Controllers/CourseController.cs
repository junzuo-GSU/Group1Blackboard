using Microsoft.AspNetCore.Mvc;

namespace Group1Blackboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            // Content() is a quick way to test the routing in this phase
            return Content("Admin area, Course controller, Index action");
        }
    }
}
