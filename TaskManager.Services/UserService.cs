using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using TaskManager.DataAccess;

namespace TaskManager.Services;

public class UserService
{
    private readonly TaskManagerDbContext _dbContext;

    public UserService(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ICollection<User> GetUsers()
    {
        return _dbContext.Users.ToList();
    }

    public User GetUserById(int id)
    {
        return _dbContext.Users.FirstOrDefault(u => u.Id == id);
    }

    public void AddUser(string username, string password, string role)
    {
        var user = new User
        {
            Username = username,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role
        };
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }

    public void UpdateUser(int id, string username, string role)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            user.Username = username;
            user.Role = role;
            _dbContext.SaveChanges();
        }
    }

    public void DeleteUser(int id)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }
    }
}