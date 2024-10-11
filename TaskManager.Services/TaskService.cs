using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Abstractions;
using TaskManager.DataAccess;

namespace TaskManager.Services;

public class TaskService : ITaskService
{
    private readonly TaskManagerDbContext _context;

        public TaskService(TaskManagerDbContext context)
        {
            _context = context;
        }

        public void AddTask(string title, string description, int projectId)
        {
            var task = new Task
            {
                Title = title,
                Description = description,
                IsCompleted = false,
                ProjectId = projectId
            };
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public IEnumerable<Task> GetTasksByProject(int projectId)
        {
            return _context.Tasks.Where(t => t.ProjectId == projectId).ToList();
        }

        public void UpdateTask(int taskId, string title, string description)
        {
            var task = _context.Tasks.Find(taskId);
            if (task != null)
            {
                task.Title = title;
                task.Description = description;
                _context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Задача не найдена.");
            }
        }

        public void DeleteTask(int taskId)
        {
            var task = _context.Tasks.Find(taskId);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Задача не найдена.");
            }
        }
}