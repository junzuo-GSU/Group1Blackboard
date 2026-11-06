using Microsoft.EntityFrameworkCore;

namespace Group1Blackboard.Models
{
    public class BlackboardContext : DbContext
    {
        public BlackboardContext(DbContextOptions<BlackboardContext> options) : base(options) { }

        public DbSet<Major> Majors { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<AppUser> Users { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // a major with courses, or a course with reservations, cannot be deleted
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Major).WithMany().HasForeignKey(c => c.MajorID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor).WithMany().HasForeignKey(c => c.InstructorID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Course).WithMany().HasForeignKey(r => r.CourseID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Major>().HasIndex(m => m.Name).IsUnique();
            modelBuilder.Entity<Course>().HasIndex(c => c.CourseNumber).IsUnique();

            // seed data
            modelBuilder.Entity<Major>().HasData(
                new Major { MajorID = 1, Name = "Computer Science", College = "College of Arts and Science" },
                new Major { MajorID = 2, Name = "Information Systems", College = "College of Business" },
                new Major { MajorID = 3, Name = "Mathematics", College = "College of Arts and Science" },
                new Major { MajorID = 4, Name = "Business Administration", College = "College of Business" },
                new Major { MajorID = 5, Name = "Elementary Education", College = "College of Education" },
                new Major { MajorID = 6, Name = "Health Administration", College = "College of Health and Human Services" }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { CourseID = 1, CourseNumber = "CSPC8720", Name = "Internet Programming", Credit = 3, Semester = "Spring", Location = "Hybrid", Times = "M/W 1:30 PM - 2:45 PM", InstructorID = 1, Description = "Server-side web development with ASP.NET Core MVC.", MajorID = 1 },
                new Course { CourseID = 2, CourseNumber = "CSPC6100", Name = "Data Structures", Credit = 3, Semester = "All", Location = "In-person", Times = "T/R 9:00 AM - 10:15 AM", InstructorID = 5, Description = "Lists, stacks, queues, trees, and hash tables.", MajorID = 1 },
                new Course { CourseID = 3, CourseNumber = "CSPC7300", Name = "Operating Systems", Credit = 3, Semester = "Fall", Location = "In-person", Times = "M/W 10:30 AM - 11:45 AM", InstructorID = 6, Description = "Processes, memory management, and file systems.", MajorID = 1 },
                new Course { CourseID = 4, CourseNumber = "CSPC7550", Name = "Machine Learning Basics", Credit = 3, Semester = "Summer", Location = "Online", Times = "Online, self-paced", InstructorID = 7, Description = "Introduction to supervised and unsupervised learning.", MajorID = 1 },
                new Course { CourseID = 5, CourseNumber = "INFS6200", Name = "Database Systems", Credit = 3, Semester = "All", Location = "Hybrid", Times = "T 6:00 PM - 8:45 PM", InstructorID = 8, Description = "Relational design, SQL, and data modeling.", MajorID = 2 },
                new Course { CourseID = 6, CourseNumber = "INFS6400", Name = "Systems Analysis and Design", Credit = 3, Semester = "Fall", Location = "Online", Times = "Online, self-paced", InstructorID = 9, Description = "Requirements gathering and information system design.", MajorID = 2 },
                new Course { CourseID = 7, CourseNumber = "INFS7100", Name = "Business Analytics", Credit = 3, Semester = "Spring", Location = "In-person", Times = "R 6:00 PM - 8:45 PM", InstructorID = 10, Description = "Using data to support business decisions.", MajorID = 2 },
                new Course { CourseID = 8, CourseNumber = "MATH5100", Name = "Linear Algebra", Credit = 4, Semester = "Fall", Location = "In-person", Times = "M/W/F 9:00 AM - 9:50 AM", InstructorID = 11, Description = "Vector spaces, matrices, and eigenvalues.", MajorID = 3 },
                new Course { CourseID = 9, CourseNumber = "MATH5200", Name = "Probability and Statistics", Credit = 3, Semester = "All", Location = "Online", Times = "Online, self-paced", InstructorID = 12, Description = "Probability models and statistical inference.", MajorID = 3 },
                new Course { CourseID = 10, CourseNumber = "BADM5000", Name = "Principles of Management", Credit = 3, Semester = "Spring", Location = "Hybrid", Times = "T/R 3:00 PM - 4:15 PM", InstructorID = 13, Description = "Planning, organizing, leading, and controlling.", MajorID = 4 },
                new Course { CourseID = 11, CourseNumber = "BADM5400", Name = "Marketing Fundamentals", Credit = 3, Semester = "Summer", Location = "In-person", Times = "M/W 5:00 PM - 7:30 PM", InstructorID = 14, Description = "Markets, consumers, and the marketing mix.", MajorID = 4 },
                new Course { CourseID = 12, CourseNumber = "BADM6000", Name = "Managerial Accounting", Credit = 3, Semester = "Fall", Location = "Online", Times = "Online, self-paced", InstructorID = 15, Description = "Cost behavior and budgeting for managers.", MajorID = 4 },
                new Course { CourseID = 13, CourseNumber = "EDUC5100", Name = "Foundations of Education", Credit = 3, Semester = "Fall", Location = "In-person", Times = "T/R 4:30 PM - 5:45 PM", InstructorID = 9, Description = "Historical and philosophical foundations of teaching.", MajorID = 5 },
                new Course { CourseID = 14, CourseNumber = "HLTH5200", Name = "Health Care Systems", Credit = 3, Semester = "Spring", Location = "Online", Times = "Online, self-paced", InstructorID = 15, Description = "Structure and financing of the U.S. health care system.", MajorID = 6 }
            );

            // sample users (the instructors teach the seeded courses) with obviously fake SSNs (area numbers starting with 9 are never issued)
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser { UserID = 1, Name = "Alan Park", DateOfBirth = new DateTime(1975, 3, 14), Email = "apark@example.edu", SSN = "900-11-1111", UserType = "Instructor" },
                new AppUser { UserID = 2, Name = "Emily Carter", DateOfBirth = new DateTime(2002, 8, 21), Email = "ecarter@example.edu", SSN = "900-22-2222", UserType = "Student" },
                new AppUser { UserID = 3, Name = "Robert Nguyen", DateOfBirth = new DateTime(1988, 11, 2), Email = "rnguyen@example.edu", SSN = "900-33-3333", UserType = "Staff" },
                new AppUser { UserID = 4, Name = "Site Admin", DateOfBirth = new DateTime(1980, 1, 1), Email = "admin@example.edu", SSN = "900-44-4444", UserType = "Admin" },
                new AppUser { UserID = 5, Name = "Maria Lopez", DateOfBirth = new DateTime(1979, 6, 2), Email = "mlopez@example.edu", SSN = "900-05-5555", UserType = "Instructor" },
                new AppUser { UserID = 6, Name = "Wei Chen", DateOfBirth = new DateTime(1972, 9, 17), Email = "wchen@example.edu", SSN = "900-06-6666", UserType = "Instructor" },
                new AppUser { UserID = 7, Name = "Priya Nair", DateOfBirth = new DateTime(1983, 2, 25), Email = "pnair@example.edu", SSN = "900-07-7777", UserType = "Instructor" },
                new AppUser { UserID = 8, Name = "James Walker", DateOfBirth = new DateTime(1968, 12, 8), Email = "jwalker@example.edu", SSN = "900-08-8888", UserType = "Instructor" },
                new AppUser { UserID = 9, Name = "Susan Miller", DateOfBirth = new DateTime(1974, 4, 30), Email = "smiller@example.edu", SSN = "900-09-9999", UserType = "Instructor" },
                new AppUser { UserID = 10, Name = "Omar Hassan", DateOfBirth = new DateTime(1981, 7, 19), Email = "ohassan@example.edu", SSN = "900-10-1110", UserType = "Instructor" },
                new AppUser { UserID = 11, Name = "Helen Brooks", DateOfBirth = new DateTime(1970, 10, 11), Email = "hbrooks@example.edu", SSN = "900-11-2221", UserType = "Instructor" },
                new AppUser { UserID = 12, Name = "Kevin Zhou", DateOfBirth = new DateTime(1985, 1, 6), Email = "kzhou@example.edu", SSN = "900-12-3332", UserType = "Instructor" },
                new AppUser { UserID = 13, Name = "Linda Grant", DateOfBirth = new DateTime(1966, 5, 23), Email = "lgrant@example.edu", SSN = "900-13-4443", UserType = "Instructor" },
                new AppUser { UserID = 14, Name = "Daniel Reed", DateOfBirth = new DateTime(1977, 8, 14), Email = "dreed@example.edu", SSN = "900-14-5554", UserType = "Instructor" },
                new AppUser { UserID = 15, Name = "Nina Patel", DateOfBirth = new DateTime(1984, 3, 9), Email = "npatel@example.edu", SSN = "900-15-6665", UserType = "Instructor" }
            );
        }
    }
}
