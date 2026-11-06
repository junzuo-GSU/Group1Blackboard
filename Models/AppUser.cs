using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Group1Blackboard.Models
{
    // named AppUser (not User) because every controller already has a property named User
    public class AppUser
    {
        public static readonly List<string> UserTypes = new() { "Instructor", "Student", "Staff", "Admin" };

        // [Key] is needed because the class name (AppUser) does not match the property name (UserID)
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please enter a name.")]
        [StringLength(60)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a date of birth.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        [JsonIgnore] // personal data is kept out of the session copy of a course
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(80)]
        [JsonIgnore]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter an SSN.")]
        [RegularExpression(@"^\d{3}-\d{2}-\d{4}$", ErrorMessage = "Use the format 000-00-0000.")]
        [JsonIgnore]
        public string SSN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a user type.")]
        [Display(Name = "User Type")]
        public string UserType { get; set; } = string.Empty;
    }
}
