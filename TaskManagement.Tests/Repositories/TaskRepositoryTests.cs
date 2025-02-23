using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Tests.Repositories
{
    public class TaskRepositoryTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Task()
        {
            var context = GetDbContext();
            var repository = new TaskRepository(context);

            var task = new TaskModel
            {
                Id = Guid.NewGuid(),
                Title = "New Task",
                Description = "Description Task",
                DueDate = DateTime.UtcNow.AddDays(3),
                Status = ETaskStatus.Pending
            };

            await repository.AddAsync(task);
            var result = await repository.GetByIdAsync(task.Id);

            Assert.NotNull(result);
            Assert.Equal("New Task", result.Title);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Tasks()
        {
            var context = GetDbContext();
            var repository = new TaskRepository(context);

            await repository.AddAsync(new TaskModel { Id = Guid.NewGuid(), Title = "Task 1", Status = ETaskStatus.Pending });
            await repository.AddAsync(new TaskModel { Id = Guid.NewGuid(), Title = "Task 2", Status = ETaskStatus.Completed });

            var tasks = await repository.GetAllAsync();

            Assert.Equal(2, tasks.Count());
        }

        [Fact]
        public async Task GetFilteredTasksAsync_ShouldReturnFilteredTasks()
        {
            var context = GetDbContext();
            var repository = new TaskRepository(context);

            var task1 = new TaskModel { Id = Guid.NewGuid(), Title = "Task 1", Status = ETaskStatus.Pending, DueDate = DateTime.Today };
            var task2 = new TaskModel { Id = Guid.NewGuid(), Title = "Task 2", Status = ETaskStatus.Completed, DueDate = DateTime.Today };
            context.Tasks.AddRange(task1, task2);
            await context.SaveChangesAsync();

            var result = await repository.GetFilteredTasksAsync(ETaskStatus.Pending, null);

            Assert.Single(result);
            Assert.Equal(task1.Id, result.First().Id);
        }


        [Fact]
        public async Task GetByIdAsync_Should_Return_Correct_Task()
        {
            var context = GetDbContext();
            var repository = new TaskRepository(context);

            var task = new TaskModel { Id = Guid.NewGuid(), Title = "Task Teste", Status = ETaskStatus.Pending };
            await repository.AddAsync(task);

            var result = await repository.GetByIdAsync(task.Id);

            Assert.NotNull(result);
            Assert.Equal(task.Title, result.Title);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Task()
        {
            var context = GetDbContext();
            var repository = new TaskRepository(context);

            var task = new TaskModel { Id = Guid.NewGuid(), Title = "Task to Delete", Status = ETaskStatus.Pending };
            await repository.AddAsync(task);

            await repository.DeleteAsync(task.Id);
            var result = await repository.GetByIdAsync(task.Id);

            Assert.Null(result);
        }
    }
}