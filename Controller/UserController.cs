using Education.DTO;
using Education.Models;
using Education.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
                return StatusCode(500, new { status = "Error", message = ex.Message });
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
    }
}
