using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Abstractions;
using TaskManager.DataAccess;

namespace TaskManager.Services;
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public void CreateTask(string title, string description, int projectId)
        {
            var task = new Task
            {
                Title = title,
                Description = description,
                ProjectId = projectId,
                IsCompleted = false,
            };

            _taskRepository.Add(task);
            Console.WriteLine("Задача успешно создана.");
        }

        public Task GetTaskById(int id)
        {
            return _taskRepository.GetById(id);
        }

        public IEnumerable<Task> GetAllTasks()
        {
            return _taskRepository.GetAll();
        }

        public IEnumerable<Task> GetTasksByProject(int projectId)
        {
            return _taskRepository.GetByProject(projectId);
        }

        public void UpdateTask(int id, string title, string description)
        {
            var task = _taskRepository.GetById(id);
            if (task == null)
            {
                throw new ArgumentException("Задача не найдена.");
            }

            task.Title = title;
            task.Description = description;

            _taskRepository.Update(task);
            Console.WriteLine("Задача успешно обновлена.");
        }

        public void DeleteTask(int id)
        {
            var task = _taskRepository.GetById(id);
            if (task == null)
            {
                throw new ArgumentException("Задача не найдена.");
            }

            _taskRepository.Delete(task);
            Console.WriteLine("Задача успешно удалена.");
        }
    }