using Application.GeneralSettings.Commands.CreateApplicationName_Logo;
using Application.GeneralSettings.Queries.GetApplicationName_Logo;
using Application.Tags.Queries.GetActiveTags;
using Application.TaskAttachments.Commands.CreateTaskAttachment;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralSettingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GeneralSettingsController(IMediator mediator)
        {
           _mediator = mediator;
        }

        [Consumes("multipart/form-data")]
        [HttpPost("SaveApplicationNameLogo")]
        public async Task<IActionResult> SaveApplicationNameLogo([FromForm] CreateApplicationName_LogoCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("GetApplicationNameLogo")]
        public async Task<IActionResult> GetApplicationNameLogo()
        {
            var response = await _mediator.Send(new GetApplicationName_LogoQuery() { });
            return Ok(response);
        }
    }
}
