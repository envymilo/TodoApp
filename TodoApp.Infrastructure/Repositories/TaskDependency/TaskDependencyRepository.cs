using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Entities;
using TodoApp.Infrastructure.Repositories.Generic;

namespace TodoApp.Infrastructure.Repositories.TaskDependency
{
    public class TaskDependencyRepository : BaseRepository<Core.Entities.TaskDependency>, ITaskDependencyRepository
    {
        public TaskDependencyRepository(TodoAppDbContext context) : base(context)
        {
        }
    }
}
