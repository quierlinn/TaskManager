using TaskManager.Core.Abstractions;

namespace TaskManager.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TaskManagerDbContext _context;

    public UserRepository(TaskManagerDbContext context)
    {
        _context = context;
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User GetById(int id)
    {
        return _context.Users.Find(id);
    }

    public User GetByUsername(string username)
    {
        return _context.Users.SingleOrDefault(u => u.Username == username);
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users.ToList();
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
    }
}