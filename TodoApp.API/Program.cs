using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using TodoApp.Core.Entities;
using TodoApp.Core.Mappings;
using TodoApp.Infrastructure;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories.Generic;
using TodoApp.Infrastructure.Repositories.TaskDependency;
using TodoApp.Infrastructure.Repositories.TaskItem;
using TodoApp.Infrastructure.Services.TaskDependency;
using TodoApp.Infrastructure.Services.TaskItem;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TodoAppDbContext>(options =>
    options.UseSqlServer(connectionString));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API v1");
        c.RoutePrefix = string.Empty; // This makes Swagger open at root (http://localhost:5000/)
    });
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthorization();

async void ApplyMigration()
{
    using var scope = app.Services.CreateScope();
    var _db = scope.ServiceProvider.GetRequiredService<TodoAppDbContext>();

    if (_db.Database.GetPendingMigrations().Any())
    {
        _db.Database.Migrate();
    }
    await DbInitializer.DataSeeding(_db);
}
ApplyMigration();

app.Run();
