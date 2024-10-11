namespace TaskManager.Core.Abstractions;

public interface ITaskService
{
    void AddTask(string title, string description, int projectId);
    IEnumerable<Task> GetTasksByProject(int projectId);
    void UpdateTask(int taskId, string title, string description);
    void DeleteTask(int taskId);
}