namespace TaskManager.Core.Abstractions;

public interface IProjectService
{
    void CreateProject(string name, string description);
    Project GetProjectById(int id);
    IEnumerable<Project> GetAllProjects();
    void UpdateProject(int id, string name, string description);
    void DeleteProject(int id);
}