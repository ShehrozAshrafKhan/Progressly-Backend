using Application.Projects.Queries.GetProjects;
using Application.TaskAttachments.Queries.GetTaskAttachmentFiles;
using Domain.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
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
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Domain.Identity.RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Domain.Identity.LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("secure-data")]
        public IActionResult SecureData()
        {
            return Ok("You are authorized");
        }

        [HttpGet("GetProfileImage")]
        public async Task<IResult> GetProfileImage()
        {
            var response = await _authService.ProfileImageAsync();
            if (response.result.Succeeded && response.data != null)
            {
                return Results.File(response.data.FileContent, response.data.ContentType, response.data.FileName);
            }
            return Results.Ok(response);
        }

        [Consumes("multipart/form-data")]
        [HttpPatch("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromForm] Domain.Identity.UpdateUserRequest request)
        {
            var response = await _authService.UpdateUserAsync(request);
            return Ok(response);
        }

        [HttpGet("GetAdminUsers")]
        public async Task<IActionResult> GetAdminUsers()
        {
            var response = await _authService.GetAdminUsers();
            return Ok(response);
        }

        [HttpGet("GetAllActiveUsers")]
        public async Task<IActionResult> GetAllActiveUsers()
        {
            var response = await _authService.GetAllActiveUsers();
            return Ok(response);
        }
    }
}
