namespace CollegeERP.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; } = "";

        public int DepartmentId { get; set; }

        public string Duration { get; set; } = "";
    }
}