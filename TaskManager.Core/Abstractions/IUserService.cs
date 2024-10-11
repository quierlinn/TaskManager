namespace TaskManager.Core.Abstractions;

public interface IUserService
{
    void CreateUser(string username, string password, string role);
    User GetUserById(int id);
    User GetUserByUsername(string username);
    IEnumerable<User> GetAllUsers();
    void UpdateUser(int id, string username, string role);
    void DeleteUser(int id);
}