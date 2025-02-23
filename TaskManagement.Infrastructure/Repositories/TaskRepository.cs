using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Interfaces.Repositories;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Repositório responsável pela persistência dos dados da entidade TaskModel.
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor do repositório, recebe o contexto do banco de dados.
        /// </summary>
        /// <param name="context">Instância do ApplicationDbContext.</param>
        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todas as tarefas do banco de dados.
        /// </summary>
        /// <returns>Lista de tarefas cadastradas.</returns>
        public async Task<IEnumerable<TaskModel>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        /// <summary>
        /// Obtém uma tarefa específica pelo seu ID.
        /// </summary>
        /// <param name="id">ID da tarefa a ser buscada.</param>
        /// <returns>Retorna a tarefa encontrada ou null caso não exista.</returns>
        public async Task<TaskModel?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        /// <summary>
        /// Obtém tarefas filtradas pelo status e/ou data de vencimento.
        /// </summary>
        /// <param name="status">Status da tarefa (Opcional).</param>
        /// <param name="dueDate">Data de vencimento da tarefa (Opcional).</param>
        /// <returns>Lista de tarefas filtradas.</returns>
        public async Task<IEnumerable<TaskModel>> GetFilteredTasksAsync(ETaskStatus? status, DateTime? dueDate)
        {
            var query = _context.Tasks.AsQueryable();

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            if (dueDate.HasValue)
                query = query.Where(t => t.DueDate.Date == dueDate.Value.Date);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Adiciona uma nova tarefa ao banco de dados.
        /// </summary>
        /// <param name="task">Objeto da tarefa a ser adicionada.</param>
        public async Task AddAsync(TaskModel task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza os dados de uma tarefa existente.
        /// </summary>
        /// <param name="task">Objeto da tarefa com os novos dados.</param>
        public async Task UpdateAsync(TaskModel task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove uma tarefa do banco de dados pelo seu ID.
        /// </summary>
        /// <param name="id">ID da tarefa a ser removida.</param>
        public async Task DeleteAsync(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
