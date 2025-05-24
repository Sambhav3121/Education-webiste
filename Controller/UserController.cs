using Education.DTO;
using Education.Models;
using Education.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 
using System;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Education.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid registration data.");

            try
            {
                var user = await _userService.RegisterUserAsync(registerDto);
                var token = await _userService.GenerateJwtTokenAsync(user);

                return Ok(new { status = "Success", message = "Registration successful.", token });
            }
           catch (Exception ex)
        {
        var errorMessage = ex.Message;
        if (ex.InnerException != null)
        errorMessage += " | Inner exception: " + ex.InnerException.Message;

        return StatusCode(500, new { status = "Error", message = errorMessage });
        }

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid login data.");

            try
            {
                var user = await _userService.LoginUserAsync(loginDto);
                var token = await _userService.GenerateJwtTokenAsync(user);

                return Ok(new { status = "Success", message = "Login successful.", token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { status = "Success", message = "Logged out successfully." });
        }

        [HttpGet("profile")]
        [Authorize] // ✅ This now works with the added using directive
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { status = "Error", message = "User not authenticated" });
            }

            try
            {
                var profile = await _userService.GetUserProfileAsync(Guid.Parse(userId));
                return Ok(profile);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { status = "Error", message = ex.Message });
            }
        }
        [HttpPut("profile")]
        [Authorize] 
        public async Task<IActionResult> EditUserProfile([FromBody] EditUserProfileDto editDto)
     {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

      if (string.IsNullOrEmpty(userId))
       {
        return Unauthorized(new { status = "Error", message = "User not authenticated" });
         }

         try
       {
         var updatedProfile = await _userService.EditUserProfileAsync(Guid.Parse(userId), editDto);
         return Ok(new { status = "Success", message = "Profile updated successfully.", data = updatedProfile });
        }
           catch (KeyNotFoundException ex)
        {
             return NotFound(new { status = "Error", message = ex.Message });
         }
       }

    }
}
