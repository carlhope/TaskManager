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
        private readonly TaskRepository _repository;

        public TaskService(TaskRepository repository)
        {
            _repository = repository;
        }

        public bool AddTask(string title, string description, DateOnly dueDate)
        {
            if (string.IsNullOrWhiteSpace(title)) return false;

            var tasks = _repository.LoadTasks();
            tasks.Add(new TaskItem
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                IsComplete = false
            });

            return _repository.SaveTasks(tasks);

        }

        public (List<TaskItem> pending, List<TaskItem> completed) ListTasks()
        {
            var tasks = _repository.LoadTasks();
            var pending = tasks.Where(t => !t.IsComplete).ToList();
            var completed = tasks.Where(t => t.IsComplete).ToList();
            return (pending, completed);

        }

        public bool MarkTaskAsComplete(int index)
        {
            var tasks = _repository.LoadTasks();
            if (index < 0 || index >= tasks.Count) return false;

            tasks[index].IsComplete = true;
            return _repository.SaveTasks(tasks);

        }

        public bool ModifyTask(int index, string newTitle, string newDescription, DateOnly newDueDate)
        {
            var tasks = _repository.LoadTasks();
            if (index < 0 || index >= tasks.Count) return false;

            var task = tasks[index];
            task.Title = newTitle;
            task.Description = newDescription;
            task.DueDate = newDueDate;
            return _repository.SaveTasks(tasks);


        }
    }
}
