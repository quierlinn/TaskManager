namespace TaskManager.Core.Abstractions;

public interface ITaskRepository
{
    void Add(Task task);
    Task GetById(int id);
    IEnumerable<Task> GetAll();
    IEnumerable<Task> GetByProject(int projectId);
    void Update(Task task);
    void Delete(Task task);
}