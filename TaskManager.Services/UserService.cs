using TaskManager;
using TaskManager.Core.Abstractions;
using TaskManager.DataAccess;

public class UserService : IUserService
{
    private readonly TaskManagerDbContext _context;

    public UserService(TaskManagerDbContext context)
    {
        _context = context;
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _context.Users.ToList();
    }

    public User GetUserById(int id)
    {
        return _context.Users.Find(id);
    }

    public void AddUser(string username, string password, string role)
    {
        var user = new User
        {
            Username = username,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role
        };

        _context.Users.Add(user);
        _context.SaveChanges();
        Console.WriteLine("Пользователь добавлен.");
    }

    public void UpdateUser(int id, string username, string role)
    {
        var user = _context.Users.Find(id);
        if (user != null)
        {
            user.Username = username;
            user.Role = role;
            _context.SaveChanges();
            Console.WriteLine("Пользователь обновлен.");
        }
        else
        {
            Console.WriteLine("Пользователь не найден.");
        }
    }

    public void DeleteUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
            Console.WriteLine("Пользователь удален.");
        }
        else
        {
            Console.WriteLine("Пользователь не найден.");
        }
    }

    public bool UserExists(string username)
    {
        return _context.Users.Any(u => u.Username == username);
    }
}