using TaskManager.Interfaces;
using TaskManager.Repositories;
using TaskManager.Services;
using TaskManager.UI;

namespace TaskManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var taskService = new TaskService(new TaskRepository("tasks.json"));
            var ui = new TaskConsoleUI(taskService);
            ui.Run();
        }

    }
}
