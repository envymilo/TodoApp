using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.DTOs;
using TodoApp.Core.Entities;
using TodoApp.Core.Enum;

namespace TodoApp.Infrastructure.Services.TaskItem
{
    public interface ITaskService
    {
        Task<IEnumerable<Core.Entities.TaskItem>> GetAllTasksAsync(int page, int pageSize);
        Task<PaginatedList<Core.Entities.TaskItem>> GetPaginatedTasksAsync(int page, int pageSize, string? search, TaskPriority? priority, bool? isCompleted);
        Task<Core.Entities.TaskItem?> GetTaskByIdAsync(Guid id);
        Task AddTaskAsync(Core.Entities.TaskItem task);
        Task UpdateTaskAsync(Guid id, Core.Entities.TaskItem task);
        Task DeleteTaskAsync(Guid id);
    }
}
