using LMS.Application.DOTs.Authentication;
using LMS.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var response = await _authService.LoginAsync(request);

            if (!response.IsSuccess)
                return Unauthorized(response);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return Ok(new
            {
                Message = "Welcome to LMS",

                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),

                Email = User.FindFirstValue(ClaimTypes.Email),

                UserName = User.Identity?.Name
            });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("admin")]
        public IActionResult AdminOnly()
        {
            return Ok(new
            {
                Message = "Welcome Admin"
            });
        }
    }
}
