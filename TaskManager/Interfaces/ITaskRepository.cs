using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Interfaces
{
    public interface ITaskRepository
    {
        List<TaskItem> LoadTasks();
        void SaveTasks(List<TaskItem> tasks);
    }
}
