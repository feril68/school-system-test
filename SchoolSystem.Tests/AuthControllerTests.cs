using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolSystem.Controllers;
using SchoolSystem.Dto;
using SchoolSystem.Exceptions;
using SchoolSystem.Models;
using SchoolSystem.Services;
using Xunit;

namespace SchoolSystem.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public void Login_ReturnsOkResult_WithToken_WhenCredentialsAreValid()
        {
            // Arrange
            var userDto = new UserDto { Username = "testuser", Password = "testpassword" };
            var user = new User { Username = "testuser", Role = "User" };
            var token = "mockJwtToken";

            _mockAuthService.Setup(s => s.Authenticate(userDto.Username, userDto.Password)).Returns(user);
            _mockAuthService.Setup(s => s.GenerateJwtToken(user)).Returns(token);

            // Act
            var result = _controller.Login(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedToken = Assert.IsType<string>(okResult.Value!.GetType().GetProperty("token")!.GetValue(okResult.Value));
            Assert.Equal(token, returnedToken);
        }

        [Fact]
        public void Login_ThrowsUnauthorizedException_WhenCredentialsAreInvalid()
        {
            // Arrange
            var userDto = new UserDto { Username = "invaliduser", Password = "wrongpassword" };
            _mockAuthService.Setup(s => s.Authenticate(userDto.Username, userDto.Password)).Returns((User?)null);

            // Act & Assert
            Assert.Throws<UnauthorizedException>(() => _controller.Login(userDto));
        }

        [Fact]
        public async Task Register_ReturnsNewUser_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var userDto = new UserDto { Username = "newuser", Password = "password" };
            var newUser = new User { Username = userDto.Username, Role = "User" };

            _mockAuthService.Setup(s => s.CreateUser(userDto)).ReturnsAsync(newUser);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<User>>(result);
            var returnedUser = Assert.IsType<User>(actionResult.Value);
            Assert.Equal(newUser.Username, returnedUser.Username);
        }

        [Fact]
        public async Task Register_ThrowsBadRequestException_WhenUsernameAlreadyExists()
        {
            // Arrange
            var userDto = new UserDto { Username = "existinguser", Password = "password" };
            _mockAuthService.Setup(s => s.CreateUser(userDto)).ThrowsAsync(new BadRequestException("Username already exists"));

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await _controller.Register(userDto));
        }

        [Fact]
        public async Task InitRegister_ReturnsAdminUser_WhenSuccessful()
        {
            // Arrange
            var adminUser = new User { Username = "admin", Role = "Admin" };
            _mockAuthService.Setup(s => s.InitAdmin()).ReturnsAsync(adminUser);

            // Act
            var result = await _controller.InitRegister();

            // Assert
            var actionResult = Assert.IsType<ActionResult<User>>(result);
            var returnedUser = Assert.IsType<User>(actionResult.Value);
            Assert.Equal(adminUser.Username, returnedUser.Username);
            Assert.Equal(adminUser.Role, returnedUser.Role);
        }

        [Fact]
        public async Task InitRegister_ThrowsBadRequestException_WhenAdminAlreadyExists()
        {
            // Arrange
            _mockAuthService.Setup(s => s.InitAdmin()).ThrowsAsync(new BadRequestException("Already init auth"));

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await _controller.InitRegister());
        }
    }
}
