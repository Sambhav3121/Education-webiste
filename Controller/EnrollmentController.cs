using Education.DTO;
using Education.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Education.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost("enroll")]
        [Authorize]
        public async Task<IActionResult> Enroll([FromBody] EnrollmentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { status = "Error", message = "Invalid enrollment data." });

            try
            {
                var enrollment = await _enrollmentService.EnrollUserAsync(dto);
                return Ok(new { status = "Success", message = "Enrollment successful.", data = enrollment });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetEnrollments(Guid userId)
        {
            try
            {
                var enrollments = await _enrollmentService.GetUserEnrollmentsAsync(userId);
                return Ok(new { status = "Success", data = enrollments });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }

        [HttpDelete("{enrollmentId}")]
        [Authorize]
        public async Task<IActionResult> Unenroll(Guid enrollmentId)
        {
            try
            {
                var result = await _enrollmentService.UnenrollUserAsync(enrollmentId);

                if (!result)
                    return NotFound(new { status = "Error", message = "Enrollment not found." });

                return Ok(new { status = "Success", message = "Unenrolled successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }
    }
}
