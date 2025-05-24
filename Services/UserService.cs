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
                FullName = registerDto.FullName,
                Email = registerDto.Email.Trim().ToLower(),  // Trim and lowercase on registration too
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password.Trim()), // Trim password before hashing
                Address = registerDto.Address,
                BirthDate = registerDto.BirthDate,
                Gender = registerDto.Gender,
                PhoneNumber = registerDto.PhoneNumber,
                Role = registerDto.Role ?? "Student" // Default role is Student
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> LoginUserAsync(LoginDto loginDto)
        {
            var email = loginDto.Email.Trim().ToLower();

            _logger.LogInformation($"Login attempt for email: {email}");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email);

            if (user == null)
            {
                _logger.LogWarning($"Login failed: user with email {email} not found.");
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var passwordTrimmed = loginDto.Password.Trim();
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(passwordTrimmed, user.PasswordHash);

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
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role) // Add role to JWT
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task LogoutUserAsync(Guid userId)
        {
            // Token invalidation logic if needed (for now, assume logout happens by front-end).
        }

        public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return new UserProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Address = user.Address,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<UserProfileDto> EditUserProfileAsync(Guid userId, EditUserProfileDto editDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            user.FullName = editDto.FullName ?? user.FullName;
            user.Address = editDto.Address ?? user.Address;
            user.PhoneNumber = editDto.PhoneNumber ?? user.PhoneNumber;
            user.BirthDate = editDto.BirthDate ?? user.BirthDate;
            user.Gender = editDto.Gender ?? user.Gender;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UserProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Address = user.Address,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
