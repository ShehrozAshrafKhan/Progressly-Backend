using Application.Projects.Commands.UpdateProject;
using Application.Tags.Commands.CreateTags;
using Application.Tags.Commands.DeleteTag;
using Application.Tags.Commands.UpdateTag;
using Application.Tags.Queries.GetActiveTags;
using Application.Tags.Queries.GetTagById;
using Application.Tags.Queries.GetTags;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
          _mediator = mediator;
        }



        [HttpPost("SaveTag")]
        public async Task<IActionResult> SaveTag(CreateTagsCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("GetTagById")]
        public async Task<IActionResult> GetTagById(Guid tagId)
        {
            var response = await _mediator.Send(new GetTagByIdQuery() { TagId=tagId});
            return Ok(response);
        }
        [HttpGet("GetTags")]
        public async Task<IActionResult> GetTags()
        {
            var response = await _mediator.Send(new GetTagsQuery() { });
            return Ok(response);
        }

        [HttpGet("GetActiveTags")]
        public async Task<IActionResult> GetActiveTags()
        {
            var response = await _mediator.Send(new GetActiveTagsQuery() { });
            return Ok(response);
        }

        [HttpPatch("UpdateTag")]
        public async Task<IActionResult> UpdateTag(UpdateTagCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("DeleteTag")]
        public async Task<IActionResult> DeleteTag(DeleteTagCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
