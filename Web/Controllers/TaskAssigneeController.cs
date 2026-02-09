using Application.Projects.Commands.CreateProject;
using Application.TaskAssignee.Commands.CreateTaskAssignee;
using Application.TaskAssignee.Commands.UpdateTaskAssignee;
using Application.TaskAssignee.Queries.GetTaskAssigneeById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskAssigneeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskAssigneeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("SaveTaskAssignee")]
        public async Task<IActionResult> SaveTaskAssignee(CreateTaskAssigneeCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPatch("UpdateTaskAssignee")]
        public async Task<IActionResult> UpdateTaskAssignee(UpdateTaskAssigneeCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("GetTaskAssigneeById")]
        public async Task<IActionResult> GetTaskAssigneeById(Guid? taskAssigneeId)
        {
            var response = await _mediator.Send(new GetTaskAssigneeByIdQuery() { TaskAssigneeId=taskAssigneeId});
            return Ok(response);
        }
    }
}
