namespace TaskManager.Core.Abstractions;

public interface IAuthenticationRepository
{
    public void Register(string username, string password, string role);
    public User Login(string username, string password);
}