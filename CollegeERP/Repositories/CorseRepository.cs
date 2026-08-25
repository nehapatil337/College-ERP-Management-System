using Dapper;
using Microsoft.Data.SqlClient;
using CollegeERP.Models;

namespace CollegeERP.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly IConfiguration _configuration;

        public CourseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            using var connection = GetConnection();

            string sql = @"
                SELECT
                    CourseId,
                    CourseName,
                    DepartmentId,
                    Duration
                FROM Courses
                ORDER BY CourseId";

            return await connection.QueryAsync<Course>(sql);
        }

        public async Task<Course?> GetCourseById(int courseId)
        {
            using var connection = GetConnection();

            string sql = @"
                SELECT
                    CourseId,
                    CourseName,
                    DepartmentId,
                    Duration
                FROM Courses
                WHERE CourseId = @CourseId";

            return await connection.QueryFirstOrDefaultAsync<Course>(
                sql,
                new { CourseId = courseId }
            );
        }

        public async Task<int> AddCourse(Course course)
        {
            using var connection = GetConnection();

            string sql = @"
                INSERT INTO Courses
                (
                    CourseName,
                    DepartmentId,
                    Duration
                )
                VALUES
                (
                    @CourseName,
                    @DepartmentId,
                    @Duration
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await connection.ExecuteScalarAsync<int>(
                sql,
                course
            );
        }

        public async Task<bool> UpdateCourse(
            int courseId,
            Course course)
        {
            using var connection = GetConnection();

            string sql = @"
                UPDATE Courses
                SET
                    CourseName = @CourseName,
                    DepartmentId = @DepartmentId,
                    Duration = @Duration
                WHERE CourseId = @CourseId";

            int rows = await connection.ExecuteAsync(
                sql,
                new
                {
                    CourseId = courseId,
                    course.CourseName,
                    course.DepartmentId,
                    course.Duration
                }
            );

            return rows > 0;
        }

        public async Task<bool> DeleteCourse(int courseId)
        {
            using var connection = GetConnection();

            string sql = @"
                DELETE FROM Courses
                WHERE CourseId = @CourseId";

            int rows = await connection.ExecuteAsync(
                sql,
                new { CourseId = courseId }
            );

            return rows > 0;
        }
    }
}