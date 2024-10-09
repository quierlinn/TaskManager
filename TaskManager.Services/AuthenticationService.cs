using TaskManager.DataAccess;

namespace TaskManager.Services;

public class AuthenticationService
{
    private readonly TaskManagerDbContext _context;

    public AuthenticationService(TaskManagerDbContext? context)
    {
        _context = context;
    }

    public void Register(string username, string password, string role)
    {
        var user = new User
        {
            Username = username,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role
        };

        _context.Users.Add(user);
        _context.SaveChanges();
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