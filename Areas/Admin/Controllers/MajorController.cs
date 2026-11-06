using Group1Blackboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace Group1Blackboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MajorController : Controller
    {
        private BlackboardContext context;
        public MajorController(BlackboardContext ctx) => context = ctx;

        public IActionResult Index()
        {
            var majors = context.Majors.OrderBy(m => m.Name).ToList();
            return View(majors);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("AddEdit", new Major());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var major = context.Majors.Find(id);
            if (major == null) return NotFoundRedirect();

            ViewBag.Action = "Edit";
            return View("AddEdit", major);
        }

        // one POST action for both Add and Edit: MajorID is 0 for a new major
        [HttpPost]
        public IActionResult Save(Major major)
        {
            if (context.Majors.Any(m => m.Name == major.Name && m.MajorID != major.MajorID))
                ModelState.AddModelError(nameof(major.Name), "This major already exists.");

            if (!ModelState.IsValid)
            {
                ViewBag.Action = major.MajorID == 0 ? "Add" : "Edit";
                return View("AddEdit", major);
            }

            if (major.MajorID == 0)
            {
                context.Majors.Add(major);
                TempData["message"] = $"Major {major.Name} was added.";
            }
            else
            {
                context.Majors.Update(major);
                TempData["message"] = $"Major {major.Name} was updated.";
            }
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var major = context.Majors.Find(id);
            if (major == null) return NotFoundRedirect();
            return View(major);
        }

        [HttpPost]
        public IActionResult Delete(Major major)
        {
            if (context.Courses.Any(c => c.MajorID == major.MajorID))
            {
                TempData["error"] = $"Major {major.Name} still has courses. Delete or move them first.";
                return RedirectToAction("Index");
            }

            context.Majors.Remove(major);
            context.SaveChanges();
            TempData["message"] = $"Major {major.Name} was deleted.";
            return RedirectToAction("Index");
        }

        private IActionResult NotFoundRedirect()
        {
            TempData["error"] = "That major could not be found.";
            return RedirectToAction("Index");
        }
    }
}
