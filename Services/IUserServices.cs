using Education.DTO;
using Education.Models;
using System.Threading.Tasks;

namespace Education.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(RegisterDto registerDto);
        Task<User> LoginUserAsync(LoginDto loginDto);
        Task<string> GenerateJwtTokenAsync(User user);
        Task LogoutUserAsync(Guid userId);
    }
}
