using Group1Blackboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Group1Blackboard.Controllers
{
    public class CatalogController : Controller
    {
        private BlackboardContext context;
        public CatalogController(BlackboardContext ctx) => context = ctx;

        // the filters arrive through the route: Catalog/Index/major-1/loc-Hybrid/sem-all
        public IActionResult Index(CatalogViewModel model)
        {
            var session = new BlackboardSession(HttpContext.Session);

            // if the session has no plan yet, restore it from the cookie
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

            // normalize the filter values so the comparison with the database is exact
            model.ActiveMajor = model.ActiveMajor.ToLower();
            model.ActiveLoc = Course.Locations.FirstOrDefault(l =>
                l.Equals(model.ActiveLoc, StringComparison.OrdinalIgnoreCase)) ?? "all";
            model.ActiveSem = Course.Semesters.FirstOrDefault(s =>
                s != "All" && s.Equals(model.ActiveSem, StringComparison.OrdinalIgnoreCase)) ?? "all";

            // keep the filters in the session so other pages can come back to them
            session.SetActiveFilters(model.ActiveMajor, model.ActiveLoc, model.ActiveSem);

            // the drop-down lists are built here and passed to the view by ViewBag
            var majors = new List<SelectListItem> { new SelectListItem("All", "all") };
            majors.AddRange(context.Majors.OrderBy(m => m.Name)
                .Select(m => new SelectListItem(m.Name, m.MajorID.ToString())));
            ViewBag.Majors = majors;

            var locations = new List<SelectListItem> { new SelectListItem("All", "all") };
            locations.AddRange(Course.Locations.Select(l => new SelectListItem(l, l)));
            ViewBag.Locations = locations;

            var semesters = new List<SelectListItem> { new SelectListItem("All", "all") };
            semesters.AddRange(Course.Semesters.Where(s => s != "All").Select(s => new SelectListItem(s, s)));
            ViewBag.Semesters = semesters;

            IQueryable<Course> query = context.Courses.Include(c => c.Major).Include(c => c.Instructor);
            if (int.TryParse(model.ActiveMajor, out int majorId))
                query = query.Where(c => c.MajorID == majorId);
            if (model.ActiveLoc != "all")
                query = query.Where(c => c.Location == model.ActiveLoc);
            if (model.ActiveSem != "all")
                // a course offered in "All" semesters matches every semester
                query = query.Where(c => c.Semester == model.ActiveSem || c.Semester == "All");

            model.Courses = query.OrderBy(c => c.CourseNumber).ToList();
            return View(model);
        }

        // the filter form posts here; PRG: redirect to Index with the chosen filters in the URL
        [HttpPost]
        public IActionResult Filter(CatalogViewModel model)
        {
            return RedirectToAction("Index", new
            {
                activeMajor = model.ActiveMajor,
                activeLoc = model.ActiveLoc,
                activeSem = model.ActiveSem
            });
        }

        public IActionResult Details(int id)
        {
            var session = new BlackboardSession(HttpContext.Session);

            var course = context.Courses.Include(c => c.Major).Include(c => c.Instructor)
                .FirstOrDefault(c => c.CourseID == id);
            if (course == null)
            {
                TempData["error"] = "That course could not be found.";
                return RedirectToAction("Index");
            }

            var model = new CatalogViewModel
            {
                Course = course,
                ActiveMajor = session.GetActiveMajor(),
                ActiveLoc = session.GetActiveLoc(),
                ActiveSem = session.GetActiveSem(),
                InPlan = session.GetPlan().Any(c => c.CourseID == id)
            };
            return View(model);
        }
    }
}
