using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Entities;
using TodoApp.Infrastructure.Repositories.TaskDependency;
using TodoApp.Infrastructure.Repositories.TaskItem;

namespace TodoApp.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        ITaskRepository _taskRepo { get; }
        ITaskDependencyRepository _taskDependencyRepo { get; }
        Task<int> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly TodoAppDbContext _context;

        public UnitOfWork(TodoAppDbContext context)
        {
            _context = context;
            _taskRepo = new TaskRepository(_context);
            _taskDependencyRepo = new TaskDependencyRepository(_context);
        }

        public ITaskRepository _taskRepo { get; }

        public ITaskDependencyRepository _taskDependencyRepo { get; }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
