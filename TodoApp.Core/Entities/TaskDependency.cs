using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TodoApp.Core.Entities
{
    public class TaskDependency
    {
        public Guid Id { get; set; }

        public Guid TaskId { get; set; }
        public Guid DependsOnTaskId { get; set; }
        public TaskItem Task { get; set; } = null!;
        public TaskItem DependsOnTask { get; set; } = null!;
    }
}
