using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Enums;

namespace TaskManagementAPI.Controllers
{
    /// <summary>
    /// Controlador responsável pela gestão de tarefas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        /// <summary>
        /// Construtor da TaskController.
        /// </summary>
        /// <param name="taskService">Serviço de tarefas injetado.</param>
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Obtém todas as tarefas.
        /// </summary>
        /// <returns>Lista de tarefas.</returns>
        /// <response code="200">Retorna a lista de tarefas.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        /// <summary>
        /// Obtém uma tarefa pelo ID.
        /// </summary>
        /// <param name="id">Identificador único da tarefa.</param>
        /// <returns>Objeto da tarefa correspondente.</returns>
        /// <response code="200">Retorna a tarefa encontrada.</response>
        /// <response code="404">Tarefa não encontrada.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            return task is not null ? Ok(task) : NotFound();
        }

        /// <summary>
        /// Filtra as tarefas pelo status e/ou data de vencimento.
        /// </summary>
        /// <param name="status">Status opcional da tarefa.</param>
        /// <param name="dueDate">Data de vencimento opcional da tarefa.</param>
        /// <returns>Lista de tarefas filtradas.</returns>
        /// <response code="200">Retorna as tarefas filtradas.</response>
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredTasks([FromQuery] ETaskStatus? status, [FromQuery] DateTime? dueDate)
        {
            var tasks = await _taskService.GetFilteredTasksAsync(status, dueDate);
            return Ok(tasks);
        }

        /// <summary>
        /// Cria uma nova tarefa.
        /// </summary>
        /// <param name="createTaskDto">Objeto da tarefa a ser criada.</param>
        /// <returns>A tarefa criada.</returns>
        /// <response code="201">Tarefa criada com sucesso.</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto createTaskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var taskAdded = await _taskService.AddTaskAsync(createTaskDto);
            return CreatedAtAction(nameof(GetById), new { id = taskAdded.Id }, createTaskDto);
        }

        /// <summary>
        /// Atualiza uma tarefa existente.
        /// </summary>
        /// <param name="id">ID da tarefa a ser atualizada.</param>
        /// <param name="task">Dados atualizados da tarefa.</param>
        /// <returns>Código 204 se a atualização for bem-sucedida.</returns>
        /// <response code="204">Tarefa atualizada com sucesso.</response>
        /// <response code="400">IDs incompatíveis.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TaskDto taskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != taskDto.Id)
                return BadRequest("O ID da URL não corresponde ao ID do corpo da requisição.");

            await _taskService.UpdateTaskAsync(taskDto);
            return NoContent();
        }

        /// <summary>
        /// Remove uma tarefa pelo ID.
        /// </summary>
        /// <param name="id">ID da tarefa a ser removida.</param>
        /// <returns>Código 204 se a remoção for bem-sucedida.</returns>
        /// <response code="204">Tarefa excluída com sucesso.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
    }
}