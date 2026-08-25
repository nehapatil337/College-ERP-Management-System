namespace CollegeERP.DTOs
{
    public class CourseDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string Duration { get; set; } = string.Empty;
    }

    public class CreateCourseDto
    {
        public string CourseName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string Duration { get; set; } = string.Empty;
    }

    public class UpdateCourseDto
    {
        public string CourseName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string Duration { get; set; } = string.Empty;
    }
}