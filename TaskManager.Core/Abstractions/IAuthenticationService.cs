namespace TaskManager.Core.Abstractions;

public interface IAuthenticationService
{
    void Register(string username, string password, string role);
    User Login(string username, string password);
}