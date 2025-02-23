using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Application.Mappers;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Interfaces.Repositories;

namespace TaskManagement.Application.Services
{
    /// <summary>
    /// Serviço para gerenciar tarefas.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        /// <summary>
        /// Construtor do serviço, recebe a interface do repositório TaskRepository.
        /// </summary>
        /// <param name="context">Interface do TaskRepository.</param>
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        /// <summary>
        /// Obtém todas as tarefas.
        /// </summary>
        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return tasks.Select(t => t.ToDto());
        }

        /// <summary>
        /// Obtém as tarefas filtradas por status e/ou data de vencimento.
        /// </summary>
        public async Task<IEnumerable<TaskDto>> GetFilteredTasksAsync(ETaskStatus? status, DateTime? dueDate)
        {
            var tasks = await _taskRepository.GetFilteredTasksAsync(status, dueDate);
            return tasks.Select(t => t.ToDto());
        }

        /// <summary>
        /// Obtém uma tarefa pelo ID.
        /// </summary>
        public async Task<TaskDto?> GetTaskByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task?.ToDto();
        }

        /// <summary>
        /// Adiciona uma nova tarefa.
        /// </summary>
        public async Task<TaskDto> AddTaskAsync(CreateTaskDto createTaskDto)
        {
            var task = createTaskDto.ToEntity();
            await _taskRepository.AddAsync(task);
            return task.ToDto();
        }

        /// <summary>
        /// Atualiza uma tarefa existente.
        /// </summary>
        public async Task UpdateTaskAsync(TaskDto taskDto)
        {
            var task = taskDto.ToEntity();
            await _taskRepository.UpdateAsync(task);
        }

        /// <summary>
        /// Exclui uma tarefa pelo ID.
        /// </summary>
        public async Task DeleteTaskAsync(Guid id)
        {
            await _taskRepository.DeleteAsync(id);
        }
    }
}
