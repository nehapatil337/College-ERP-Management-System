using CollegeERP.Models;

namespace CollegeERP.Repositories
{
    public interface ICollegeRepository
    {
        Task<IEnumerable<College>> GetAllCollegesAsync();
        Task<College?> GetCollegeByRollNoAsync(int rollNo);
        Task<int> AddCollegeAsync(College college);
        Task<bool> UpdateCollegeAsync(int rollNo, College college);
        Task<bool> DeleteCollegeAsync(int rollNo);
    }
}