using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Entities;
using TodoApp.Infrastructure.Repositories.Generic;
using TodoApp.Infrastructure.Repositories.TaskDependency;
using TodoApp.Infrastructure.Repositories.TaskItem;
using TodoApp.Infrastructure.Services.TaskDependency;
using TodoApp.Infrastructure.Services.TaskItem;

namespace TodoApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TodoAppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskService, TaskService>();

            services.AddScoped<ITaskDependencyRepository, TaskDependencyRepository>();
            services.AddScoped<ITaskDependencyService, TaskDependencyService>();

            return services;
        }
    }
}
