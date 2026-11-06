using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Group1Blackboard.Models
{
    public class Course
    {
        // predefined lists, shared by the admin forms and the student filters
        public static readonly List<string> Semesters = new() { "All", "Spring", "Fall", "Summer" };
        public static readonly List<string> Locations = new() { "In-person", "Hybrid", "Online" };

        public int CourseID { get; set; }

        [Required(ErrorMessage = "Please enter a course number.")]
        [RegularExpression(@"^[A-Za-z]{3,4}\d{4}$", ErrorMessage = "Use a format like CSPC8720.")]
        public string CourseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a course name.")]
        [StringLength(80, ErrorMessage = "The name must be 80 characters or fewer.")]
        public string Name { get; set; } = string.Empty;

        [Range(1, 6, ErrorMessage = "Credit must be between 1 and 6.")]
        public int Credit { get; set; }

        // "All" means the course is offered in every semester (Spring, Fall, and Summer)
        [Required(ErrorMessage = "Please select the offered semester.")]
        public string Semester { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a location.")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the meeting times.")]
        [StringLength(60)]
        public string Times { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a short description.")]
        [StringLength(250, ErrorMessage = "The description must be 250 characters or fewer.")]
        public string Description { get; set; } = string.Empty;

        // foreign key and navigation property
        [Range(1, int.MaxValue, ErrorMessage = "Please select a major.")]
        public int MajorID { get; set; }

        [ValidateNever]
        public Major Major { get; set; } = null!;

        // the instructor is a user whose UserType is "Instructor"
        [Range(1, int.MaxValue, ErrorMessage = "Please select an instructor.")]
        public int InstructorID { get; set; }

        [ValidateNever]
        public AppUser Instructor { get; set; } = null!;
    }
}
