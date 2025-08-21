using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _filePath;
        public TaskRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<TaskItem> LoadTasks()
        {
            if (!File.Exists(_filePath)) return new List<TaskItem>();

            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
            }
            catch
            {
                return new List<TaskItem>();
            }

        }

        public bool SaveTasks(List<TaskItem> tasks)
        {
            if (tasks.Any(t => string.IsNullOrWhiteSpace(t.Title)))
            {
                Console.WriteLine("Validation error: Task title cannot be empty.");
                return false;
            }

            try
            {
                var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error saving tasks: {ex.Message}");
                return false;
            }
        }
    }
}
