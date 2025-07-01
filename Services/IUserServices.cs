using Education.DTO;
using Education.Models;
using System;
using System.Threading.Tasks;

namespace Education.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(RegisterDto registerDto);
        Task<User> LoginUserAsync(LoginDto loginDto);
        Task<string> GenerateJwtTokenAsync(User user);
        Task LogoutUserAsync(Guid userId);
        Task<UserProfileDto> GetUserProfileAsync(Guid userId);
        Task<UserProfileDto> EditUserProfileAsync(Guid userId, EditUserProfileDto editDto);
    }
}
