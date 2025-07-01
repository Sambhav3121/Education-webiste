using Education.DTO;
using Education.Models;
using Education.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Threading.Tasks;

namespace Education.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        // Allow Teacher and Admin to create lessons
        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> CreateLesson([FromBody] LessonCreateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { status = "Error", message = "User not authenticated" });

            try
            {
                var lesson = await _lessonService.CreateLessonAsync(dto, userId);
                return Ok(new { status = "Success", message = "Lesson created successfully.", data = lesson });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }
    }
}
