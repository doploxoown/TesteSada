using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Common;
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
        [ProducesResponseType(typeof(IEnumerable<TaskDto>), 200)]
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
        [ProducesResponseType(typeof(TaskDto), 200)]
        [ProducesResponseType(404)]
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
        [ProducesResponseType(typeof(IEnumerable<TaskDto>), 200)]
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
        /// <response code="400">Dados inválidos.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreateTaskDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto createTaskDto)
        {
            if (!ModelState.IsValid)
            {
                var actionContext = new ActionContext
                {
                    HttpContext = HttpContext,
                    RouteData = RouteData
                };

                var validationProblem = new CustomBadRequestDetails(actionContext);
                return new BadRequestObjectResult(validationProblem);
            }

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
        /// <response code="400">IDs incompatíveis ou dados inválidos.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskDto updateTaskDto)
        {
            if (!ModelState.IsValid)
            {
                var actionContext = new ActionContext
                {
                    HttpContext = HttpContext,
                    RouteData = RouteData
                };

                var validationProblem = new CustomBadRequestDetails(actionContext);
                return new BadRequestObjectResult(validationProblem);
            }

            if (id != updateTaskDto.Id)
            {
                var errorDetails = new ErrorDetails
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "O ID da URL não corresponde ao ID do corpo da requisição."
                };
                return BadRequest(errorDetails);
            }

            await _taskService.UpdateTaskAsync(updateTaskDto);
            return NoContent();
        }

        /// <summary>
        /// Remove uma tarefa pelo ID.
        /// </summary>
        /// <param name="id">ID da tarefa a ser removida.</param>
        /// <returns>Código 204 se a remoção for bem-sucedida.</returns>
        /// <response code="204">Tarefa excluída com sucesso.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
    }
}
