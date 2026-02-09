using Application.Module.Commands.CreateModule;
using Application.Modules.Commands.DeleteModule;
using Application.Modules.Commands.UpdateModule;
using Application.Modules.Queries.GetActiveModules;
using Application.Modules.Queries.GetModuleById;
using Application.Modules.Queries.GetModules;
using Application.Projects.Commands.CreateProject;
using Application.Projects.Commands.DeleteProject;
using Application.Projects.Commands.UpdateProject;
using Application.Projects.Queries.GetActiveProjects;
using Application.Projects.Queries.GetProjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ModulesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("SaveModule")]
        public async Task<IActionResult> SaveModule(CreateModuleCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("GetModuleById")]
        public async Task<IActionResult> GetModuleById(Guid moduleId)
        {
            var response = await _mediator.Send(new GetModuleByIdQuery() {ModuleId= moduleId });
            return Ok(response);
        }
        [HttpGet("GetModules")]
        public async Task<IActionResult> GetModules()
        {
            var response = await _mediator.Send(new GetModulesQuery() { });
            return Ok(response);
        }

        [HttpGet("GetActiveModules")]
        public async Task<IActionResult> GetActiveModules()
        {
            var response = await _mediator.Send(new GetActiveModulesQuery() { });
            return Ok(response);
        }

        [HttpPatch("UpdateModule")]
        public async Task<IActionResult> UpdateModule(UpdateModuleCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("DeleteModule")]
        public async Task<IActionResult> DeleteModule(DeleteModuleCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
