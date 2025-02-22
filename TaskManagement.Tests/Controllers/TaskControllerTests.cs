using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces.Services;
using TaskManagementAPI.Controllers;

namespace TaskManagement.Tests.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly TaskController _taskController;

        public TaskControllerTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _taskController = new TaskController(_taskServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithTaskList()
        {
            // Arrange
            var tasks = new List<TaskModel> { new() { Id = Guid.NewGuid(), Title = "Task 1" } };

            _taskServiceMock.Setup(svc => svc.GetAllTasksAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _taskController.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTasks = Assert.IsType<List<TaskModel>>(okResult.Value);
            Assert.Single(returnedTasks);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            _taskServiceMock.Setup(svc => svc.GetTaskByIdAsync(It.IsAny<Guid>())).ReturnsAsync((TaskModel?)null);

            // Act
            var result = await _taskController.GetById(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenTaskIsCreated()
        {
            // Arrange
            var newTask = new TaskModel { Id = Guid.NewGuid(), Title = "New Task" };

            _taskServiceMock.Setup(svc => svc.AddTaskAsync(newTask)).Returns(Task.CompletedTask);

            // Act
            var result = await _taskController.Create(newTask);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(TaskController.GetById), createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var task = new TaskModel { Id = Guid.NewGuid(), Title = "Updated Task" };

            // Act
            var result = await _taskController.Update(Guid.NewGuid(), task);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenTaskIsDeleted()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _taskServiceMock.Setup(svc => svc.DeleteTaskAsync(taskId)).Returns(Task.CompletedTask);

            // Act
            var result = await _taskController.Delete(taskId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
