using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Enums;
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
            var tasks = new List<TaskDto> { new() { Id = Guid.NewGuid(), Title = "Task 1" } };

            _taskServiceMock.Setup(svc => svc.GetAllTasksAsync()).ReturnsAsync(tasks);

            var result = await _taskController.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTasks = Assert.IsType<List<TaskDto>>(okResult.Value);
            Assert.Single(returnedTasks);
        }

        [Fact]
        public async Task GetFilteredTasks_ReturnsOkResult_WithFilteredTasks()
        {
            var status = ETaskStatus.Completed;
            var dueDate = new DateTime(2025, 2, 22);
            var tasks = new List<TaskDto>
            {
                new TaskDto { Id = Guid.NewGuid(), Status = ETaskStatus.Completed, DueDate = dueDate, Description = "Test task" }
            };

            _taskServiceMock
                .Setup(service => service.GetFilteredTasksAsync(status, dueDate))
                .ReturnsAsync(tasks);

            var result = await _taskController.GetFilteredTasks(status, dueDate);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<List<TaskDto>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.Equal(tasks[0].Id, returnValue[0].Id);
            Assert.Equal(tasks[0].Status, returnValue[0].Status);
        }

        [Fact]
        public async Task GetFilteredTasks_ReturnsOkResult_WithNoTasks()
        {
            var status = ETaskStatus.InProgress;
            var dueDate = new DateTime(2025, 3, 1);
            var tasks = new List<TaskDto>();

            _taskServiceMock
                .Setup(service => service.GetFilteredTasksAsync(status, dueDate))
                .ReturnsAsync(tasks);

            var result = await _taskController.GetFilteredTasks(status, dueDate);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<List<TaskDto>>(okResult.Value);
            Assert.Empty(returnValue);
        }


        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            _taskServiceMock.Setup(svc => svc.GetTaskByIdAsync(It.IsAny<Guid>())).ReturnsAsync((TaskDto?)null);

            var result = await _taskController.GetById(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenTaskIsCreated()
        {
            var newTask = new CreateTaskDto { Title = "New Task" };
            var taskCreated = new TaskDto { Id = Guid.NewGuid(), Title = "New Task" };

            _taskServiceMock.Setup(svc => svc.AddTaskAsync(newTask)).ReturnsAsync(taskCreated);

            var result = await _taskController.Create(newTask);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(TaskController.GetById), createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenIdsDoNotMatch()
        {
            var task = new TaskDto { Id = Guid.NewGuid(), Title = "Updated Task" };

            var result = await _taskController.Update(Guid.NewGuid(), task);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenTaskIsDeleted()
        {
            var taskId = Guid.NewGuid();

            _taskServiceMock.Setup(svc => svc.DeleteTaskAsync(taskId)).Returns(Task.CompletedTask);

            var result = await _taskController.Delete(taskId);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
