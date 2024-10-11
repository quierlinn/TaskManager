namespace TaskManager.Core.Abstractions;

public interface IUserRepository
{
    void Add(User user);
    User GetById(int id);
    User GetByUsername(string username);
    IEnumerable<User> GetAll();
    void Update(User user);
    void Delete(User user);
}