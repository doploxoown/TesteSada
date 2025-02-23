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
        [Required(ErrorMessage = "O ID da tarefa é obrigatório.")]
        public Guid Id { get; set; }

        /// <summary>
        /// Título da tarefa.
        /// </summary>
        [Required(ErrorMessage = "O título é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada da tarefa.
        /// </summary>
        [MaxLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string? Description { get; set; }

        /// <summary>
        /// Data de vencimento da tarefa.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Status atual da tarefa.
        /// </summary>
        [Required(ErrorMessage = "O status é obrigatório.")]
        public ETaskStatus Status { get; set; }
    }
}
