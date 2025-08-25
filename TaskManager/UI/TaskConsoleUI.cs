using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Interfaces;
using TaskManager.Services;

namespace TaskManager.UI
{
    public class TaskConsoleUI
    {
        private readonly ITaskService _taskService;

        public TaskConsoleUI(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public void Run()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("===========================");
                Console.WriteLine("     Task Manager App      ");
                Console.WriteLine("===========================");
                Console.WriteLine("1. View Tasks");
                Console.WriteLine("2. Add Task");
                Console.WriteLine("3. Mark Task as Complete");
                Console.WriteLine("4. Edit Task");
                Console.WriteLine("5. Exit");
                Console.Write("\nSelect an option: ");

                switch (Console.ReadLine())
                {
                    case "1": ViewTasks(_taskService); break;
                    case "2": AddTask(_taskService); break;
                    case "3": CompleteTask(_taskService); break;
                    case "4": EditTask(_taskService); break;
                    case "5": running = false; break;
                    default:
                        Console.WriteLine("Invalid option. Press Enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }
        static void ViewTasks(ITaskService taskService)
        {
            Console.Clear();
            var (pending, completed) = taskService.ListTasks();

            Console.WriteLine("Pending Tasks:\n");
            if (!pending.Any())
            {
                Console.WriteLine("  None");
            }
            else
            {
                for (int i = 0; i < pending.Count; i++)
                {
                    var task = pending[i];
                    Console.WriteLine($"  {i}: {(string.IsNullOrWhiteSpace(task.Title) ? "[None]" : task.Title)} (Due: {(task.DueDate != DateOnly.MinValue ? task.DueDate.ToShortDateString() : "[None]")})");
                    Console.WriteLine($"  Description: {(string.IsNullOrWhiteSpace(task.Description) ? "[None]" : task.Description)}\n");
                }
                
            }

            Console.WriteLine("\nCompleted Tasks:\n");
            if (!completed.Any())
            {
                Console.WriteLine("  None");
            }
            else
            {
                for (int i = 0; i < completed.Count; i++)
                {
                    var task = completed[i];
                    Console.WriteLine($"  {i}: {(string.IsNullOrWhiteSpace(task.Title) ? "[None]" : task.Title)} (Completed)");
                    Console.WriteLine($"  Description: {(string.IsNullOrWhiteSpace(task.Description) ? "[None]" : task.Description)}\n");
                }
            }

            Console.WriteLine("\nPress Enter to return to the menu.");
            Console.ReadLine();
        }
        static void AddTask(ITaskService taskService)
        {
            Console.Clear();
            Console.WriteLine("Add New Task\n");

            var title = PromptForValidTitle();

            var description = PromptForValidDescription();

            Console.Write("Enter due date (yyyy-MM-dd): ");
            if (!DateOnly.TryParse(Console.ReadLine(), out var dueDate))
            {
                Console.WriteLine("Invalid date format. Press Enter to return.");
                Console.ReadLine();
                return;
            }

            bool success = taskService.AddTask(title, description, dueDate);
            Console.WriteLine(success ? "Task added successfully." : "Failed to add task.");
            Console.WriteLine("Press Enter to return.");
            Console.ReadLine();
        }
        static void CompleteTask(ITaskService taskService)
        {
            Console.Clear();
            var (pending, _) = taskService.ListTasks();

            if (!pending.Any())
            {
                Console.WriteLine("No pending tasks to complete.");
                Console.WriteLine("Press Enter to return.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Pending Tasks:\n");
            for (int i = 0; i < pending.Count; i++)
            {
                Console.WriteLine($"{i}: {pending[i].Title} (Due: {pending[i].DueDate})");
            }

            Console.Write("\nEnter the index of the task to mark as complete: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 0 || index >= pending.Count)
            {
                Console.WriteLine("Invalid index. Press Enter to return.");
                Console.ReadLine();
                return;
            }

            Guid taskId = pending[index].Id;
            bool success = taskService.MarkTaskAsComplete(taskId);

            Console.WriteLine(success ? "Task marked as complete." : "Failed to mark task as complete.");
            Console.WriteLine("Press Enter to return.");
            Console.ReadLine();
        }
        static void EditTask(ITaskService taskService)
        {
            Console.Clear();
            var (pending, _) = taskService.ListTasks();

            if (!pending.Any())
            {
                Console.WriteLine("No pending tasks to edit.");
                Console.WriteLine("Press Enter to return.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Pending Tasks:\n");
            for (int i = 0; i < pending.Count; i++)
            {
                var task = pending[i];
                Console.WriteLine($"{i}: {task.Title} (Due: {task.DueDate})");
            }

            Console.Write("\nEnter the index of the task to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 0 || index >= pending.Count)
            {
                Console.WriteLine("Invalid index. Press Enter to return.");
                Console.ReadLine();
                return;
            }

            var original = pending[index];

            Console.WriteLine("\nLeave fields blank to keep existing values.");

            Console.Write($"New title [{original.Title}]: ");
            var newTitle = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newTitle)) newTitle = original.Title;

            Console.Write($"New description [{original.Description}]: ");
            var newDescription = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newDescription)) newDescription = original.Description;

            Console.Write($"New due date [{original.DueDate:yyyy-MM-dd}]: ");
            var dateInput = Console.ReadLine();
            DateOnly newDueDate = original.DueDate;
            if (!string.IsNullOrWhiteSpace(dateInput) && !DateOnly.TryParse(dateInput, out newDueDate))
            {
                Console.WriteLine("Invalid date format. Press Enter to return.");
                Console.ReadLine();
                return;
            }

            bool success = taskService.ModifyTask(index, newTitle, newDescription, newDueDate);
            Console.WriteLine(success ? "Task updated successfully." : "Failed to update task.");
            Console.WriteLine("Press Enter to return.");
            Console.ReadLine();
        }

        private static string PromptForValidTitle()
        {
            Console.Write("Enter task title: ");
            var title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Title cannot be empty. Please try again.");
                return PromptForValidTitle();
            }

            return title;
        }
        private static string PromptForValidDescription()
        {
            Console.Write("Enter task description: ");
            var description = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Description cannot be empty. Please try again.");
                return PromptForValidDescription();
            }

            return description;
        }
    }
}
