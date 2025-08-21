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
        private const string FilePath = "tasks.json";

        public List<TaskItem> LoadTasks()
        {
            if (!File.Exists(FilePath)) return new List<TaskItem>();

            try
            {
                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
            }
            catch
            {
                return new List<TaskItem>();
            }

        }

        public void SaveTasks(List<TaskItem> tasks)
        {
            try
            {
                var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch
            {
                Console.WriteLine("Error saving tasks.");
            }

        }
    }
}
