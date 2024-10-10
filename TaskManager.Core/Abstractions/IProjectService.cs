namespace TaskManager.Core.Abstractions;

public interface IProjectService
{ 
    IEnumerable<Project> GetAllProjects();

    Project GetProjectById(int id);

    void AddProject(string name, string description);

    void UpdateProject(int id, string name, string description);

    void DeleteProject(int id);
}