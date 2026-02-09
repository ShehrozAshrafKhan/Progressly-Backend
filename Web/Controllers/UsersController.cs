using Application.Projects.Queries.GetProjectById;
using Application.Users.Commands.GetUserByUserIds;
using Application.Users.Queries.GetUsersByModule;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;

        public UsersController(IMediator mediator, IAuthService authService)
        {
            _mediator = mediator;
            _authService = authService;
        }

        [HttpGet("GetAllActiveUsersCount")]
        public async Task<IActionResult> GetAllActiveUsersCount()
        {
            var response = await _authService.GetAllActiveUsersCount();
            return Ok(response);
        }

        [HttpGet("GetUserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var response = await _authService.GetUserInfo();
            return Ok(response);
        }

        [HttpGet("GetUsersByModuleId")]
        public async Task<IActionResult> GetUsersByModuleId(Guid moduleId)
        {
            var response = await _mediator.Send(new GetUsersByModuleQuery() { ModuleId = moduleId });
            return Ok(response);
        }

        [HttpPost("GetUserByIds")]
        public async Task<IActionResult> GetUserByIds(GetUserByUserIdsCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
