using Application.Tasks.Commands.CreateTask;
using Application.TaskTags.Commands.CreateTaskTag;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTagController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskTagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("SaveTaskTag")]
        public async Task<IActionResult> SaveTaskTag(CreateTaskTagCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
