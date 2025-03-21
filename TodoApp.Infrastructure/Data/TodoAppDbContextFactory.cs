using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Entities;
using Microsoft.Extensions.Configuration;

namespace TodoApp.Infrastructure.Data
{
    public class TodoAppDbContextFactory : IDesignTimeDbContextFactory<TodoAppDbContext>
    {
        public TodoAppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var builder = new DbContextOptionsBuilder<TodoAppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new TodoAppDbContext(builder.Options);
        }
    }
}
