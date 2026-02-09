using Application.Projects.Commands.CreateProject;
using Application.Projects.Commands.DeleteProject;
using Application.Projects.Commands.UpdateProject;
using Application.Projects.Queries.GetActiveProjects;
using Application.Projects.Queries.GetProjectById;
using Application.Projects.Queries.GetProjects;
using Application.Tags.Commands.DeleteTag;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
           _mediator = mediator;
        }

        [HttpPost("SaveProject")]
        public async Task<IActionResult> SaveProject(CreateProjectCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("GetProjectById")]
        public async Task<IActionResult> GetProjectById(Guid projectId)
        {
            var response = await _mediator.Send(new GetProjectByIdQuery() { ProjectId=projectId});
            return Ok(response);
        }

        [HttpGet("GetProjects")]
        public async Task<IActionResult> GetProjects()
        {
            var response = await _mediator.Send(new GetProjectsQuery() { });
            return Ok(response);
        }

        [HttpGet("GetActiveProjects")]
        public async Task<IActionResult> GetActiveProjects()
        {
            var response = await _mediator.Send(new GetActiveProjectsQuery() { });
            return Ok(response);
        }

        [HttpPatch("UpdateProject")]
        public async Task<IActionResult> UpdateProject(UpdateProjectCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("DeleteProject")]
        public async Task<IActionResult> DeleteProject(DeleteProjectCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
