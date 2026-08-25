using CollegeERP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CollegeERP.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<DepartmentRepository> _logger;

        public DepartmentRepository(
            IConfiguration configuration,
            ILogger<DepartmentRepository> logger)
        {
            _logger = logger;

            _connectionString =
                "Server=127.0.0.1,62100;Database=college;Integrated Security=true;Encrypt=false;";
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            const string query = @"
                SELECT
                    DepartmentId,
                    DepartmentName,
                    HOD
                FROM dbo.Departments
                ORDER BY DepartmentId";

            using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            return await connection.QueryAsync<Department>(query);
        }

        public async Task<Department?> GetDepartmentByIdAsync(int departmentId)
        {
            const string query = @"
                SELECT
                    DepartmentId,
                    DepartmentName,
                    HOD
                FROM dbo.Departments
                WHERE DepartmentId = @DepartmentId";

            using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            return await connection.QueryFirstOrDefaultAsync<Department>(
                query,
                new { DepartmentId = departmentId });
        }

        public async Task<int> AddDepartmentAsync(Department department)
        {
            const string query = @"
                INSERT INTO dbo.Departments
                (
                    DepartmentName,
                    HOD
                )
                VALUES
                (
                    @DepartmentName,
                    @HOD
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            return await connection.QuerySingleAsync<int>(
                query,
                department);
        }

        public async Task<bool> UpdateDepartmentAsync(
            int departmentId,
            Department department)
        {
            const string query = @"
                UPDATE dbo.Departments
                SET
                    DepartmentName = @DepartmentName,
                    HOD = @HOD
                WHERE DepartmentId = @DepartmentId";

            using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            var rows = await connection.ExecuteAsync(
                query,
                new
                {
                    DepartmentId = departmentId,
                    DepartmentName = department.DepartmentName,
                    HOD = department.HOD
                });

            return rows > 0;
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            const string query = @"
                DELETE FROM dbo.Departments
                WHERE DepartmentId = @DepartmentId";

            using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            var rows = await connection.ExecuteAsync(
                query,
                new { DepartmentId = departmentId });

            return rows > 0;
        }
    }
}