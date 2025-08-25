using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Repositories;

namespace TaskManager.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public bool AddTask(string title, string description, DateOnly dueDate, Priority priority)
        {
            if (string.IsNullOrWhiteSpace(title)) return false;

            var tasks = _repository.LoadTasks();
            tasks.Add(new TaskItem
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                Priority = priority,
                IsComplete = false

            });

            return _repository.SaveTasks(tasks);

        }

        public (List<TaskItem> pending, List<TaskItem> completed) ListTasks()
        {
            var tasks = _repository.LoadTasks();
            var pending = tasks.Where(t => !t.IsComplete).OrderBy(t=>t.DueDate).ToList();
            var completed = tasks.Where(t => t.IsComplete).OrderByDescending(t=>t.UpdatedAt).ToList();
            return (pending, completed);

        }

        public bool MarkTaskAsComplete(Guid id)
        {
            var tasks = _repository.LoadTasks();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;
            task.UpdatedAt = DateTime.Now;

            task.IsComplete = true;
            return _repository.SaveTasks(tasks);
        }

        public bool ModifyTask(int index, string newTitle, string newDescription, DateOnly newDueDate, Priority newPriority)
        {
            var tasks = _repository.LoadTasks();
            if (index < 0 || index >= tasks.Count) return false;
            if (string.IsNullOrWhiteSpace(newTitle)) return false;


            var task = tasks[index];
            task.Title = newTitle;
            task.Description = newDescription;
            task.DueDate = newDueDate;
            task.Priority = newPriority;
            task.UpdatedAt = DateTime.Now;
            return _repository.SaveTasks(tasks);


        }
    }
}
