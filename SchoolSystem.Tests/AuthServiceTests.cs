using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolSystem.Data;
using SchoolSystem.Dto;
using SchoolSystem.Exceptions;
using SchoolSystem.Models;
using SchoolSystem.Services;
using Xunit;

namespace SchoolSystem.Tests
{
    public class AuthServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly AppDbContext _context;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(_options);

            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(config => config["Jwt:Key"]).Returns("83CF2BE5E22EE899463158DDA559FDw2");
            _mockConfig.Setup(config => config["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfig.Setup(config => config["Jwt:Audience"]).Returns("TestAudience");

            _authService = new AuthService(_context, _mockConfig.Object);
        }

        [Fact]
        public void Authenticate_ReturnsUser_WhenCredentialsAreValid()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";
            var hashedPassword = _authService.HashPassword(password);
            _context.Users.Add(new User { Username = username, Password = hashedPassword, Role = "User" });
            _context.SaveChanges();

            // Act
            var user = _authService.Authenticate(username, password);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(username, user!.Username);
        }

        [Fact]
        public void Authenticate_ReturnsNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var username = "invaliduser";
            var password = "invalidpassword";

            // Act
            var user = _authService.Authenticate(username, password);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public void GenerateJwtToken_ReturnsValidToken()
        {
            // Arrange
            var user = new User { Username = "testuser", Role = "User" };

            // Act
            var token = _authService.GenerateJwtToken(user);

            // Assert
            Assert.NotNull(token);
            Assert.IsType<string>(token);
        }

        [Fact]
        public async Task CreateUser_AddsUserToDatabase()
        {
            // Arrange
            var userDto = new UserDto { Username = "newuser", Password = "newpassword" };

            // Act
            var newUser = await _authService.CreateUser(userDto);

            // Assert
            Assert.NotNull(newUser);
            Assert.Equal(userDto.Username, newUser.Username);
            Assert.Equal(1, _context.Users.Count());
        }

        [Fact]
        public async Task CreateUser_ThrowsException_WhenUsernameExists()
        {
            // Arrange
            var username = "existinguser";
            _context.Users.Add(new User { Username = username, Password = "somepassword" });
            _context.SaveChanges();
            var userDto = new UserDto { Username = username, Password = "newpassword" };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await _authService.CreateUser(userDto));
        }

        [Fact]
        public async Task InitAdmin_AddsAdminUserToDatabase()
        {
            // Act
            var adminUser = await _authService.InitAdmin();

            // Assert
            Assert.NotNull(adminUser);
            Assert.Equal("admin", adminUser.Username);
            Assert.Equal("Admin", adminUser.Role);
        }

        [Fact]
        public async Task InitAdmin_ThrowsException_WhenAdminExists()
        {
            // Arrange
            _context.Users.Add(new User { Username = "admin", Password = "somepassword", Role = "Admin" });
            _context.SaveChanges();

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await _authService.InitAdmin());
        }

        [Fact]
        public void HashPassword_ReturnsConsistentHash()
        {
            // Arrange
            var password = "mypassword";

            // Act
            var hash1 = _authService.HashPassword(password);
            var hash2 = _authService.HashPassword(password);

            // Assert
            Assert.Equal(hash1, hash2);
        }
    }
}
