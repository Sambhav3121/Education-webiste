using Education.DTO;
using Education.Models;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Education.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace Education.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtSecurityDto _jwtSettings;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbContext context, IOptions<JwtSecurityDto> jwtSettings, ILogger<UserService> logger)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public async Task<User> RegisterUserAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = registerDto.FullName?.Trim(),
                Email = registerDto.Email?.Trim().ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password?.Trim()),
                Address = registerDto.Address?.Trim(),
                BirthDate = registerDto.BirthDate,
                Gender = registerDto.Gender,
                PhoneNumber = registerDto.PhoneNumber?.Trim(),
                Role = registerDto.Role ?? "Student"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> LoginUserAsync(LoginDto loginDto)
        {
            var email = loginDto.Email?.Trim().ToLower();
            var password = loginDto.Password?.Trim();

            _logger.LogInformation($"Login attempt for email: {email}");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email);

            if (user == null)
            {
                _logger.LogWarning($"Login failed: user with email {email} not found.");
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            _logger.LogInformation($"User found: {user.Email}, hashed password: {user.PasswordHash}");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            _logger.LogInformation($"Password verification result for {email}: {isPasswordValid}");

            if (!isPasswordValid)
            {
                _logger.LogWarning($"Login failed: invalid password for email {email}.");
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            return user;
        }

        public async Task<string> GenerateJwtTokenAsync(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("FullName", user.FullName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task LogoutUserAsync(Guid userId)
        {
            // JWT tokens are stateless; implement token blacklisting here if needed.
            _logger.LogInformation($"LogoutUserAsync called for user ID: {userId}");
            await Task.CompletedTask;
        }

        // Implement the missing interface methods

        public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            return new UserProfileDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Role = user.Role
            };
        }

        public async Task<UserProfileDto> EditUserProfileAsync(Guid userId, EditUserProfileDto editDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            user.FullName = editDto.FullName?.Trim() ?? user.FullName;
            user.Address = editDto.Address?.Trim() ?? user.Address;
            user.PhoneNumber = editDto.PhoneNumber?.Trim() ?? user.PhoneNumber;
            user.BirthDate = editDto.BirthDate ?? user.BirthDate;
            user.Gender = editDto.Gender ?? user.Gender;

            await _context.SaveChangesAsync();

            return new UserProfileDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Role = user.Role
            };
        }
    }
}
