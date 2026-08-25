using CollegeERP.Models;
using CollegeERP.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CollegeERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(
            IDepartmentRepository departmentRepository,
            ILogger<DepartmentController> logger)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        // GET: api/department
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartments()
        {
            try
            {
                var departments =
                    await _departmentRepository.GetAllDepartmentsAsync();

                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while retrieving departments.");

                return StatusCode(500, new
                {
                    success = false,
                    message = "Error while retrieving departments."
                });
            }
        }

        // GET: api/department/1
        [HttpGet("{departmentId}")]
        public async Task<ActionResult<Department>> GetDepartmentById(
            int departmentId)
        {
            try
            {
                var department =
                    await _departmentRepository
                        .GetDepartmentByIdAsync(departmentId);

                if (department == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Department not found."
                    });
                }

                return Ok(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while retrieving department.");

                return StatusCode(500, new
                {
                    success = false,
                    message = "Error while retrieving department."
                });
            }
        }

        // POST: api/department
        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment(
            [FromBody] Department department)
        {
            try
            {
                if (department == null ||
                    string.IsNullOrWhiteSpace(department.DepartmentName))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Department name is required."
                    });
                }

                var departmentId =
                    await _departmentRepository
                        .AddDepartmentAsync(department);

                department.DepartmentId = departmentId;

                return CreatedAtAction(
                    nameof(GetDepartmentById),
                    new { departmentId = departmentId },
                    department);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while creating department.");

                return StatusCode(500, new
                {
                    success = false,
                    message = "Error while creating department."
                });
            }
        }

        // PUT: api/department/1
        [HttpPut("{departmentId}")]
        public async Task<IActionResult> UpdateDepartment(
            int departmentId,
            [FromBody] Department department)
        {
            try
            {
                if (department == null ||
                    string.IsNullOrWhiteSpace(department.DepartmentName))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Department name is required."
                    });
                }

                var success =
                    await _departmentRepository
                        .UpdateDepartmentAsync(
                            departmentId,
                            department);

                if (!success)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Department not found."
                    });
                }

                department.DepartmentId = departmentId;

                return Ok(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while updating department.");

                return StatusCode(500, new
                {
                    success = false,
                    message = "Error while updating department."
                });
            }
        }

        // DELETE: api/department/1
        [HttpDelete("{departmentId}")]
        public async Task<IActionResult> DeleteDepartment(
            int departmentId)
        {
            try
            {
                var success =
                    await _departmentRepository
                        .DeleteDepartmentAsync(departmentId);

                if (!success)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Department not found."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Department deleted successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while deleting department.");

                return StatusCode(500, new
                {
                    success = false,
                    message = "Error while deleting department."
                });
            }
        }
    }
}