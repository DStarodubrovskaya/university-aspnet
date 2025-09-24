using TaskManagerDariaSt.Models;
using TaskManagerDariaSt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace TaskManagerDariaSt.Pages
{
    public class TasksModel : PageModel
    {
        // Injecting task and user services
        private readonly ITasksService _tasksService;
        private readonly IUsersService _usersService;

        public TasksModel(ITasksService tasksService, IUsersService usersService)
        {
            _tasksService = tasksService;
            _usersService = usersService;
        }
        [BindProperty]
        public TaskItem taskItem { get; set; } = new();

        // List of tasks for dropdowns
        [BindProperty]
        public List<TaskItem> Tasks { get; set; } = new();
        [BindProperty]
        public User user { get; set; } = new();

        // List of users for dropdowns
        [BindProperty]
        public List<User> Users { get; set; } = new();

        // Result flag for service actions
        [BindProperty]
        public bool result { get; set; }

        // TempData messages for UI feedback
        [TempData]
        public string? AddResult { get; set; }
        [TempData]
        public string? DeleteResult { get; set; }
        [TempData]
        public string? AssignResult { get; set; }

        // Loads tasks and users on page load
        public void OnGet()
        {
            Tasks = _tasksService.GetAllTasks();
            Users = _usersService.GetAllUsers();
        }

        // Adds new task to DB
        public IActionResult OnPostAdd()
        {
            result = _tasksService.AddTask(taskItem);
            if (result)
            {
                AddResult = $"Success! The task has been added.";
            }
            else
            {
                AddResult = $"Action failed.";
            }
            return RedirectToPage();
        }

        // Deletes task (if allowed by status)
        public IActionResult OnPostDelete()
        {
            result = _tasksService.DeleteTask(taskItem.Id);
            if (result)
            {
                DeleteResult = $"Success! The task has been deleted.";
            } else
            {
                DeleteResult = $"Action failed: Deletion was prevented because the status is 'completed' or null.";
            }
                return RedirectToPage();
        }

        // Assigns task to selected user
        public IActionResult OnPostAssign()
        {
            Users = _usersService.GetAllUsers();

            // Check if selected user exists
            var foundUser = Users.FirstOrDefault(u => u.Id == user.Id);
            if (foundUser == null)
            {
                ModelState.AddModelError("", "User not found");
                return Page();
            }

            var fullName = foundUser.FullName;
            result = _tasksService.AssignTask(user.Id, fullName, taskItem.Id);
            if (result)
            {
                AssignResult = $"Success! The task has been assigned.";
            }
            else
            {
                AssignResult = $"Action failed.";
            }
            return RedirectToPage();
        }
    }
}
