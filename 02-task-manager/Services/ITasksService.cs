using TaskManagerDariaSt.Models;
namespace TaskManagerDariaSt.Services
{
    public interface ITasksService
    {
        public bool AddTask(TaskItem newTask);
        public bool AssignTask(string idUser, string nameUser, string idTask);
        public bool UnAssignTask(string idUser);
        public List<TaskItem> DisplayTasksByUser(string idUser);
        public List<TaskItem> DisplayTasksByStatus(string status);
        public List<TaskItem> DisplayTasksByUserAndStatus(string idUser, string status);
        public bool UpdateTask(TaskItem updatedTask);
        public bool DeleteTask(string idTask);
        public List<TaskItem> GetAllTasks();
        public TaskItem GetTaskById(string idTask);
    }
}
