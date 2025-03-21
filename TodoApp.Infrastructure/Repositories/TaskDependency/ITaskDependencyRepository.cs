using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Entities;
using TodoApp.Infrastructure.Repositories.Generic;

namespace TodoApp.Infrastructure.Repositories.TaskDependency
{
    public interface ITaskDependencyRepository : IBaseRepository<Core.Entities.TaskDependency>
    {
    }
}
