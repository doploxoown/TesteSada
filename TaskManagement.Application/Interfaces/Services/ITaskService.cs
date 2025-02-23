using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Interfaces.Services
{
    /// <summary>
    /// Interface do serviço de tarefas.
    /// </summary>
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();
        Task<IEnumerable<TaskDto>> GetFilteredTasksAsync(ETaskStatus? status, DateTime? dueDate);
        Task<TaskDto?> GetTaskByIdAsync(Guid id);
        Task<TaskDto> AddTaskAsync(CreateTaskDto createTaskDto);
        Task UpdateTaskAsync(TaskDto taskDto);
        Task DeleteTaskAsync(Guid id);
    }
}
