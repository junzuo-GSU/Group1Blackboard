using Group1Blackboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Group1Blackboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private BlackboardContext context;
        public UserController(BlackboardContext ctx) => context = ctx;

        public IActionResult Index()
        {
            var users = context.Users.OrderBy(u => u.Name).ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult Add()
        {
            LoadLists();
            ViewBag.Action = "Add";
            return View("AddEdit", new AppUser { UserType = "Student" });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = context.Users.Find(id);
            if (user == null) return NotFoundRedirect();

            LoadLists();
            ViewBag.Action = "Edit";
            return View("AddEdit", user);
        }

        [HttpPost]
        public IActionResult Save(AppUser user)
        {
            if (user.DateOfBirth > DateTime.Today)
                ModelState.AddModelError(nameof(user.DateOfBirth), "Date of birth cannot be in the future.");
            if (context.Users.Any(u => u.Email == user.Email && u.UserID != user.UserID))
                ModelState.AddModelError(nameof(user.Email), "This email is already used by another user.");

            // an instructor who teaches a course must stay an instructor
            if (user.UserType != "Instructor" && context.Courses.Any(c => c.InstructorID == user.UserID))
                ModelState.AddModelError(nameof(user.UserType), "This user teaches courses, so the type must stay Instructor.");

            if (!ModelState.IsValid)
            {
                LoadLists();
                ViewBag.Action = user.UserID == 0 ? "Add" : "Edit";
                return View("AddEdit", user);
            }

            if (user.UserID == 0)
            {
                context.Users.Add(user);
                TempData["message"] = $"User {user.Name} was added.";
            }
            else
            {
                context.Users.Update(user);
                TempData["message"] = $"User {user.Name} was updated.";
            }
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = context.Users.Find(id);
            if (user == null) return NotFoundRedirect();
            return View(user);
        }

        [HttpPost]
        public IActionResult Delete(AppUser user)
        {
            if (context.Courses.Any(c => c.InstructorID == user.UserID))
            {
                TempData["error"] = $"User {user.Name} teaches courses. Assign another instructor first.";
                return RedirectToAction("Index");
            }

            context.Users.Remove(user);
            context.SaveChanges();
            TempData["message"] = $"User {user.Name} was deleted.";
            return RedirectToAction("Index");
        }

        private void LoadLists() =>
            ViewBag.UserTypes = new SelectList(AppUser.UserTypes);

        private IActionResult NotFoundRedirect()
        {
            TempData["error"] = "That user could not be found.";
            return RedirectToAction("Index");
        }
    }
}
