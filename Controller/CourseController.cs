using Education.DTO;
using Education.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Education.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto createCourseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid course data.");

            try
            {
                var course = await _courseService.CreateCourseAsync(createCourseDto);
                var courseDto = new CourseDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    InstructorName = "Instructor Name",
                    Category = course.Category,
                    Price = course.Price,
                    CreatedAt = course.CreatedAt
                };

                return CreatedAtAction(nameof(GetCourseById), new { id = courseDto.Id }, courseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(Guid id)
        {
            try
            {
                var course = await _courseService.GetCourseByIdAsync(id);
                return Ok(course);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { status = "Error", message = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCourses([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string category = null, [FromQuery] string search = null)
        {
            var courses = await _courseService.GetAllCoursesAsync(page, pageSize, category, search);
            return Ok(courses);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            try
            {
                await _courseService.DeleteCourseAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { status = "Error", message = ex.Message });
            }
        }
    }
}
