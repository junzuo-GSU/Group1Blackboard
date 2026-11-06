using Group1Blackboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Group1Blackboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private BlackboardContext context;
        public CourseController(BlackboardContext ctx) => context = ctx;

        public IActionResult Index()
        {
            var courses = context.Courses.Include(c => c.Major).Include(c => c.Instructor)
                .OrderBy(c => c.CourseNumber).ToList();
            return View(courses);
        }

        [HttpGet]
        public IActionResult Add()
        {
            LoadLists();
            ViewBag.Action = "Add";
            // sensible defaults for a new course
            return View("AddEdit", new Course { Credit = 3, Semester = "All", Location = "In-person" });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = context.Courses.Find(id);
            if (course == null) return NotFoundRedirect();

            LoadLists();
            ViewBag.Action = "Edit";
            return View("AddEdit", course);
        }

        [HttpPost]
        public IActionResult Save(Course course)
        {
            if (context.Courses.Any(c => c.CourseNumber == course.CourseNumber && c.CourseID != course.CourseID))
                ModelState.AddModelError(nameof(course.CourseNumber), "This course number already exists.");

            if (course.InstructorID > 0 &&
                !context.Users.Any(u => u.UserID == course.InstructorID && u.UserType == "Instructor"))
                ModelState.AddModelError(nameof(course.InstructorID), "Please select a user of type Instructor.");

            if (!ModelState.IsValid)
            {
                LoadLists();
                ViewBag.Action = course.CourseID == 0 ? "Add" : "Edit";
                return View("AddEdit", course);
            }

            course.CourseNumber = course.CourseNumber.ToUpper();
            if (course.CourseID == 0)
            {
                context.Courses.Add(course);
                TempData["message"] = $"Course {course.CourseNumber} was added.";
            }
            else
            {
                context.Courses.Update(course);
                TempData["message"] = $"Course {course.CourseNumber} was updated.";
            }
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var course = context.Courses.Include(c => c.Major).Include(c => c.Instructor)
                .FirstOrDefault(c => c.CourseID == id);
            if (course == null) return NotFoundRedirect();
            return View(course);
        }

        [HttpPost]
        public IActionResult Delete(Course course)
        {
            if (context.Reservations.Any(r => r.CourseID == course.CourseID))
            {
                TempData["error"] = $"Course {course.CourseNumber} has reservations and cannot be deleted.";
                return RedirectToAction("Index");
            }

            context.Courses.Remove(course);
            context.SaveChanges();
            TempData["message"] = $"Course {course.CourseNumber} was deleted.";
            return RedirectToAction("Index");
        }

        // build the drop-down lists in the controller and pass them to the view
        private void LoadLists()
        {
            ViewBag.Majors = new SelectList(context.Majors.OrderBy(m => m.Name), "MajorID", "Name");
            ViewBag.Semesters = new SelectList(Course.Semesters);
            ViewBag.Locations = new SelectList(Course.Locations);
            // only users of type Instructor can teach a course
            ViewBag.Instructors = new SelectList(
                context.Users.Where(u => u.UserType == "Instructor").OrderBy(u => u.Name), "UserID", "Name");
        }

        private IActionResult NotFoundRedirect()
        {
            TempData["error"] = "That course could not be found.";
            return RedirectToAction("Index");
        }
    }
}
