using Microsoft.EntityFrameworkCore;
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
    public class TaskRepository : BaseRepository<Core.Entities.TaskItem>, ITaskRepository
    {
        public TaskRepository(TodoAppDbContext context) : base(context) { }

        public async Task<PaginatedList<Core.Entities.TaskItem>> GetPaginatedTasksAsync(int page, int pageSize, string? search, TaskPriority? priority, bool? isCompleted, string? sortBy = "asc")
        {
            var query = _context.Tasks
                .Include(t => t.Dependencies)
                .ThenInclude(d => d.DependsOnTask)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t =>
                    (t.Title != null && EF.Functions.Like(t.Title, $"%{search}%")) ||
                    (t.Description != null && EF.Functions.Like(t.Description, $"%{search}%")));
            }

            if (priority.HasValue)
            {
                query = query.Where(t => t.Priority == priority);
            }

            if (isCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == isCompleted);
            }

            query = sortBy?.ToLower() == "desc"
                ? query.OrderByDescending(t => t.DueDate)
                : query.OrderBy(t => t.DueDate);

            return await PaginatedList<Core.Entities.TaskItem>.CreateAsync(query, page, pageSize);
        }
    }
}
