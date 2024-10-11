namespace TaskManager.Core.Abstractions;

public interface IProjectRepository
{
    void Add(Project project);
    Project GetById(int id);
    IEnumerable<Project> GetAll();
    void Update(Project project);
    void Delete(Project project);
}