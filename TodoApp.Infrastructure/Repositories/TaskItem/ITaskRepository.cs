using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Entities;
using TodoApp.Core.Enum;
using TodoApp.Infrastructure.Repositories.Generic;

namespace TodoApp.Infrastructure.Repositories.TaskItem
{
    public interface ITaskRepository : IBaseRepository<Core.Entities.TaskItem>
    {
        Task<PaginatedList<Core.Entities.TaskItem>> GetPaginatedTasksAsync(int page, int pageSize, string? search, TaskPriority? priority, bool? isCompleted, string? sortBy = "asc");
    }
}
