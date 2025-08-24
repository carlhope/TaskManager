using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Interfaces
{
    public interface ITaskService
    {
      
            bool AddTask(string title, string description, DateOnly dueDate);
            bool ModifyTask(int index, string newTitle, string newDescription, DateOnly newDueDate);
            bool MarkTaskAsComplete(int index);
            (List<TaskItem> pending, List<TaskItem> completed) ListTasks();
    }
}
