namespace TaskManager.Core.Abstractions;

public interface ITaskService
{
    void CreateTask(string title, string description, int projectId);
    Task GetTaskById(int id);
    IEnumerable<Task> GetAllTasks();
    IEnumerable<Task> GetTasksByProject(int projectId);
    void UpdateTask(int id, string title, string description);
    void DeleteTask(int id);
}