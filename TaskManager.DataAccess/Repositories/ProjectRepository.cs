using TaskManager.Core.Abstractions;

namespace TaskManager.DataAccess.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly TaskManagerDbContext _context;

    public ProjectRepository(TaskManagerDbContext context)
    {
        _context = context;
    }

    public void Add(Project project)
    {
        _context.Projects.Add(project);
        _context.SaveChanges();
    }

    public Project GetById(int id)
    {
        return _context.Projects.Find(id);
    }

    public IEnumerable<Project> GetAll()
    {
        return _context.Projects.ToList();
    }

    public void Update(Project project)
    {
        _context.Projects.Update(project);
        _context.SaveChanges();
    }

    public void Delete(Project project)
    {
        _context.Projects.Remove(project);
        _context.SaveChanges();
    }
}