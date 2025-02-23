using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Mappers
{
    /// <summary>
    /// Classe responsável por mapear TaskModel para TaskDto e vice-versa.
    /// </summary>
    public static class TaskMapper
    {
        /// <summary>
        /// Converte um TaskModel para TaskDto.
        /// </summary>
        public static TaskDto ToDto(this TaskModel task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status
            };
        }

        /// <summary>
        /// Converte um TaskDto para TaskModel.
        /// </summary>
        public static TaskModel ToEntity(this TaskDto taskDto)
        {
            return new TaskModel
            {
                Id = taskDto.Id,
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate.GetValueOrDefault(),
                Status = taskDto.Status
            };
        }

        /// <summary>
        /// Converte um CreateTaskDto para TaskModel.
        /// </summary>
        public static TaskModel ToEntity(this CreateTaskDto createTaskDto)
        {
            return new TaskModel
            {
                Id = Guid.NewGuid(),
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                DueDate = createTaskDto.DueDate.GetValueOrDefault(),
                Status = createTaskDto.Status
            };
        }

        /// <summary>
        /// Converte um UpdateTaskDto para TaskModel.
        /// </summary>
        public static TaskModel ToEntity(this UpdateTaskDto updateTaskDto)
        {
            return new TaskModel
            {
                Id = updateTaskDto.Id,
                Title = updateTaskDto.Title,
                Description = updateTaskDto.Description,
                DueDate = updateTaskDto.DueDate.GetValueOrDefault(),
                Status = updateTaskDto.Status
            };
        }

        /// <summary>
        /// Converte uma lista de TaskModel para uma lista de TaskDto.
        /// </summary>
        public static IEnumerable<TaskDto> ToDtoList(this IEnumerable<TaskModel> tasks)
        {
            return tasks.Select(task => task.ToDto()).ToList();
        }
    }
}