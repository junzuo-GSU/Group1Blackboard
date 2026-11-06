using System.ComponentModel.DataAnnotations;

namespace Group1Blackboard.Models
{
    public class Major
    {
        public int MajorID { get; set; }

        [Required(ErrorMessage = "Please enter a major name.")]
        [StringLength(60, ErrorMessage = "The name must be 60 characters or fewer.")]
        public string Name { get; set; } = string.Empty;

        // a plain text field, such as "College of Business"
        [Required(ErrorMessage = "Please enter the college.")]
        [StringLength(80, ErrorMessage = "The college must be 80 characters or fewer.")]
        public string College { get; set; } = string.Empty;
    }
}
