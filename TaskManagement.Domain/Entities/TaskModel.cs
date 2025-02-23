using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
    /// <summary>
    /// Representa uma tarefa no sistema de gerenciamento de tarefas.
    /// </summary>
    public class TaskModel
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
        /// <example>Finalizar o relatório trimestral</example>
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        public required string Title { get; set; }

        /// <summary>
        /// Descrição da tarefa.
        /// </summary>
        /// <example>Revisar e finalizar o relatório financeiro do último trimestre antes da reunião com a diretoria.</example>
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string? Description { get; set; }

        /// <summary>
        /// Data e hora de vencimento da tarefa.
        /// </summary>
        /// <example>2025-03-15T14:00:00Z</example>
        [Required(ErrorMessage = "A data de vencimento é obrigatória.")]
        [DataType(DataType.DateTime, ErrorMessage = "Formato de data inválido.")]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Status atual da tarefa.
        /// </summary>
        /// <example>Pending</example>
        [Required(ErrorMessage = "O status é obrigatório.")]
        [EnumDataType(typeof(ETaskStatus), ErrorMessage = "Status inválido.")]
        public ETaskStatus Status { get; set; }
    }
}
