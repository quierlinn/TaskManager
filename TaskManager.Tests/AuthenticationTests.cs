using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;
using TaskManager.Core.Abstractions;
using TaskManager.DataAccess;
using TaskManager.Services;

namespace TaskManager.Tests;

public class AuthenticationTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly IAuthenticationService _authService;

    public AuthenticationTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _authService = new AuthenticationService(_mockUserRepository.Object);
    }

    [Fact]
    public void Register_ShouldAddUser_WhenUserDoesNotExist()
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
        _authService.Register(username, password, role);
        // Assert
        _mockUserRepository.Verify(repo => repo.GetByUsername(username), Times.Once);
        _mockUserRepository.Verify(repo => repo.Add(It.IsAny<User>()), Times.Once);
        Assert.NotNull(capturedUser);
        Assert.Equal(username, capturedUser.Username);
        Assert.Equal(role, capturedUser.Role);
        Assert.True(BCrypt.Net.BCrypt.Verify(password, capturedUser.Password));
    }
    [Fact]
    public void Register_ShouldThrowException_WhenUserAlreadyExists()
    {
        // Arrange
        var username = "existinguser";
        var password = "password";
        var role = "User";
        _mockUserRepository.Setup(repo => repo.GetByUsername(username))
            .Returns(new User { Username = username });
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _authService.Register(username, password, role));
        Assert.Equal("Пользователь с таким именем уже существует.", exception.Message);
        _mockUserRepository.Verify(repo => repo.GetByUsername(username), Times.Once);
        _mockUserRepository.Verify(repo => repo.Add(It.IsAny<User>()), Times.Never);
    }
    [Fact]
    public void Login_ShouldReturnUser_WhenCredentialsAreValid()
    {
        // Arrange
        var username = "validuser";
        var password = "validpassword";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User { Username = username, Password = hashedPassword, Role = "User" };
        _mockUserRepository.Setup(repo => repo.GetByUsername(username))
            .Returns(user);
        // Act
        var result = _authService.Login(username, password);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
        _mockUserRepository.Verify(repo => repo.GetByUsername(username), Times.Once);
    }
    [Fact]
    public void Login_ShouldReturnNull_WhenPasswordIsInvalid()
    {
        // Arrange
        var username = "validuser";
        var password = "validpassword";
        var wrongPassword = "wrongpassword";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User { Username = username, Password = hashedPassword, Role = "User" };
        _mockUserRepository.Setup(repo => repo.GetByUsername(username))
            .Returns(user);
        // Act
        var result = _authService.Login(username, wrongPassword);
        // Assert
        Assert.Null(result);
        _mockUserRepository.Verify(repo => repo.GetByUsername(username), Times.Once);
    }
    [Fact]
    public void Login_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var username = "nonexistentuser";
        var password = "password";
        _mockUserRepository.Setup(repo => repo.GetByUsername(username))
            .Returns((User)null);
        // Act
        var result = _authService.Login(username, password);
        // Assert
        Assert.Null(result);
        _mockUserRepository.Verify(repo => repo.GetByUsername(username), Times.Once);
    }
}