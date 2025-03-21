using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Entities;

namespace TodoApp.Infrastructure.Services.TaskDependency
{
    public class TaskDependencyService : ITaskDependencyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskDependencyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<(bool Success, string Message)> AddTaskDependencyAsync(Guid taskId, Guid dependsOnTaskId)
        {
            var task = await _unitOfWork._taskRepo.GetByIdAsync(taskId);
            var dependsOnTask = await _unitOfWork._taskRepo.GetByIdAsync(dependsOnTaskId);

            if (task == null || dependsOnTask == null)
                return (false, "Task not found.");

            var existingDependency = await _unitOfWork._taskDependencyRepo.FindAsync(d =>
                d.TaskId == taskId && d.DependsOnTaskId == dependsOnTaskId);
            if (existingDependency.Count() != 0)
                return (false, "Dependency already exists.");

            if (await HasCircularDependency(taskId, dependsOnTaskId))
                return (false, "Adding this dependency would create a circular dependency.");

            var dependency = new Core.Entities.TaskDependency
            {
                Id = Guid.NewGuid(),
                TaskId = taskId,
                DependsOnTaskId = dependsOnTaskId
            };

            await _unitOfWork._taskDependencyRepo.AddAsync(dependency);
            return (true, "Dependency added successfully.");
        }

        private async Task<bool> HasCircularDependency(Guid taskId, Guid dependsOnTaskId)
        {
            var visited = new HashSet<Guid>();
            return await CheckForCycle(dependsOnTaskId, taskId, visited);
        }

        private async Task<bool> CheckForCycle(Guid currentTaskId, Guid targetTaskId, HashSet<Guid> visited)
        {
            if (currentTaskId == targetTaskId)
                return true;

            if (visited.Contains(currentTaskId))
                return false;

            visited.Add(currentTaskId);

            var dependencies = await _unitOfWork._taskDependencyRepo.FindAsync(d =>
                d.TaskId == currentTaskId);
            if (dependencies != null) { 
            foreach (var dependency in dependencies)
            {
                if (await CheckForCycle(dependency.DependsOnTaskId, targetTaskId, visited))
                    return true;
            }
            }

            return false;
        }

        public async Task<(bool Success, string Message)> RemoveDependencyAsync(Guid taskId, Guid dependsOnTaskId)
        {
            var dependency = await _unitOfWork._taskDependencyRepo.FindAsync(d =>
                d.TaskId == taskId && d.DependsOnTaskId == dependsOnTaskId);

            if (dependency == null)
                return (false, "Dependency not found.");

            await _unitOfWork._taskDependencyRepo.DeleteAsync(dependency.FirstOrDefault().Id);
            return (true, "Dependency removed successfully.");
        }

        public Task<IEnumerable<Core.Entities.TaskDependency>> GetAllTaskDependenciesAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Entities.TaskDependency?> GetTaskDependencyByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTaskDependencyAsync(Core.Entities.TaskDependency task)
        {
            throw new NotImplementedException();
        }
    }
}
