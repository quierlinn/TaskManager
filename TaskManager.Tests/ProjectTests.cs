using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Core.Abstractions;
using TaskManager.DataAccess;
using TaskManager.DataAccess.Repositories;
using TaskManager.Services;

namespace TaskManager.Tests;

public class ProjectTests
{
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly IProjectService _projectService;

    public ProjectTests()
    {
        _mockProjectRepository = new Mock<IProjectRepository>();
        _projectService = new ProjectService(_mockProjectRepository.Object);
    }
    [Fact]
    public void GetProjectById_ShouldReturnProject_WhenProjectExists()
    {
        // Arrange
        var projectId = 1;
        var project = new Project { Id = projectId, Name = "Project 1", Description = "Description 1" };
        _mockProjectRepository.Setup(repo => repo.GetById(projectId))
            .Returns(project);
        // Act
        var result = _projectService.GetProjectById(projectId);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(projectId, result.Id);
        Assert.Equal("Project 1", result.Name);
    }
    [Fact]
    public void GetProjectById_ShouldReturnNull_WhenProjectDoesNotExist()
    {
        // Arrange
        var projectId = 1;
        _mockProjectRepository.Setup(repo => repo.GetById(projectId))
            .Returns((Project)null);
        // Act
        var result = _projectService.GetProjectById(projectId);
        // Assert
        Assert.Null(result);
    }
    [Fact]
    public void GetAllProjects_ShouldReturnAllProjects()
    {
        // Arrange
        var projects = new List<Project>
        {
            new Project { Id = 1, Name = "Project 1", Description = "Description 1" },
            new Project { Id = 2, Name = "Project 2", Description = "Description 2" }
        };
        _mockProjectRepository.Setup(repo => repo.GetAll())
            .Returns(projects);
        // Act
        var result = _projectService.GetAllProjects();
        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, p => p.Name == "Project 1");
        Assert.Contains(result, p => p.Name == "Project 2");
    }

    [Fact]
    public void UpdateProject_ShouldModifyProject_WhenProjectExists()
    {
        // Arrange
        var projectId = 1;
        var existingProject = new Project { Id = projectId, Name = "Old Name", Description = "Old Description" };
        _mockProjectRepository.Setup(repo => repo.GetById(projectId))
            .Returns(existingProject);
        // Act
        _projectService.UpdateProject(projectId, "New Name", "New Description");
        // Assert
        Assert.Equal("New Name", existingProject.Name);
        Assert.Equal("New Description", existingProject.Description);
        _mockProjectRepository.Verify(repo => repo.Update(existingProject), Times.Once);
    }

    [Fact]
    public void UpdateProject_ShouldThrowException_WhenProjectDoesNotExist()
    {
        // Arrange
        var projectId = 1;
        var newName = "New Name";
        var newDescription = "New Description";
        _mockProjectRepository.Setup(repo => repo.GetById(projectId))
            .Returns((Project)null);
        // Act & Assert
        var exception =
            Assert.Throws<ArgumentException>(() => _projectService.UpdateProject(projectId, newName, newDescription));
        Assert.Equal("Проект не найден.", exception.Message);
        _mockProjectRepository.Verify(repo => repo.Update(It.IsAny<Project>()), Times.Never);
    }

    [Fact]
    public void DeleteProject_ShouldRemoveProject_WhenProjectExists()
    {
        // Arrange
        var projectId = 1;
        var existingProject = new Project { Id = projectId, Name = "Project to Delete", Description = "Description" };
        _mockProjectRepository.Setup(repo => repo.GetById(projectId))
            .Returns(existingProject);
        // Act
        _projectService.DeleteProject(projectId);
        // Assert
        _mockProjectRepository.Verify(repo => repo.Delete(existingProject), Times.Once);
    }
    [Fact]
    public void DeleteProject_ShouldThrowException_WhenProjectDoesNotExist()
    {
        // Arrange
        var projectId = 1;
        _mockProjectRepository.Setup(repo => repo.GetById(projectId))
            .Returns((Project)null);
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _projectService.DeleteProject(projectId));
        Assert.Equal("Проект не найден.", exception.Message);
        _mockProjectRepository.Verify(repo => repo.Delete(It.IsAny<Project>()), Times.Never);
    }
}