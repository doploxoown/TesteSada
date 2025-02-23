using Moq;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
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
            var tasks = new List<TaskModel>
            {
                new() { Id = Guid.NewGuid(), Title = "Task 1" },
                new() { Id = Guid.NewGuid(), Title = "Task 2" }
            };

            _taskRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tasks);

            var result = await _taskService.GetAllTasksAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetFilteredTasksAsync_ShouldReturnFilteredTaskDtos()
        {
            var taskList = new List<TaskModel>
            {
                new TaskModel { Id = Guid.NewGuid(), Title = "Task 1", Status = ETaskStatus.Pending, DueDate = DateTime.Today },
                new TaskModel { Id = Guid.NewGuid(), Title = "Task 2", Status = ETaskStatus.Completed, DueDate = DateTime.Today }
            };

            _taskRepositoryMock
                .Setup(repo => repo.GetFilteredTasksAsync(ETaskStatus.Pending, null))
                .ReturnsAsync(taskList.Where(t => t.Status == ETaskStatus.Pending));

            var result = await _taskService.GetFilteredTasksAsync(ETaskStatus.Pending, null);

            Assert.Single(result);
            Assert.Equal("Task 1", result.First().Title);
            _taskRepositoryMock.Verify(repo => repo.GetFilteredTasksAsync(ETaskStatus.Pending, null), Times.Once);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
        {
            var task = new TaskModel { Id = Guid.NewGuid(), Title = "Test Task" };

            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(task.Id)).ReturnsAsync(task);

            var result = await _taskService.GetTaskByIdAsync(task.Id);

            Assert.NotNull(result);
            Assert.Equal(task.Title, result.Title);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((TaskModel?)null);

            var result = await _taskService.GetTaskByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task AddTaskAsync_ShouldCallRepositoryMethod()
        {
            var createTaskDto = new CreateTaskDto
            {
                Title = "New Task",
                Description = "Description Task",
                DueDate = DateTime.UtcNow.AddDays(5)
            };

            await _taskService.AddTaskAsync(createTaskDto);

            _taskRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<TaskModel>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldCallRepositoryMethod()
        {
            var taskId = Guid.NewGuid();
            var taskDto = new UpdateTaskDto { Id = taskId, Title = "Updated Task" };

            _taskRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<TaskModel>()))
                .Returns(Task.CompletedTask);

            await _taskService.UpdateTaskAsync(taskDto);

            _taskRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<TaskModel>(t => t.Id == taskDto.Id)), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldCallRepositoryMethod()
        {
            var taskId = Guid.NewGuid();

            _taskRepositoryMock.Setup(repo => repo.DeleteAsync(taskId)).Returns(Task.CompletedTask);

            await _taskService.DeleteTaskAsync(taskId);

            _taskRepositoryMock.Verify(repo => repo.DeleteAsync(taskId), Times.Once);
        }
    }
}
