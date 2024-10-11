using TaskManager.Core.Abstractions;
using TaskManager.DataAccess;

namespace TaskManager.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void Register(string username, string password, string role)
    {
        var existingUser = _userRepository.GetByUsername(username);
        if (existingUser != null)
        {
            throw new ArgumentException("Пользователь с таким именем уже существует.");
        }

        var user = new User
        {
            Username = username,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role
        };

        _userRepository.Add(user);
        Console.WriteLine("Пользователь успешно зарегистрирован.");
    }

    public User Login(string username, string password)
    {
        var user = _userRepository.GetByUsername(username);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return user;
        }
        return null;
    }
}