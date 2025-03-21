using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Core.Entities
{
    public class TodoAppDbContext : DbContext
    {
        public TodoAppDbContext(DbContextOptions<TodoAppDbContext> options) : base(options) { }

        #region DbSet

        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskDependency> TaskDependencies { get; set; }

        #endregion DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskDependency>()
                .HasIndex(td => new { td.TaskId, td.DependsOnTaskId })
                .IsUnique();

            modelBuilder.Entity<TaskDependency>()
                .HasOne(td => td.Task)
                .WithMany(t => t.Dependencies)
                .HasForeignKey(td => td.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskDependency>()
                .HasOne(td => td.DependsOnTask)
                .WithMany()
                .HasForeignKey(td => td.DependsOnTaskId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("Relational:CheckConstraints",
               "ALTER TABLE TaskDependencies ADD CONSTRAINT CHK_TaskDependency_SelfReference CHECK (TaskId <> DependsOnTaskId)");

        }
    }
}
