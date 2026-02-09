using Application.ProjectAssignees.Commands.CreateProjectAssignee;
using Application.TaskAssignee.Commands.CreateTaskAssignee;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectAssigneeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectAssigneeController(IMediator mediator)
        {
           _mediator = mediator;
        }

        [HttpPost("SaveProjectAssignee")]
        public async Task<IActionResult> SaveProjectAssignee(CreateProjectAssigneeCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
