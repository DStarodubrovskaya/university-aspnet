using TaskSchedulerDariaSt.Models;
namespace TaskSchedulerDariaSt.Services
{
    public interface ITasksService
    {
        public Node<TaskItem>? AddTask(Node<TaskItem>? first, TaskItem newTask);
        public List<TaskItem> PrintTasks(Node<TaskItem>? first);
        public List<TaskItem> FilterTasks(Node<TaskItem>? first, int priority);
        public int GetTotalTime(Node<TaskItem>? first);
        public Node<TaskItem>? RemoveTaskByName(Node<TaskItem>? first, string name);
        public Node<TaskItem>? RemoveTaskById(Node<TaskItem>? first, string id);
        public TaskItem? GetTaskById(Node<TaskItem>? first, string id);
    }
}
