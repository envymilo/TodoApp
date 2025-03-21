using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Entities;
using TodoApp.Core.Enum;

namespace TodoApp.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static readonly Guid task1 = Guid.NewGuid();
        public static readonly Guid task2 = Guid.NewGuid();
        public static readonly Guid task3 = Guid.NewGuid();
        public static readonly Guid task4 = Guid.NewGuid();
        public static readonly Guid task5 = Guid.NewGuid();

        public static readonly Guid taskDependency1 = Guid.NewGuid();
        public static readonly Guid taskDependency2 = Guid.NewGuid();
        public static readonly Guid taskDependency3 = Guid.NewGuid();
        public static readonly Guid taskDependency4 = Guid.NewGuid();
        public static readonly DateTime now = DateTime.Today;

        public static async Task DataSeeding(TodoAppDbContext context)
        {
            if (!context.Tasks.Any()) // Prevent duplicate seeding
            {
                var tasks = new List<TaskItem>
        {
            new TaskItem { Id = task1, Title = "Learn ReactJS", DueDate = DateTime.Today.AddDays(5), Priority = TaskPriority.Medium, IsCompleted = false },
            new TaskItem { Id = task2, Title = "Learn JavaScript", DueDate = DateTime.Today.AddDays(1), Priority = TaskPriority.High, IsCompleted = true },
            new TaskItem { Id = task3, Title = "Learn NextJS", DueDate = DateTime.Today, Priority = TaskPriority.Medium, IsCompleted = false },
            new TaskItem { Id = task4, Title = "Learn ThreeJS", DueDate = DateTime.Today.AddDays(9), Priority = TaskPriority.Low, IsCompleted = false },
            new TaskItem { Id = task5, Title = "Create a Portfolio Website", DueDate = DateTime.Today.AddDays(7), Priority = TaskPriority.High, IsCompleted = false }
        };

                await context.Tasks.AddRangeAsync(tasks);
                await context.SaveChangesAsync();
            }

            if (!context.TaskDependencies.Any()) // Prevent duplicate seeding
            {
                var taskDependencies = new List<TaskDependency>
        {
                new TaskDependency { Id = taskDependency1, TaskId = task3, DependsOnTaskId = task1 }, // NextJS after ReactJS
                new TaskDependency { Id = taskDependency2, TaskId = task4, DependsOnTaskId = task2 }, // ThreeJS after JavaScript
                new TaskDependency { Id = taskDependency3, TaskId = task5, DependsOnTaskId = task4 }, // Create portfolio after ThreeJS
                new TaskDependency { Id = taskDependency4, TaskId = task5, DependsOnTaskId = task3 }  // Create portfolio after NextJS
        };

                await context.TaskDependencies.AddRangeAsync(taskDependencies);
                await context.SaveChangesAsync();
            }

        }
    }
}
