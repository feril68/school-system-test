using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Dto;
using SchoolSystem.Exceptions;
using SchoolSystem.Models;
using SchoolSystem.Services;

namespace SchoolSystem.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDto userDto)
        {
            var authenticatedUser = _authService.Authenticate(userDto.Username, userDto.Password);
            if (authenticatedUser == null)
            {
                throw new UnauthorizedException("Invalid credentials");
            }

            var token = _authService.GenerateJwtToken(authenticatedUser);
            return Ok(new { token });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserDto userDto)
        {
            var newUser = await _authService.CreateUser(userDto);
            return newUser;
        }

        [HttpPost("init")]
        public async Task<ActionResult<User>> InitRegister()
        {
            var newUser = await _authService.InitAdmin();
            return newUser;
        }
    }
}
