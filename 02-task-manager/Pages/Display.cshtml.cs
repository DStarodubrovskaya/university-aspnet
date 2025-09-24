using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerDariaSt.Models;
using TaskManagerDariaSt.Services;

namespace TaskManagerDariaSt.Pages
{
    public class DisplayModel : PageModel
    {
        // Injecting task and user services
        private readonly ITasksService _tasksService;
        private readonly IUsersService _usersService;

        public DisplayModel(ITasksService tasksService, IUsersService usersService)
        {
            _tasksService = tasksService;
            _usersService = usersService;
        }
        
        [BindProperty]
        public TaskItem taskItem { get; set; } = new();
        [BindProperty]
        public List<TaskItem> Tasks { get; set; } = new();
        [BindProperty]
        public User user { get; set; } = new();

        // Full user list for dropdowns
        [BindProperty]
        public List<User> Users { get; set; } = new();

        // Controls if tasks exist
        [BindProperty]
        public bool HasTasks { get; set; } = true;

        // Loads all tasks and users on page load
        public void OnGet()
        {
            Tasks = _tasksService.GetAllTasks();
            Users = _usersService.GetAllUsers();
            if (!(Tasks.Count > 0))
            { 
                HasTasks = false;
            }
        }
        // Filters tasks based on selected user and status
        public void OnPostDisplayByUserAndStatus() {
            // Both filters applied
            if ((user.Id != "all") & (taskItem.Status != "all"))
            {
                Tasks = _tasksService.DisplayTasksByUserAndStatus(user.Id, taskItem.Status);
            }
            // No filters (show all)
            else if ((user.Id == "all") & (taskItem.Status == "all"))
            {
                Tasks = _tasksService.GetAllTasks();
            }
            // Filter by user only
            else if (user.Id != "all") 
            {
                Tasks = _tasksService.DisplayTasksByUser(user.Id);
            }
            // Filter by status only
            else
            {
                Tasks = _tasksService.DisplayTasksByStatus(taskItem.Status);
            }
            // Check if any tasks found
            if (!(Tasks.Count > 0))
            {
                HasTasks = false;
            }
            // Reloads user list for dropdown
            Users = _usersService.GetAllUsers();
        }
    }
}
