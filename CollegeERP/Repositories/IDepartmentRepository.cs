using CollegeERP.Models;

namespace CollegeERP.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();

        Task<Department?> GetDepartmentByIdAsync(int departmentId);

        Task<int> AddDepartmentAsync(Department department);

        Task<bool> UpdateDepartmentAsync(int departmentId, Department department);

        Task<bool> DeleteDepartmentAsync(int departmentId);
    }
}