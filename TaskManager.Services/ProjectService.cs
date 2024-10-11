using TaskManager.Core.Abstractions;
using TaskManager.DataAccess;

namespace TaskManager.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public void CreateProject(string name, string description)
    {
        var project = new Project
        {
            Name = name,
            Description = description
        };
        _projectRepository.Add(project);
    }

    public Project GetProjectById(int id)
    {
        return _projectRepository.GetById(id);
    }

    public IEnumerable<Project> GetAllProjects()
    {
        return _projectRepository.GetAll();
    }

    public void UpdateProject(int id, string name, string description)
    {
        var project = _projectRepository.GetById(id);
        if (project == null)
        {
            throw new ArgumentException("Проект не найден.");
        }

        project.Name = name;
        project.Description = description;
        _projectRepository.Update(project);
    }

    public void DeleteProject(int id)
    {
        var project = _projectRepository.GetById(id);
        if (project == null)
        {
            throw new ArgumentException("Проект не найден.");
        }

        _projectRepository.Delete(project);
    }
}