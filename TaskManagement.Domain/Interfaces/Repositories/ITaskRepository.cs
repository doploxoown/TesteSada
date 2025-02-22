using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskModel>> GetAllAsync();
        Task<IEnumerable<TaskModel>> GetFilteredTasksAsync(ETaskStatus? status, DateTime? dueDate);
        Task<TaskModel?> GetByIdAsync(Guid id);
        Task AddAsync(TaskModel task);
        Task UpdateAsync(TaskModel task);
        Task DeleteAsync(Guid id);
    }
}
