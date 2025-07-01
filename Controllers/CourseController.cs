using Education.DTO;
using Education.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { status = "Error", message = "User not authenticated" });

            try
            {
                var course = await _courseService.CreateCourseAsync(dto, userId);
                return Ok(new
                {
                    status = "Success",
                    message = "Course created successfully.",
                    data = course
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "Error",
                    message = ex.Message,
                    innerException = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}
