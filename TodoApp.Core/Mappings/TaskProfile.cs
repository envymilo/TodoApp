using AutoMapper;
using TodoApp.Core.DTOs;
using TodoApp.Core.Entities;

namespace TodoApp.Core.Mappings
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<RequestTaskDto, TaskItem>();

            CreateMap<TaskItem, TaskDto>()                
                .ForMember(dest => dest.DependsOnTasks, opt => opt.MapFrom((src, dest, destMember, context) =>
                    GetAllDependsOnTasks(src, context)
                ));

            CreateMap<TaskDependency, DependentTaskDto>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.DependsOnTaskId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.DependsOnTask.Title));
        }

        private List<DependentTaskDto> GetAllDependsOnTasks(Core.Entities.TaskItem task, ResolutionContext context)
        {
            var dependsOnTasks = task.Dependencies
                .Select(d => new DependentTaskDto
                {
                    TaskId = d.DependsOnTaskId,
                    Title = d.DependsOnTask.Title
                })
                .ToList();

            foreach (var dependsOnTask in dependsOnTasks)
            {
                var parentTask = task.Dependencies
                    .FirstOrDefault(d => d.DependsOnTaskId == dependsOnTask.TaskId)?.DependsOnTask;

                if (parentTask != null)
                {
                    dependsOnTask.SubTasks = GetAllDependsOnTasks(parentTask, context);
                }
            }

            return dependsOnTasks;
        }
    }
}
