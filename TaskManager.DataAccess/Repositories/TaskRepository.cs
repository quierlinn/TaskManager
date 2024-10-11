using TaskManager.Core.Abstractions;

namespace TaskManager.DataAccess.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskManagerDbContext _context;

    public TaskRepository(TaskManagerDbContext context)
    {
        _context = context;
    }

    public void Add(Task task)
    {
        _context.Tasks.Add(task);
        _context.SaveChanges();
    }

    public Task GetById(int id)
    {
        return _context.Tasks.Find(id);
    }

    public IEnumerable<Task> GetAll()
    {
        return _context.Tasks.ToList();
    }

    public IEnumerable<Task> GetByProject(int projectId)
    {
        return _context.Tasks.Where(t => t.ProjectId == projectId).ToList();
    }

    public void Update(Task task)
    {
        _context.Tasks.Update(task);
        _context.SaveChanges();
    }

    public void Delete(Task task)
    {
        _context.Tasks.Remove(task);
        _context.SaveChanges();
    }
}