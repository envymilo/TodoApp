using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.DTOs;
using TodoApp.Infrastructure.Services.TaskDependency;
using TodoApp.Infrastructure.Services.TaskItem;

namespace TodoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskDependencyController : ControllerBase
    {
        private readonly ITaskDependencyService _taskDependencyService;
        private readonly IMapper _mapper;


        public TaskDependencyController(ITaskDependencyService taskDependencyService, IMapper mapper)
        {
            _taskDependencyService = taskDependencyService;
            _mapper = mapper;
        }

        [HttpPost("{taskId}/dependent/{dependsOnTaskId}")]
        public async Task<IActionResult> AddDependency(Guid taskId, Guid dependsOnTaskId)
        {
            var result = await _taskDependencyService.AddTaskDependencyAsync(taskId, dependsOnTaskId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpDelete("{taskId}/dependent/{dependsOnTaskId}")]
        public async Task<IActionResult> RemoveDependency(Guid taskId, Guid dependsOnTaskId)
        {
            var result = await _taskDependencyService.RemoveDependencyAsync(taskId, dependsOnTaskId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
    }
}
