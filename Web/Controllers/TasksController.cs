using Application.Common.Models;
using Application.Projects.Commands.CreateProject;
using Application.Projects.Commands.DeleteProject;
using Application.Projects.Commands.UpdateProject;
using Application.Projects.Queries.GetActiveProjects;
using Application.Projects.Queries.GetProjects;
using Application.Tags.Queries.GetActiveTags;
using Application.Tags.Queries.GetTags;
using Application.Tasks.Commands.CreateTask;
using Application.Tasks.Commands.DeleteTask;
using Application.Tasks.Commands.UpdateTask;
using Application.Tasks.Commands.UpdateTaskStatus;
using Application.Tasks.Queries.GetCompletedTasks;
using Application.Tasks.Queries.GetIsCompletedTasks;
using Application.Tasks.Queries.GetIsCompletedTasksCount;
using Application.Tasks.Queries.GetPendingTasks;
using Application.Tasks.Queries.GetTaskById;
using Application.Tasks.Queries.GetTasks;
using Application.Tasks.Queries.GetTasksCount;
using Application.Tasks.Queries.GetUserDashboardTasks;
using Application.Tasks.Queries.GetUserTasksDetail;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("SaveTask")]
        public async Task<IActionResult> SaveTask(CreateTaskCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("GetTaskById")]
        public async Task<IActionResult> GetTaskById(Guid? taskId)
        {
            var response = await _mediator.Send(new GetTaskByIdQuery() { TaskId = taskId });
            return Ok(response);
        }

        [HttpGet("GetTasks")]
        public async Task<IActionResult> GetTasks()
        {
            var response = await _mediator.Send(new GetTasksQuery() { });
            return Ok(response);
        }

        [HttpGet("GetTasksCount")]
        public async Task<IActionResult> GetTasksCount()
        {
            var response = await _mediator.Send(new GetTasksCountQuery() { });
            return Ok(response);
        }

        [HttpPatch("UpdateTask")]
        public async Task<IActionResult> UpdateTask(UpdateTaskCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("DeleteTask")]
        public async Task<IActionResult> DeleteTask(DeleteTaskCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("GetUserTasksCount")]
        public async Task<IActionResult> GetUserTasksCount()
        {
            var response = await _mediator.Send(new GetUserDashboardTasksQuery() { });
            return Ok(response);
        }
      
        [HttpGet("GetUserTasks")]
        public async Task<IActionResult> GetUserTasks(string? type)
        {
            var response = await _mediator.Send(new GetUserTasksDetailQuery() { Type=type });
            return Ok(response);
        }

        [HttpGet("GetIsCompletedTasksCount")]
        public async Task<IActionResult> GetIsCompletedTasksCount()
        {
            var response = await _mediator.Send(new GetIsCompletedTasksCountQuery() {});
            return Ok(response);
        }

        [HttpGet("GetIsCompletedTasks")]
        public async Task<IActionResult> GetIsCompletedTasks()
        {
            var response = await _mediator.Send(new GetIsCompletedTasksQuery() {});
            return Ok(response);
        }

        [HttpPatch("UpdateTaskStatus")]
        public async Task<IActionResult> UpdateTaskStatus(UpdateTaskStatusCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("GetCompletedTasks")]
        public async Task<IActionResult> GetCompletedTasks()
        {
            var response = await _mediator.Send(new GetCompletedTasksQuery() { });
            return Ok(response);
        }

        [HttpGet("GetPendingTasks")]
        public async Task<IActionResult> GetPendingTasks()
        {
            var response = await _mediator.Send(new GetPendingTasksQuery() { });
            return Ok(response);
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadCodeChanges([FromForm] IFormCollection formData)
        {
            var taskId = formData["taskId"];
            var groupedByPrefix = formData
            .Where(f => f.Key.Contains("_tagId"))
            .ToDictionary(k => k.Key.Split("_")[0] + "_" + k.Key.Split("_")[1], v => v.Value.ToString());

            foreach (var group in groupedByPrefix)
            {
                var prefix = group.Key; // e.g., 74f3..._0
                var tagId = group.Value;

                // Now find files like: 74f3..._0_oldFile, 74f3..._0_newFile
                var oldFile = formData.Files.FirstOrDefault(f => f.Name == $"{prefix}_oldFile");
                var newFile = formData.Files.FirstOrDefault(f => f.Name == $"{prefix}_newFile");

                if (oldFile != null)
                {
                    var path = Path.Combine("UploadDir", oldFile.FileName);
                    using var stream = new FileStream(path, FileMode.Create);
                    await oldFile.CopyToAsync(stream);
                }

                if (newFile != null)
                {
                    var path = Path.Combine("UploadDir", newFile.FileName);
                    using var stream = new FileStream(path, FileMode.Create);
                    await newFile.CopyToAsync(stream);
                }

                Console.WriteLine($"Processed for TagId: {tagId}");
            }

            foreach (var file in formData.Files)
            {
                var fileName = file.FileName;
                var filePath = Path.Combine("YourUploadDirectory", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return Ok(new { result = Result.Success(), message = "Uploaded" });
        }

    }
}
