using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs
{
    /// <summary>
    /// DTO para representar uma tarefa na API.
    /// </summary>
    public class TaskDto
    {
        /// <summary>
        /// Identificador único da tarefa.
        /// </summary>
        /// <example>e1d3f95b-7c6d-4f2c-a93b-708d635b57a3</example>
        public Guid Id { get; set; }

        /// <summary>
        /// Título da tarefa.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada da tarefa.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Data de vencimento da tarefa.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Status atual da tarefa.
        /// </summary>
        public ETaskStatus Status { get; set; }
    }
}
