using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Entities;

namespace TodoApp.Infrastructure.Services.TaskDependency
{
    public interface ITaskDependencyService
    {
        Task<IEnumerable<Core.Entities.TaskDependency>> GetAllTaskDependenciesAsync(int page, int pageSize);
        Task<Core.Entities.TaskDependency?> GetTaskDependencyByIdAsync(int id);
        Task<(bool Success, string Message)> AddTaskDependencyAsync(Guid taskId, Guid dependsOnTaskId);
        Task UpdateTaskDependencyAsync(Core.Entities.TaskDependency task);
        Task<(bool Success, string Message)> RemoveDependencyAsync(Guid taskId, Guid dependsOnTaskId);
    }
}
