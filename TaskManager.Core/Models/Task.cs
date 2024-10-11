namespace TaskManager;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
}