using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Core.DTOs
{
    public class DependentTaskDto
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<DependentTaskDto> SubTasks { get; set; } = new();
    }
}
