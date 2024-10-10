using TaskManager.DataAccess;

namespace TaskManager.Services;

public class ProjectService
{
    private readonly TaskManagerDbContext _dbContext;

    public ProjectService(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Project> GetProjects()
    {
        return _dbContext.Projects.ToList();
    }

    public Project GetProjectById(int id)
    {
        return _dbContext.Projects.FirstOrDefault(p => p.Id == id);
    }

    public void AddProject(string name, string description)
    {
        var project = new Project
        {
            Name = name,
            Description = description
        };
        _dbContext.Projects.Add(project);
        _dbContext.SaveChanges();
    }

    public void UpdateProject(int id, string name, string description)
    {
        var project = _dbContext.Projects.FirstOrDefault(p => p.Id == id);
        if (project != null)
        {
            project.Name = name;
            project.Description = description;
            _dbContext.SaveChanges();
        }
    }

    public void DeleteProject(int id)
    {
        var project = _dbContext.Projects.FirstOrDefault(p => p.Id == id);
        if (project != null)
        {
            _dbContext.Projects.Remove(project);
            _dbContext.SaveChanges();
        }
    }
}