using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Tests.Repositories
{
    public class TaskRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly TaskRepository _repository;

        public TaskRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TaskDatabaseTest")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new TaskRepository(_context);

            _context.Tasks.AddRange(
                new TaskModel { Id = Guid.NewGuid(), Title = "Tarefa 1", Status = ETaskStatus.Pending, DueDate = DateTime.Today },
                new TaskModel { Id = Guid.NewGuid(), Title = "Tarefa 2", Status = ETaskStatus.Completed, DueDate = DateTime.Today.AddDays(1) }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task Teste_Filtrar_Tarefas_Por_Status()
        {
            var result = await _repository.GetFilteredTasksAsync(ETaskStatus.Pending, null);
            Assert.Single(result);
            Assert.Equal("Tarefa 1", result.First().Title);
        }

        [Fact]
        public async Task Teste_Filtrar_Tarefas_Por_DataVencimento()
        {
            var result = await _repository.GetFilteredTasksAsync(null, DateTime.Today);
            Assert.Single(result);
            Assert.Equal("Tarefa 1", result.First().Title);
        }

        [Fact]
        public async Task Teste_Filtrar_Tarefas_Por_Status_E_Data()
        {
            var result = await _repository.GetFilteredTasksAsync(ETaskStatus.Completed, DateTime.Today.AddDays(1));
            Assert.Single(result);
            Assert.Equal("Tarefa 2", result.First().Title);
        }

        [Fact]
        public async Task Teste_Adicionar_Tarefa()
        {
            var newTask = new TaskModel
            {
                Id = Guid.NewGuid(),
                Title = "Nova Tarefa",
                Status = ETaskStatus.Pending,
                DueDate = DateTime.Today
            };

            await _repository.AddAsync(newTask);
            var tasks = await _repository.GetAllAsync();

            Assert.Equal(3, tasks.Count());
            Assert.Contains(tasks, t => t.Title == "Nova Tarefa");
        }

        [Fact]
        public async Task Teste_Remover_Tarefa()
        {
            var existingTask = _context.Tasks.First();
            await _repository.DeleteAsync(existingTask.Id);

            var tasks = await _repository.GetAllAsync();
            Assert.DoesNotContain(tasks, t => t.Id == existingTask.Id);
        }
    }
}