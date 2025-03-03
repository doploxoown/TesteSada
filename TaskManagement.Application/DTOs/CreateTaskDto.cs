﻿using FluentValidation;
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
        /// Status inicial da tarefa.
        /// </summary>
        public ETaskStatus Status { get; set; }
    }

    public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("O título é obrigatório.")
                .MaximumLength(100).WithMessage("O título deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("O status é obrigatório.");

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.Now).WithMessage("A data de vencimento deve ser no futuro.");
        }
    }
}
