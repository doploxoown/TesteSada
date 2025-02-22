using Moq;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces.Repositories;

namespace TaskManagement.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllTasksAsync_ShouldReturnTaskList()
        {
            // Arrange
            var tasks = new List<TaskModel>
            {
                new() { Id = Guid.NewGuid(), Title = "Task 1" },
                new() { Id = Guid.NewGuid(), Title = "Task 2" }
            };

            _taskRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetAllTasksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
        {
            // Arrange
            var task = new TaskModel { Id = Guid.NewGuid(), Title = "Test Task" };

            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(task.Id)).ReturnsAsync(task);

            // Act
            var result = await _taskService.GetTaskByIdAsync(task.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(task.Title, result.Title);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            // Arrange
            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((TaskModel?)null);

            // Act
            var result = await _taskService.GetTaskByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddTaskAsync_ShouldCallRepositoryMethod()
        {
            // Arrange
            var task = new TaskModel { Title = "New Task" };

            _taskRepositoryMock.Setup(repo => repo.AddAsync(task)).Returns(Task.CompletedTask);

            // Act
            await _taskService.AddTaskAsync(task);

            // Assert
            _taskRepositoryMock.Verify(repo => repo.AddAsync(task), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldCallRepositoryMethod()
        {
            // Arrange
            var task = new TaskModel { Id = Guid.NewGuid(), Title = "Updated Task" };

            _taskRepositoryMock.Setup(repo => repo.UpdateAsync(task)).Returns(Task.CompletedTask);

            // Act
            await _taskService.UpdateTaskAsync(task);

            // Assert
            _taskRepositoryMock.Verify(repo => repo.UpdateAsync(task), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldCallRepositoryMethod()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _taskRepositoryMock.Setup(repo => repo.DeleteAsync(taskId)).Returns(Task.CompletedTask);

            // Act
            await _taskService.DeleteTaskAsync(taskId);

            // Assert
            _taskRepositoryMock.Verify(repo => repo.DeleteAsync(taskId), Times.Once);
        }
    }
}
