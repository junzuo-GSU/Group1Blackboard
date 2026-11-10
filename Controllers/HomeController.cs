using Group1Blackboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group1Blackboard.Controllers
{
    public class HomeController : Controller
    {
        private BlackboardContext context;
        public HomeController(BlackboardContext ctx) => context = ctx;

        // student dashboard
        public IActionResult Index()
        {
            // if the session has no plan yet, restore it from the cookie so the cart badge is right
            var session = new BlackboardSession(HttpContext.Session);
            if (session.GetPlanCountOrNull() == null)
            {
                var ids = new BlackboardCookies(Request.Cookies).GetPlanIds();
                List<Course> plan = new List<Course>();
                if (ids.Count > 0)
                {
                    plan = context.Courses.Include(c => c.Major).Include(c => c.Instructor)
                        .Where(c => ids.Contains(c.CourseID)).ToList();
                }
                session.SetPlan(plan);
            }
            return View();
        }
    }
}
