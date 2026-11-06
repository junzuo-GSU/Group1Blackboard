using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Group1Blackboard.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Majors",
                columns: table => new
                {
                    MajorID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    College = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.MajorID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    SSN = table.Column<string>(type: "TEXT", nullable: false),
                    UserType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Credit = table.Column<int>(type: "INTEGER", nullable: false),
                    Semester = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Times = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    MajorID = table.Column<int>(type: "INTEGER", nullable: false),
                    InstructorID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseID);
                    table.ForeignKey(
                        name: "FK_Courses_Majors_MajorID",
                        column: x => x.MajorID,
                        principalTable: "Majors",
                        principalColumn: "MajorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_Users_InstructorID",
                        column: x => x.InstructorID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationID);
                    table.ForeignKey(
                        name: "FK_Reservations_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Majors",
                columns: new[] { "MajorID", "College", "Name" },
                values: new object[,]
                {
                    { 1, "College of Arts and Science", "Computer Science" },
                    { 2, "College of Business", "Information Systems" },
                    { 3, "College of Arts and Science", "Mathematics" },
                    { 4, "College of Business", "Business Administration" },
                    { 5, "College of Education", "Elementary Education" },
                    { 6, "College of Health and Human Services", "Health Administration" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "DateOfBirth", "Email", "Name", "SSN", "UserType" },
                values: new object[,]
                {
                    { 1, new DateTime(1975, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "apark@example.edu", "Alan Park", "900-11-1111", "Instructor" },
                    { 2, new DateTime(2002, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "ecarter@example.edu", "Emily Carter", "900-22-2222", "Student" },
                    { 3, new DateTime(1988, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "rnguyen@example.edu", "Robert Nguyen", "900-33-3333", "Staff" },
                    { 4, new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.edu", "Site Admin", "900-44-4444", "Admin" },
                    { 5, new DateTime(1979, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "mlopez@example.edu", "Maria Lopez", "900-05-5555", "Instructor" },
                    { 6, new DateTime(1972, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "wchen@example.edu", "Wei Chen", "900-06-6666", "Instructor" },
                    { 7, new DateTime(1983, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "pnair@example.edu", "Priya Nair", "900-07-7777", "Instructor" },
                    { 8, new DateTime(1968, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "jwalker@example.edu", "James Walker", "900-08-8888", "Instructor" },
                    { 9, new DateTime(1974, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "smiller@example.edu", "Susan Miller", "900-09-9999", "Instructor" },
                    { 10, new DateTime(1981, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "ohassan@example.edu", "Omar Hassan", "900-10-1110", "Instructor" },
                    { 11, new DateTime(1970, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "hbrooks@example.edu", "Helen Brooks", "900-11-2221", "Instructor" },
                    { 12, new DateTime(1985, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "kzhou@example.edu", "Kevin Zhou", "900-12-3332", "Instructor" },
                    { 13, new DateTime(1966, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "lgrant@example.edu", "Linda Grant", "900-13-4443", "Instructor" },
                    { 14, new DateTime(1977, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "dreed@example.edu", "Daniel Reed", "900-14-5554", "Instructor" },
                    { 15, new DateTime(1984, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "npatel@example.edu", "Nina Patel", "900-15-6665", "Instructor" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseID", "CourseNumber", "Credit", "Description", "InstructorID", "Location", "MajorID", "Name", "Semester", "Times" },
                values: new object[,]
                {
                    { 1, "CSPC8720", 3, "Server-side web development with ASP.NET Core MVC.", 1, "Hybrid", 1, "Internet Programming", "Spring", "M/W 1:30 PM - 2:45 PM" },
                    { 2, "CSPC6100", 3, "Lists, stacks, queues, trees, and hash tables.", 5, "In-person", 1, "Data Structures", "All", "T/R 9:00 AM - 10:15 AM" },
                    { 3, "CSPC7300", 3, "Processes, memory management, and file systems.", 6, "In-person", 1, "Operating Systems", "Fall", "M/W 10:30 AM - 11:45 AM" },
                    { 4, "CSPC7550", 3, "Introduction to supervised and unsupervised learning.", 7, "Online", 1, "Machine Learning Basics", "Summer", "Online, self-paced" },
                    { 5, "INFS6200", 3, "Relational design, SQL, and data modeling.", 8, "Hybrid", 2, "Database Systems", "All", "T 6:00 PM - 8:45 PM" },
                    { 6, "INFS6400", 3, "Requirements gathering and information system design.", 9, "Online", 2, "Systems Analysis and Design", "Fall", "Online, self-paced" },
                    { 7, "INFS7100", 3, "Using data to support business decisions.", 10, "In-person", 2, "Business Analytics", "Spring", "R 6:00 PM - 8:45 PM" },
                    { 8, "MATH5100", 4, "Vector spaces, matrices, and eigenvalues.", 11, "In-person", 3, "Linear Algebra", "Fall", "M/W/F 9:00 AM - 9:50 AM" },
                    { 9, "MATH5200", 3, "Probability models and statistical inference.", 12, "Online", 3, "Probability and Statistics", "All", "Online, self-paced" },
                    { 10, "BADM5000", 3, "Planning, organizing, leading, and controlling.", 13, "Hybrid", 4, "Principles of Management", "Spring", "T/R 3:00 PM - 4:15 PM" },
                    { 11, "BADM5400", 3, "Markets, consumers, and the marketing mix.", 14, "In-person", 4, "Marketing Fundamentals", "Summer", "M/W 5:00 PM - 7:30 PM" },
                    { 12, "BADM6000", 3, "Cost behavior and budgeting for managers.", 15, "Online", 4, "Managerial Accounting", "Fall", "Online, self-paced" },
                    { 13, "EDUC5100", 3, "Historical and philosophical foundations of teaching.", 9, "In-person", 5, "Foundations of Education", "Fall", "T/R 4:30 PM - 5:45 PM" },
                    { 14, "HLTH5200", 3, "Structure and financing of the U.S. health care system.", 15, "Online", 6, "Health Care Systems", "Spring", "Online, self-paced" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseNumber",
                table: "Courses",
                column: "CourseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_InstructorID",
                table: "Courses",
                column: "InstructorID");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_MajorID",
                table: "Courses",
                column: "MajorID");

            migrationBuilder.CreateIndex(
                name: "IX_Majors_Name",
                table: "Majors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CourseID",
                table: "Reservations",
                column: "CourseID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Majors");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
