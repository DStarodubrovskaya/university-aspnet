using TaskSchedulerDariaSt.Models;

namespace TaskSchedulerDariaSt.Services
{
    public class SessionService : ISessionService
    {
        // Injecting task and user services
        private readonly IHttpContextAccessor _HttpContext;
        private readonly IUsersService _userService;
        public SessionService(IHttpContextAccessor HttpContext, IUsersService userService)
        {
            _HttpContext = HttpContext;
            _userService = userService;
        }
        private ISession Session => _HttpContext.HttpContext!.Session;
        public User? currentUser {
            get
            {
                return Session.Get<User>("CurrentUser"); // takes from Session
            }
            set
            {
                Session.Set("CurrentUser", value); // saves to Session
            }
        }
        public Node<TaskItem>? firstTask
        {
            get
            {
                return Session.Get<Node<TaskItem>>("CurrentUserTasks"); // takes from Session
            }
            set
            {
                Session.Set("CurrentUserTasks", value); // saves to Session
            }
        }

        // This method checks for existing session data.
        // If missing, loads tasks from DB as a linked list & stores in session
        public bool Load()
        {
            if (currentUser == null)
                return false; // Prevents entering the App without Loggin

            if (firstTask == null)
            {
                firstTask = _userService.GetTasksByUser(currentUser.Id); // Loads tasks from DB
            }
            return true;
        }
        public void SetUser(User? user)
        {
            currentUser = user;           // Updates user in Session
            firstTask = null;           // Resets the task list
        }
    }
}
