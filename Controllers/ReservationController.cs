using Group1Blackboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group1Blackboard.Controllers
{
    public class ReservationController : Controller
    {
        private BlackboardContext context;
        public ReservationController(BlackboardContext ctx) => context = ctx;

        // all reservations stored in the database
        public IActionResult Index()
        {
            var reservations = context.Reservations
                .Include(r => r.Course).ThenInclude(c => c.Instructor)
                .OrderBy(r => r.Course.CourseNumber)
                .ToList();
            return View(reservations);
        }

        // step 1: ask the student to confirm
        [HttpGet]
        public IActionResult Cancel(int id)
        {
            var reservation = context.Reservations
                .Include(r => r.Course).ThenInclude(c => c.Instructor)
                .FirstOrDefault(r => r.ReservationID == id);
            if (reservation == null)
            {
                TempData["error"] = "That reservation could not be found.";
                return RedirectToAction("Index");
            }
            return View(reservation);
        }

        // step 2: confirmed, remove the row from the table (PRG)
        [HttpPost]
        public IActionResult Cancel(Reservation reservation)
        {
            var found = context.Reservations.Include(r => r.Course)
                .FirstOrDefault(r => r.ReservationID == reservation.ReservationID);
            if (found == null)
            {
                TempData["error"] = "That reservation could not be found.";
                return RedirectToAction("Index");
            }

            context.Reservations.Remove(found);
            context.SaveChanges();
            TempData["message"] = $"The reservation for {found.Course.CourseNumber} {found.Course.Name} was canceled.";
            return RedirectToAction("Index");
        }
    }
}
