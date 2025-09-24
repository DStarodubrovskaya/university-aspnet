using TaskSchedulerDariaSt.Models;

namespace TaskSchedulerDariaSt.Services
{
    public interface ISessionService
    {
        public User? currentUser { get; set; }
        public Node<TaskItem>? firstTask { get; set; }

        // Ensures the state is loaded & reloads it from DB if needed
        public bool Load();
        // Updates users between sessions
        public void SetUser(User? user);
    }
}
