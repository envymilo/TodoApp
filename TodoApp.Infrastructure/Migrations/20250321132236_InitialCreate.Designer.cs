﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoApp.Core.Entities;

#nullable disable

namespace TodoApp.Infrastructure.Migrations
{
    [DbContext(typeof(TodoAppDbContext))]
    [Migration("20250321132236_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TodoApp.Core.Entities.TaskDependency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DependsOnTaskId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TaskItemId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DependsOnTaskId");

                    b.HasIndex("TaskItemId");

                    b.HasIndex("TaskId", "DependsOnTaskId")
                        .IsUnique();

                    b.ToTable("TaskDependencies");
                });

            modelBuilder.Entity("TodoApp.Core.Entities.TaskItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TodoApp.Core.Entities.TaskDependency", b =>
                {
                    b.HasOne("TodoApp.Core.Entities.TaskItem", "DependsOnTask")
                        .WithMany()
                        .HasForeignKey("DependsOnTaskId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TodoApp.Core.Entities.TaskItem", "DependentTask")
                        .WithMany("Dependencies")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TodoApp.Core.Entities.TaskItem", null)
                        .WithMany("DependentOn")
                        .HasForeignKey("TaskItemId");

                    b.Navigation("DependentTask");

                    b.Navigation("DependsOnTask");
                });

            modelBuilder.Entity("TodoApp.Core.Entities.TaskItem", b =>
                {
                    b.Navigation("Dependencies");

                    b.Navigation("DependentOn");
                });
#pragma warning restore 612, 618
        }
    }
}
