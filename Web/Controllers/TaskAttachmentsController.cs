using Application.TaskAttachments.Commands;
using Application.TaskAttachments.Commands.CreateTaskAttachment;
using Application.TaskAttachments.Commands.UpdateTaskAttachment;
using Application.TaskAttachments.Queries.GetTaskAttachment;
using Application.TaskAttachments.Queries.GetTaskAttachmentFiles;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskAttachmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TaskAttachmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Consumes("multipart/form-data")]
        [HttpPost("SaveTaskAttachments")]
        public async Task<IActionResult> SaveTaskAttachments([FromForm] CreateTaskAttachmentCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [Consumes("multipart/form-data")]
        [HttpPatch("UpdateTaskAttachments")]
        public async Task<IActionResult> UpdateTaskAttachments([FromForm] UpdateTaskAttachmentCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("GetTaskAttachments")]
        public async Task<IActionResult> GetTaskAttachments()
        {
            var response = await _mediator.Send(new GetTaskAttachmentQuery (){ });
            return Ok(response);
        }

        [HttpGet("GetTaskAttachmentFile")]
        public async Task<IResult> GetTaskAttachmentFile([FromQuery] Guid taskAttachmentId)
        {
            var response = await _mediator.Send(new GetTaskAttachmentFileQuery() { TaskAttachmentId = taskAttachmentId });
            if (response.result.Succeeded && response.data != null)
            {
                return Results.File(response.data.FileContent, response.data.ContentType, response.data.FileName);
            }
            return Results.Ok(response);
        }
    }
}
