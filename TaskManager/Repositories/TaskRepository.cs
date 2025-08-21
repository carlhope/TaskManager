using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        public List<TaskItem> LoadTasks()
        {
            throw new NotImplementedException();
        }

        public void SaveTasks(List<TaskItem> tasks)
        {
            throw new NotImplementedException();
        }
    }
}
