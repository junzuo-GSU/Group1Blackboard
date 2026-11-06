using Group1Blackboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group1Blackboard.Controllers
{
    public class PlanController : Controller
    {
        private BlackboardContext context;
        public PlanController(BlackboardContext ctx) => context = ctx;

        public IActionResult Index()
        {
            RestorePlan();
            var session = new PlanSession(HttpContext.Session);
            var model = new CatalogViewModel
            {
                Courses = session.GetPlan(),
                ActiveMajor = session.GetActiveMajor(),
                ActiveLoc = session.GetActiveLoc(),
                ActiveSem = session.GetActiveSem()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(int id)
        {
            RestorePlan();
            var session = new PlanSession(HttpContext.Session);
            var plan = session.GetPlan();

            var course = context.Courses.Include(c => c.Major).Include(c => c.Instructor)
                .FirstOrDefault(c => c.CourseID == id);

            if (course == null)
                TempData["error"] = "That course could not be found.";
            else if (plan.Any(c => c.CourseID == id))
                TempData["message"] = $"{course.CourseNumber} is already in your plan.";
            else if (context.Reservations.Any(r => r.CourseID == id))
                TempData["message"] = $"{course.CourseNumber} is already reserved.";
            else
            {
                plan.Add(course);
                SavePlan(plan);
                TempData["message"] = $"{course.CourseNumber} {course.Name} was added to your plan.";
            }

            // PRG: back to the catalog with the retained filters
            return RedirectToAction("Index", "Catalog", new
            {
                activeMajor = session.GetActiveMajor(),
                activeLoc = session.GetActiveLoc(),
                activeSem = session.GetActiveSem()
            });
        }

        // cancel a course from the plan
        [HttpPost]
        public IActionResult Remove(int id)
        {
            RestorePlan();
            var session = new PlanSession(HttpContext.Session);
            var plan = session.GetPlan();

            var course = plan.FirstOrDefault(c => c.CourseID == id);
            if (course != null)
            {
                plan.Remove(course);
                SavePlan(plan);
                TempData["message"] = $"{course.CourseNumber} was removed from your plan.";
            }
            return RedirectToAction("Index");
        }

        // move a course from the session to the database
        [HttpPost]
        public IActionResult Reserve(int id)
        {
            RestorePlan();
            var session = new PlanSession(HttpContext.Session);
            var plan = session.GetPlan();

            var course = plan.FirstOrDefault(c => c.CourseID == id);
            if (course == null)
                TempData["error"] = "That course is not in your plan.";
            else
            {
                if (!context.Reservations.Any(r => r.CourseID == id))
                {
                    context.Reservations.Add(new Reservation { CourseID = id });
                    context.SaveChanges();
                }
                plan.Remove(course);
                SavePlan(plan);
                TempData["message"] = $"{course.CourseNumber} {course.Name} is reserved.";
            }
            return RedirectToAction("Index");
        }

        // If the session has no plan yet (new browser session), seed it from the cookie
        private void RestorePlan()
        {
            var session = new PlanSession(HttpContext.Session);
            if (session.GetPlanCountOrNull() != null) return;

            var ids = new PlanCookies(Request.Cookies).GetPlanIds();
            List<Course> plan = new List<Course>();
            if (ids.Count > 0)
            {
                plan = context.Courses.Include(c => c.Major).Include(c => c.Instructor)
                    .Where(c => ids.Contains(c.CourseID)).ToList();
            }
            session.SetPlan(plan);
        }

        // keep the session and the cookie consistent
        private void SavePlan(List<Course> plan)
        {
            new PlanSession(HttpContext.Session).SetPlan(plan);
            new PlanCookies(Response.Cookies).SetPlanIds(plan);
        }
    }
}
