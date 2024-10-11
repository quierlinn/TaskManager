using TaskManager.Core.Abstractions;
using TaskManager.DataAccess;

namespace TaskManager.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly TaskManagerDbContext _context;

    public AuthenticationService(TaskManagerDbContext context)
    {
        _context = context;
    }

    public void Register(string username, string password, string role)
    {
        if (_context.Users.Any(u => u.Username == username))
        {
            Console.WriteLine("Пользователь с таким именем уже существует.");
            return;
        }

        var user = new User
        {
            Username = username,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role
        };

        _context.Users.Add(user);
        _context.SaveChanges();
        Console.WriteLine("Пользователь успешно зарегистрирован.");
    }

    public User Login(string username, string password)
    {
        var user = _context.Users.SingleOrDefault(u => u.Username == username);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return user;
        }
        return null;
    }
}