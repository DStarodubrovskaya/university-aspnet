using TaskSchedulerDariaSt.Models;
namespace TaskSchedulerDariaSt.Services
{
    public interface IUsersService
    {
        public bool AddUser(User newUser);
        public bool DeleteUser(string id);
        public List<User> GetAllUsers();
        public User? GetUserById(string id);
        public Node<TaskItem>? GetTasksByUser(string id);
        public bool UpdateTasks(string id, Node<TaskItem>? first);
    }
}
