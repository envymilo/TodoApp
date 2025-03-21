using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Entities;
using TodoApp.Core.Enum;
using TodoApp.Infrastructure.Repositories.TaskItem;
using TodoApp.Infrastructure.Services.TaskItem;

namespace TodoApp.Infrastructure.Services.TaskItem
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Core.Entities.TaskItem>> GetAllTasksAsync(int page, int pageSize)
        {
            return await _unitOfWork._taskRepo.GetPagedAsync(page, pageSize);
        }

        public async Task<PaginatedList<Core.Entities.TaskItem>> GetPaginatedTasksAsync(int page, int pageSize, string? search, TaskPriority? priority, bool? isCompleted)
        {
            return await _unitOfWork._taskRepo.GetPaginatedTasksAsync(page, pageSize, search, priority, isCompleted);
        }

        public async Task<Core.Entities.TaskItem?> GetTaskByIdAsync(Guid id)
        {
            return await _unitOfWork._taskRepo.GetByIdAsync(id);
        }

        public async Task<Core.Entities.TaskItem?> FindDuplicateTask(string title)
        {
            var result = await _unitOfWork._taskRepo.FindAsync(t => t.Title == title);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Core.Entities.TaskItem>> SearchTasksByTitleOrDescription(string title)
        {
            return await _unitOfWork._taskRepo.FindAsync(t =>
                   (t.Title != null && EF.Functions.Like(t.Title, $"%{title}%")) ||
                   (t.Description != null && EF.Functions.Like(t.Description, $"%{title}%")));
        }


        public async Task AddTaskAsync(Core.Entities.TaskItem task)
        {
            await _unitOfWork._taskRepo.AddAsync(task);
        }

        public async Task UpdateTaskAsync(Guid id, Core.Entities.TaskItem task)
        {
            await _unitOfWork._taskRepo.UpdateAsync(id, task);
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            await _unitOfWork._taskRepo.DeleteAsync(id);
        }
    }
}
