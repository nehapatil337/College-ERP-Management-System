using CollegeERP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CollegeERP.Repositories
{
    public class CollegeRepository : ICollegeRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<CollegeRepository> _logger;

        public CollegeRepository(
            IConfiguration configuration,
            ILogger<CollegeRepository> logger)
        {
            _logger = logger;

            _connectionString =
                "Server=127.0.0.1,62100;Database=college;Integrated Security=true;Encrypt=false;";

            _logger.LogInformation("Database connection configured successfully.");
        }

        // GET ALL COLLEGES
        public async Task<IEnumerable<College>> GetAllCollegesAsync()
        {
            const string query = @"
                SELECT 
                    ROLLNO AS RollNo,
                    NAME AS Name,
                    AGE AS Age,
                    CITY AS City,
                    DEPARTMENT AS Department
                FROM [dbo].[college]";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var colleges =
                        await connection.QueryAsync<College>(query);

                    return colleges;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while retrieving all colleges.");

                throw;
            }
        }

        // GET COLLEGE BY ROLL NO
        public async Task<College?> GetCollegeByRollNoAsync(int rollNo)
        {
            const string query = @"
                SELECT 
                    ROLLNO AS RollNo,
                    NAME AS Name,
                    AGE AS Age,
                    CITY AS City,
                    DEPARTMENT AS Department
                FROM [dbo].[college]
                WHERE ROLLNO = @RollNo";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var college =
                        await connection.QueryFirstOrDefaultAsync<College>(
                            query,
                            new { RollNo = rollNo });

                    return college;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while retrieving college with RollNo {RollNo}.",
                    rollNo);

                throw;
            }
        }

        // ADD COLLEGE
        public async Task<int> AddCollegeAsync(College college)
        {
            const string getRollNoQuery = @"
                SELECT ISNULL(MAX(ROLLNO), 0) + 1
                FROM [dbo].[college]";

            const string insertQuery = @"
                INSERT INTO [dbo].[college]
                (ROLLNO, NAME, AGE, CITY, DEPARTMENT)
                VALUES
                (@RollNo, @Name, @Age, @City, @Department)";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    int rollNo =
                        await connection.ExecuteScalarAsync<int>(
                            getRollNoQuery);

                    await connection.ExecuteAsync(
                        insertQuery,
                        new
                        {
                            RollNo = rollNo,
                            Name = college.Name,
                            Age = college.Age,
                            City = college.City,
                            Department = college.Department
                        });

                    _logger.LogInformation(
                        "College added successfully with RollNo {RollNo}.",
                        rollNo);

                    return rollNo;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while adding a new college.");

                throw;
            }
        }

        // UPDATE COLLEGE
        public async Task<bool> UpdateCollegeAsync(
            int rollNo,
            College college)
        {
            const string query = @"
                UPDATE [dbo].[college]
                SET
                    NAME = @Name,
                    AGE = @Age,
                    CITY = @City,
                    DEPARTMENT = @Department
                WHERE ROLLNO = @RollNo";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var rowsAffected =
                        await connection.ExecuteAsync(
                            query,
                            new
                            {
                                RollNo = rollNo,
                                Name = college.Name,
                                Age = college.Age,
                                City = college.City,
                                Department = college.Department
                            });

                    if (rowsAffected > 0)
                    {
                        _logger.LogInformation(
                            "College with RollNo {RollNo} updated successfully.",
                            rollNo);

                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while updating college with RollNo {RollNo}.",
                    rollNo);

                throw;
            }
        }

        // DELETE COLLEGE
        public async Task<bool> DeleteCollegeAsync(int rollNo)
        {
            const string query = @"
                DELETE FROM [dbo].[college]
                WHERE ROLLNO = @RollNo";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var rowsAffected =
                        await connection.ExecuteAsync(
                            query,
                            new { RollNo = rollNo });

                    if (rowsAffected > 0)
                    {
                        _logger.LogInformation(
                            "College with RollNo {RollNo} deleted successfully.",
                            rollNo);

                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while deleting college with RollNo {RollNo}.",
                    rollNo);

                throw;
            }
        }
    }
}