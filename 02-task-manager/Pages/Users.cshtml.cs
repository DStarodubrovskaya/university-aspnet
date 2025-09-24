using TaskManagerDariaSt.Models;
using TaskManagerDariaSt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskManagerDariaSt.Pages
{
    public class UsersModel : PageModel
    {
        // Injecting task and user services
        private readonly ITasksService _tasksService;
        private readonly IUsersService _usersService;

        public UsersModel(ITasksService tasksService, IUsersService usersService)
        {
            _tasksService = tasksService;
            _usersService = usersService;
        }
        
        [BindProperty]
        public User user { get; set; } = new();
        // Full user list for dropdown
        [BindProperty]
        public List<User> Users { get; set; } = new();

        // Result flag for actions
        [BindProperty]
        public bool result { get; set; }

        // TempData messages for UI feedback
        [TempData]
        public string? AddResult { get; set; }
        [TempData]
        public string? DeleteResult { get; set; }

        // Loads all users on page load
        public void OnGet()
        {
            Users = _usersService.GetAllUsers();
        }

        // Adds new user
        public IActionResult OnPostAdd()
        {
            result = _usersService.AddUser(user);
            if (result)
            {
                AddResult = $"Success! The user has been added.";
            }
            else
            {
                AddResult = $"Action failed.";
            }
            return RedirectToPage();
        }

        // Deletes user AND! unassigns them from tasks
        public IActionResult OnPostDelete()
        {
            // First unassigns user from all tasks
            result = _tasksService.UnAssignTask(user.Id);
            // Then deletes user from DB
            result = _usersService.DeleteUser(user.Id);
            if (result)
            {
                DeleteResult = $"Success! The user has been deleted.";
            }
            else
            {
                DeleteResult = $"Action failed.";
            }
            return RedirectToPage();
        }
    }
}
