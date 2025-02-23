using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs
{
    /// <summary>
    /// DTO para criação de uma nova tarefa.
    /// </summary>
    public class CreateTaskDto
    {
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
        /// Status inicial da tarefa.
        /// </summary>
        [Required(ErrorMessage = "O status é obrigatório.")]
        public ETaskStatus Status { get; set; }
    }
}
