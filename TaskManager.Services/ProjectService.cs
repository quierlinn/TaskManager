using TaskManager.Core.Abstractions;
using TaskManager.DataAccess;

namespace TaskManager.Services;

public class ProjectService : IProjectService
{
    private readonly TaskManagerDbContext _context;

    public ProjectService(TaskManagerDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Project> GetAllProjects()
    {
        return _context.Projects.ToList();
    }

    public Project GetProjectById(int id)
    {
        return _context.Projects.Find(id);
    }

    public void AddProject(string name, string description)
    {
        var project = new Project
        {
            Name = name,
            Description = description
        };

        _context.Projects.Add(project);
        _context.SaveChanges();
        Console.WriteLine("Проект добавлен.");
    }

    public void UpdateProject(int id, string name, string description)
    {
        var project = _context.Projects.Find(id);
        if (project != null)
        {
            project.Name = name;
            project.Description = description;
            _context.SaveChanges();
            Console.WriteLine("Проект обновлен.");
        }
        else
        {
            Console.WriteLine("Проект не найден.");
        }
    }

    public void DeleteProject(int id)
    {
        var project = _context.Projects.Find(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            _context.SaveChanges();
            Console.WriteLine("Проект удален.");
        }
        else
        {
            Console.WriteLine("Проект не найден.");
        }
    }
}