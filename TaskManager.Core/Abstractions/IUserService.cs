namespace TaskManager.Core.Abstractions;

public interface IUserService
{
    IEnumerable<User> GetAllUsers();
    User GetUserById(int id);
    void AddUser(string username, string password, string role);
    void UpdateUser(int id, string username, string role);
    void DeleteUser(int id);
    bool UserExists(string username);
}