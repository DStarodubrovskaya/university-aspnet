using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Session;
using TaskSchedulerDariaSt.Models;
using TaskSchedulerDariaSt.Services;

namespace TaskSchedulerDariaSt.Pages
{
    public class ExecuteTaskModel : PageModel
    {
        // Injecting session, task and user services
        private readonly ISessionService _sessionService;
        private readonly ITasksService _taskService;
        private readonly IUsersService _userService;
        public ExecuteTaskModel(ISessionService sessionService, ITasksService taskService, IUsersService userService)
        {
            _sessionService = sessionService;
            _taskService = taskService;
            _userService = userService;
        }

        [BindProperty]
        public TaskItem? taskItem { get; set; }
        
        public IActionResult OnGet(string id)
        {
            // Redirects unlogined user to Login page
            if (!_sessionService.Load()) return RedirectToPage("Index"); 

            // Loads task info from Session by id
            taskItem = _taskService.GetTaskById(_sessionService.firstTask, id);

            // Prevents Errors
            if (taskItem == null) return RedirectToPage("ManageTasks"); 

            return Page();
        }

        public async Task<IActionResult> OnPostExecuteAsync(string id)
        {
            // Redirects unlogined user to Login page
            if (!_sessionService.Load()) return RedirectToPage("Index");

            // Loads task info from Session by id
            taskItem = _taskService.GetTaskById(_sessionService.firstTask, id);
            // Prevents Errors
            if (taskItem == null) return RedirectToPage("ManageTasks");

            // Simulates execution
            await Task.Delay(TimeSpan.FromSeconds(taskItem.EstimatedTime));

            // Stores completed tasks in a "log journal"
            var newLog = $"Task {taskItem?.Name} was executed by {_sessionService.currentUser.UserName} at {DateTime.UtcNow}\n";
            System.IO.File.AppendAllText("ExecutedTasks.txt", newLog);

            // Updates session and DB after execution
            _sessionService.firstTask = _taskService.RemoveTaskById(_sessionService.firstTask, id); // Removes the task from session
            _userService.UpdateTasks(_sessionService.currentUser.Id, _sessionService.firstTask); /// Updates DB

            // Feedback to user (for alert in ManageTasks)
            TempData["ExecuteResult"] = $"Task {taskItem.Name} executed successfully!";

            return RedirectToPage("ManageTasks");
        }

    }
}
