using Application.Tags.Commands.DeleteTag;
using Application.Tags.Queries.GetTagById;
using Application.TaskAttachments.Queries.GetTaskAttachmentFiles;
using Application.TaskCodeChanges.Commands.DeleteTaskCodeChange;
using Application.TaskCodeChanges.Commands.UploadCodeChanges;
using Application.TaskCodeChanges.Queries.GetTaskCodeChanges;
using Application.TaskCodeChanges.Queries.GetTaskCodeChangesFiles;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskCodeChangesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskCodeChangesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("UploadTaskCodeChanges")]
        public async Task<IActionResult> UploadTaskCodeChanges()
        {
            var form = await Request.ReadFormAsync();
            var command = new UploadCodeChangesCommand(form);
            var result = await _mediator.Send(command);

            if (result.result.Succeeded)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetTaskCodeChangesByTaskId")]
        public async Task<IActionResult> GetTaskCodeChangesByTaskId(Guid? taskId)
        {
            var response = await _mediator.Send(new GetTaskCodeChangesQuery() { TaskId = taskId });
            return Ok(response);
        }

        [HttpDelete("DeleteTaskCodeChange")]
        public async Task<IActionResult> DeleteTaskCodeChange(DeleteTaskCodeChangeCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("GetTaskCodeChangeFile")]
        public async Task<IResult> GetTaskCodeChangeFile([FromQuery] Guid taskCodeChangeId,string? fileName,string? fileType)
        {
            var response = await _mediator.Send(new GetTaskCodeChangesFilesQuery() { TaskCodeChangeId = taskCodeChangeId,FileName=fileName,FileType=fileType });
            if (response.result.Succeeded && response.data != null)
            {
                return Results.File(response.data.FileContent, response.data.ContentType, response.data.FileName);
            }
            return Results.Ok(response);
        }
    }
}
