using SchoolSystem.Dto;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        User? Authenticate(string username, string password);
        User GetUser(string username);

        Task<User> CreateUser(UserDto userDto);
    }
}
