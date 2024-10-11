using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Core.Abstractions;
using TaskManager.DataAccess;
using TaskManager.DataAccess.Repositories;
using TaskManager.Services;

namespace TaskManager.Tests;

public class TaskTests
{
    private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly ITaskService _taskService;

        public TaskTests()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _taskService = new TaskService(_mockTaskRepository.Object);
        }
        [Fact]
        public void CreateTask_ShouldAddTask()
        {
            // Arrange
            var title = "New Task";
            var description = "Task Description";
            var userId = 1;

            Task capturedTask = null;
            _mockTaskRepository.Setup(repo => repo.Add(It.IsAny<Task>()))
                .Callback<Task>(task => capturedTask = task);
            // Act
            _taskService.CreateTask(title, description, userId);
            // Assert
            _mockTaskRepository.Verify(repo => repo.Add(It.IsAny<Task>()), Times.Once);
            Assert.NotNull(capturedTask);
            Assert.Equal(title, capturedTask.Title);
            Assert.Equal(description, capturedTask.Description);
            Assert.Equal(userId, capturedTask.ProjectId);
        }
        [Fact]
        public void GetTaskById_ShouldReturnTask_WhenTaskExists()
        {
            // Arrange
            var taskId = 1;
            var task = new Task { Id = taskId, Title = "Task 1", Description = "Description 1", ProjectId = 1 };
            _mockTaskRepository.Setup(repo => repo.GetById(taskId))
                .Returns(task);
            // Act
            var result = _taskService.GetTaskById(taskId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            Assert.Equal("Task 1", result.Title);
            Assert.Equal("Description 1", result.Description);
            _mockTaskRepository.Verify(repo => repo.GetById(taskId), Times.Once);
        }
        [Fact]
        public void GetTaskById_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = 1;
            _mockTaskRepository.Setup(repo => repo.GetById(taskId))
                .Returns((Task)null);
            // Act
            var result = _taskService.GetTaskById(taskId);
            // Assert
            Assert.Null(result);
            _mockTaskRepository.Verify(repo => repo.GetById(taskId), Times.Once);
        }
        [Fact]
        public void GetAllTasks_ShouldReturnAllTasks()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task { Id = 1, Title = "Task 1", Description = "Description 1", ProjectId = 1 },
                new Task { Id = 2, Title = "Task 2", Description = "Description 2", ProjectId = 2 }
            };
            _mockTaskRepository.Setup(repo => repo.GetAll())
                .Returns(tasks);
            // Act
            var result = _taskService.GetAllTasks();
            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, t => t.Title == "Task 1");
            Assert.Contains(result, t => t.Title == "Task 2");
            _mockTaskRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void GetTasksByUserId_ShouldReturnTasksForUser()
        {
            // Arrange
            var projectId = 1;
            var tasks = new List<Task>
            {
                new Task { Id = 1, Title = "Task 1", Description = "Description 1", ProjectId = projectId },
                new Task { Id = 2, Title = "Task 2", Description = "Description 2", ProjectId = projectId }
            };
            _mockTaskRepository.Setup(repo => repo.GetByProject(projectId))
                .Returns(tasks);
            // Act
            var result = _taskService.GetTasksByProject(projectId);
            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, t => Assert.Equal(projectId, t.ProjectId));
            _mockTaskRepository.Verify(repo => repo.GetByProject(projectId), Times.Once);
        }
        [Fact]
        public void UpdateTask_ShouldModifyTask_WhenTaskExists()
        {
            // Arrange
            var taskId = 1;
            var existingTask = new Task
            {
                Id = taskId,
                Title = "Old Task",
                Description = "Old Description",
                ProjectId = 1,
            };
            _mockTaskRepository.Setup(repo => repo.GetById(taskId))
                .Returns(existingTask);
            // Act
            var newTitle = "Updated Task";
            var newDescription = "Updated Description";
            _taskService.UpdateTask(taskId, newTitle, newDescription);
            // Assert
            Assert.Equal(newTitle, existingTask.Title);
            Assert.Equal(newDescription, existingTask.Description);
            _mockTaskRepository.Verify(repo => repo.GetById(taskId), Times.Once);
            _mockTaskRepository.Verify(repo => repo.Update(existingTask), Times.Once);
        }

        [Fact]
        public void UpdateTask_ShouldThrowException_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = 1;
            var newTitle = "Updated Task";
            var newDescription = "Updated Description";
            _mockTaskRepository.Setup(repo => repo.GetById(taskId))
                .Returns((Task)null);
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _taskService.UpdateTask(taskId, newTitle, newDescription));
            Assert.Equal("Задача не найдена.", exception.Message);
            _mockTaskRepository.Verify(repo => repo.GetById(taskId), Times.Once);
            _mockTaskRepository.Verify(repo => repo.Update(It.IsAny<Task>()), Times.Never);
        }

        [Fact]
        public void DeleteTask_ShouldRemoveTask_WhenTaskExists()
        {
            // Arrange
            var taskId = 1;
            var existingTask = new Task { Id = taskId, Title = "Task to Delete", Description = "Description", ProjectId = 1 };
            _mockTaskRepository.Setup(repo => repo.GetById(taskId))
                .Returns(existingTask);
            // Act
            _taskService.DeleteTask(taskId);
            // Assert
            _mockTaskRepository.Verify(repo => repo.GetById(taskId), Times.Once);
            _mockTaskRepository.Verify(repo => repo.Delete(existingTask), Times.Once);
        }
        [Fact]
        public void DeleteTask_ShouldThrowException_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = 1;
            _mockTaskRepository.Setup(repo => repo.GetById(taskId))
                .Returns((Task)null);
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _taskService.DeleteTask(taskId));
            Assert.Equal("Задача не найдена.", exception.Message);
            _mockTaskRepository.Verify(repo => repo.GetById(taskId), Times.Once);
            _mockTaskRepository.Verify(repo => repo.Delete(It.IsAny<Task>()), Times.Never);
        }
}