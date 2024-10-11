using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Core.Abstractions;
using TaskManager.DataAccess;
using TaskManager.DataAccess.Repositories;

namespace TaskManager.Tests;

public class UserTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly IUserService _userService;

    public UserTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _userService = new UserService(_mockUserRepository.Object);
    }
    [Fact]
    public void CreateUser_ShouldAddUser_WhenUserDoesNotExist()
    {
        // Arrange
        var username = "newuser";
        var password = "newpassword";
        var role = "User";
        _mockUserRepository.Setup(repo => repo.GetByUsername(username))
            .Returns((User)null);
        User capturedUser = null;
        _mockUserRepository.Setup(repo => repo.Add(It.IsAny<User>()))
            .Callback<User>(user => capturedUser = user);
        // Act
        _userService.CreateUser(username, password, role);
        // Assert
        _mockUserRepository.Verify(repo => repo.GetByUsername(username), Times.Once);
        _mockUserRepository.Verify(repo => repo.Add(It.IsAny<User>()), Times.Once);
        Assert.NotNull(capturedUser);
        Assert.Equal(username, capturedUser.Username);
        Assert.Equal(role, capturedUser.Role);
        Assert.True(BCrypt.Net.BCrypt.Verify(password, capturedUser.Password));
    }

    [Fact]
    public void CreateUser_ShouldThrowException_WhenUserAlreadyExists()
    {
        // Arrange
        var username = "existinguser";
        var password = "password";
        var role = "User";
        _mockUserRepository.Setup(repo => repo.GetByUsername(username))
            .Returns(new User { Username = username });
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _userService.CreateUser(username, password, role));
        Assert.Equal("Пользователь с таким именем уже существует.", exception.Message);
        _mockUserRepository.Verify(repo => repo.GetByUsername(username), Times.Once);
        _mockUserRepository.Verify(repo => repo.Add(It.IsAny<User>()), Times.Never);
    }
    [Fact]
    public void GetUserById_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var user = new User { Id = userId, Username = "user1", Role = "Admin" };
        _mockUserRepository.Setup(repo => repo.GetById(userId))
            .Returns(user);
        // Act
        var result = _userService.GetUserById(userId);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("user1", result.Username);
        Assert.Equal("Admin", result.Role);
        _mockUserRepository.Verify(repo => repo.GetById(userId), Times.Once);
    }

    [Fact]
    public void GetUserById_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = 1;
        _mockUserRepository.Setup(repo => repo.GetById(userId))
            .Returns((User)null);
        // Act
        var result = _userService.GetUserById(userId);
        // Assert
        Assert.Null(result);
        _mockUserRepository.Verify(repo => repo.GetById(userId), Times.Once);
    }
    [Fact]
    public void GetAllUsers_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Username = "user1", Role = "Admin" },
            new User { Id = 2, Username = "user2", Role = "User" }
        };
        _mockUserRepository.Setup(repo => repo.GetAll())
            .Returns(users);
        // Act
        var result = _userService.GetAllUsers();
        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, u => u.Username == "user1");
        Assert.Contains(result, u => u.Username == "user2");
        _mockUserRepository.Verify(repo => repo.GetAll(), Times.Once);
    }
    [Fact]
    public void UpdateUser_ShouldModifyUser_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var existingUser = new User { Id = userId, Username = "olduser", Role = "User" };
        _mockUserRepository.Setup(repo => repo.GetById(userId))
            .Returns(existingUser);
        // Act
        _userService.UpdateUser(userId, "newuser", "Admin");
        // Assert
        Assert.Equal("newuser", existingUser.Username);
        Assert.Equal("Admin", existingUser.Role);
        _mockUserRepository.Verify(repo => repo.GetById(userId), Times.Once);
        _mockUserRepository.Verify(repo => repo.Update(existingUser), Times.Once);
    }
    [Fact]
    public void UpdateUser_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = 1;
        var newUsername = "newuser";
        var newRole = "Admin";
        _mockUserRepository.Setup(repo => repo.GetById(userId))
            .Returns((User)null);
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _userService.UpdateUser(userId, newUsername, newRole));
        Assert.Equal("Пользователь не найден.", exception.Message);
        _mockUserRepository.Verify(repo => repo.GetById(userId), Times.Once);
        _mockUserRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public void DeleteUser_ShouldRemoveUser_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var existingUser = new User { Id = userId, Username = "user1", Role = "User" };
        _mockUserRepository.Setup(repo => repo.GetById(userId))
            .Returns(existingUser);
        // Act
        _userService.DeleteUser(userId);
        // Assert
        _mockUserRepository.Verify(repo => repo.GetById(userId), Times.Once);
        _mockUserRepository.Verify(repo => repo.Delete(existingUser), Times.Once);
    }

    [Fact]
    public void DeleteUser_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = 1;
        _mockUserRepository.Setup(repo => repo.GetById(userId))
            .Returns((User)null);
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _userService.DeleteUser(userId));
        Assert.Equal("Пользователь не найден.", exception.Message);
        _mockUserRepository.Verify(repo => repo.GetById(userId), Times.Once);
        _mockUserRepository.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Never);
    }
}