using CollegeERP.Models;
using CollegeERP.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CollegeERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;

        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            try
            {
                var courses = await _courseRepository.GetAllCourses();

                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while retrieving courses",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourse(int courseId)
        {
            try
            {
                var course = await _courseRepository.GetCourseById(courseId);

                if (course == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Course not found"
                    });
                }

                return Ok(course);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error retrieving course",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            try
            {
                int id = await _courseRepository.AddCourse(course);

                return Ok(new
                {
                    success = true,
                    message = "Course added successfully",
                    courseId = id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error adding course",
                    error = ex.Message
                });
            }
        }

        [HttpPut("{courseId}")]
        public async Task<IActionResult> UpdateCourse(
            int courseId,
            [FromBody] Course course)
        {
            try
            {
                bool updated =
                    await _courseRepository.UpdateCourse(courseId, course);

                if (!updated)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Course not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Course updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error updating course",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            try
            {
                bool deleted =
                    await _courseRepository.DeleteCourse(courseId);

                if (!deleted)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Course not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Course deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error deleting course",
                    error = ex.Message
                });
            }
        }
    }
}