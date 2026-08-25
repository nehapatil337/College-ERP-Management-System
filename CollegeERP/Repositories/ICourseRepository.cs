using CollegeERP.Models;

namespace CollegeERP.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCourses();

        Task<Course?> GetCourseById(int courseId);

        Task<int> AddCourse(Course course);

        Task<bool> UpdateCourse(int courseId, Course course);

        Task<bool> DeleteCourse(int courseId);
    }
}