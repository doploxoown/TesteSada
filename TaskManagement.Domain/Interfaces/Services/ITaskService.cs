using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Interfaces.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskModel>> GetAllTasksAsync();
        Task<IEnumerable<TaskModel>> GetFilteredTasksAsync(ETaskStatus? status, DateTime? dueDate);
        Task<TaskModel?> GetTaskByIdAsync(Guid id);
        Task AddTaskAsync(TaskModel task);
        Task UpdateTaskAsync(TaskModel task);
        Task DeleteTaskAsync(Guid id);
    }
}
