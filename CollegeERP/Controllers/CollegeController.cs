using CollegeERP.DTOs;
using CollegeERP.Models;
using CollegeERP.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CollegeERP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollegeController : ControllerBase
    {
        private readonly ICollegeRepository _collegeRepository;
        private readonly ILogger<CollegeController> _logger;

        public CollegeController(ICollegeRepository collegeRepository, ILogger<CollegeController> logger)
        {
            _collegeRepository = collegeRepository;
            _logger = logger;
        }

        // GET: api/college
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CollegeDto>>>> GetAllColleges()
        {
            try
            {
                var colleges = await _collegeRepository.GetAllCollegesAsync();
                var collegeDtos = colleges.Select(c => new CollegeDto
                {
                    RollNo = c.RollNo,
                    Name = c.Name,
                    Age = c.Age,
                    City = c.City,
                    Department = c.Department
                });

                return Ok(new ApiResponse<IEnumerable<CollegeDto>>(
                    success: true,
                    message: "Colleges retrieved successfully",
                    data: collegeDtos
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving colleges");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<IEnumerable<CollegeDto>>(
                        success: false,
                        message: "An error occurred while retrieving colleges",
                        data: null
                    ));
            }
        }

        // GET: api/college/{rollNo}
        [HttpGet("{rollNo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<CollegeDto>>> GetCollegeByRollNo(int rollNo)
        {
            try
            {
                var college = await _collegeRepository.GetCollegeByRollNoAsync(rollNo);
                if (college == null)
                {
                    return NotFound(new ApiResponse<CollegeDto>(
                        success: false,
                        message: $"College with Roll Number {rollNo} not found",
                        data: null
                    ));
                }

                var collegeDto = new CollegeDto
                {
                    RollNo = college.RollNo,
                    Name = college.Name,
                    Age = college.Age,
                    City = college.City,
                    Department = college.Department
                };

                return Ok(new ApiResponse<CollegeDto>(
                    success: true,
                    message: "College retrieved successfully",
                    data: collegeDto
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving college");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<CollegeDto>(
                        success: false,
                        message: "An error occurred while retrieving the college",
                        data: null
                    ));
            }
        }

        // POST: api/college
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<CollegeDto>>> CreateCollege([FromBody] CreateCollegeDto createCollegeDto)
        {
            try
            {
                if (createCollegeDto == null)
                {
                    return BadRequest(new ApiResponse<CollegeDto>(
                        success: false,
                        message: "Request body is required",
                        data: null
                    ));
                }

                var college = new College
                {
                    Name = createCollegeDto.Name,
                    Age = createCollegeDto.Age,
                    City = createCollegeDto.City,
                    Department = createCollegeDto.Department
                };

                var rollNo = await _collegeRepository.AddCollegeAsync(college);

                var collegeDto = new CollegeDto
                {
                    RollNo = rollNo,
                    Name = college.Name,
                    Age = college.Age,
                    City = college.City,
                    Department = college.Department
                };

                return CreatedAtAction(nameof(GetCollegeByRollNo), new { rollNo },
                    new ApiResponse<CollegeDto>(
                        success: true,
                        message: "College created successfully",
                        data: collegeDto
                    ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a college");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<CollegeDto>(
                        success: false,
                        message: "An error occurred while creating the college",
                        data: null
                    ));
            }
        }

        // PUT: api/college/{rollNo}
        [HttpPut("{rollNo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<CollegeDto>>> UpdateCollege(int rollNo, [FromBody] UpdateCollegeDto updateCollegeDto)
        {
            try
            {
                if (updateCollegeDto == null)
                {
                    return BadRequest(new ApiResponse<CollegeDto>(
                        success: false,
                        message: "Request body is required",
                        data: null
                    ));
                }

                var college = new College
                {
                    RollNo = rollNo,
                    Name = updateCollegeDto.Name,
                    Age = updateCollegeDto.Age,
                    City = updateCollegeDto.City,
                    Department = updateCollegeDto.Department
                };

                var success = await _collegeRepository.UpdateCollegeAsync(rollNo, college);
                if (!success)
                {
                    return NotFound(new ApiResponse<CollegeDto>(
                        success: false,
                        message: $"College with Roll Number {rollNo} not found",
                        data: null
                    ));
                }

                var collegeDto = new CollegeDto
                {
                    RollNo = college.RollNo,
                    Name = college.Name,
                    Age = college.Age,
                    City = college.City,
                    Department = college.Department
                };

                return Ok(new ApiResponse<CollegeDto>(
                    success: true,
                    message: "College updated successfully",
                    data: collegeDto
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a college");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<CollegeDto>(
                        success: false,
                        message: "An error occurred while updating the college",
                        data: null
                    ));
            }
        }

        // DELETE: api/college/{rollNo}
        [HttpDelete("{rollNo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> DeleteCollege(int rollNo)
        {
            try
            {
                var success = await _collegeRepository.DeleteCollegeAsync(rollNo);
                if (!success)
                {
                    return NotFound(new ApiResponse<string>(
                        success: false,
                        message: $"College with Roll Number {rollNo} not found",
                        data: null
                    ));
                }

                return Ok(new ApiResponse<string>(
                    success: true,
                    message: "College deleted successfully",
                    data: null
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a college");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>(
                        success: false,
                        message: "An error occurred while deleting the college",
                        data: null
                    ));
            }
        }
    }
}