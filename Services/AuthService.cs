using Microsoft.IdentityModel.Tokens;
using SchoolSystem.Data;
using SchoolSystem.Dto;
using SchoolSystem.Exceptions;
using SchoolSystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SchoolSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public User? Authenticate(string username, string password)
        {
            var saltedPassword = this.HashPassword(password);
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == saltedPassword);
        }

        public User GetUser(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) throw new NotFoundException("User not found");
            return user;
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string HashPassword(string password)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + key;
                var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public async Task<User> CreateUser(UserDto userDto)
        {
            var existUser = _context.Users.Any(u => u.Username == userDto.Username);
            if (existUser)
            {
                throw new BadRequestException("Username already exists");
            }
            User newUser = new User
            {
                Username = userDto.Username,
                Password = this.HashPassword(userDto.Password),
                Role = "User"
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<User> InitAdmin()
        {
            var existUser = _context.Users.Any(u => u.Username == "admin");
            if (existUser)
            {
                throw new BadRequestException("Already init auth");
            }
            User newUser = new User
            {
                Username = "admin",
                Password = this.HashPassword("admin"),
                Role = "Admin"
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }
    }
}
