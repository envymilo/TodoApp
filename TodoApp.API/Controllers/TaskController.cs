using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TodoApp.Core.DTOs;
using TodoApp.Core.Entities;
using TodoApp.Core.Enum;
using TodoApp.Infrastructure.Services.TaskItem;
namespace TodoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;


        public TaskController(ITaskService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetTasks(
            [FromQuery] string? search,
            [FromQuery] TaskPriority? priority = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
                        [FromQuery] bool? isCompleted = null)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Page and PageSize must be greater than zero.");

            try
            {
                var paginatedTasks = await _taskService.GetPaginatedTasksAsync(page, pageSize, search, priority, isCompleted);
                var taskDtos = _mapper.Map<List<TaskDto>>(paginatedTasks.Data);

                return Ok(new
                {
                    totalCount = paginatedTasks.TotalCount,
                    pageSize = paginatedTasks.PageSize,
                    currentPage = paginatedTasks.CurrentPage,
                    totalPages = paginatedTasks.TotalPages,
                    hasPreviousPage = paginatedTasks.HasPreviousPage,
                    hasNextPage = paginatedTasks.HasNextPage,
                    data = taskDtos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                    return NotFound(new { message = "Task not found." });

                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTask([FromBody] RequestTaskDto taskdto)
        {
            if (taskdto == null)
                return BadRequest("Task data is required.");

            if (string.IsNullOrWhiteSpace(taskdto.Title))
                return BadRequest("Task title is required.");

            try
            {
                var task = _mapper.Map<TaskItem>(taskdto);
                task.Id = Guid.NewGuid();
                await _taskService.AddTaskAsync(task);
                return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] RequestTaskDto taskdto)
        {
            try
            {
                var existingTask = await _taskService.GetTaskByIdAsync(id);
                if (existingTask == null)
                    return NotFound(new { message = "Task not found." });

                var task = _mapper.Map<TaskItem>(taskdto);
                task.Id = id;
                await _taskService.UpdateTaskAsync(id, task);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                    return NotFound(new { message = "Task not found." });

                await _taskService.DeleteTaskAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}

