using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Group1Blackboard.Models
{
    // a reserved course: moved here from the session when the student clicks Reserve
    public class Reservation
    {
        public int ReservationID { get; set; }

        public int CourseID { get; set; }

        [ValidateNever]
        public Course Course { get; set; } = null!;
    }
}
