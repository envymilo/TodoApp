using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Core.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public bool IsCompleted { get; set; } = false;

        public List<TaskDependency> Dependencies { get; set; } = new();
    }
}
