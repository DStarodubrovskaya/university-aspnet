using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskSchedulerDariaSt.Models;
using TaskSchedulerDariaSt.Services;

namespace TaskSchedulerDariaSt.Pages
{
    public class ManageTasksModel : PageModel
    {
        // Injecting session, task and user services
        private readonly ISessionService _sessionService;
        private readonly ITasksService _taskService;
        private readonly IUsersService _userService;
        public ManageTasksModel(ISessionService sessionService, ITasksService taskService, IUsersService userService)
        {
            _sessionService = sessionService;
            _taskService = taskService;
            _userService = userService;
        }
        
        // Full Task list for dropdown
        public List<TaskItem> Tasks { get; set; } = new();
        public List<string> UniqueTaskNames { get; set; } = new();
        // I need UniqueTaskNames for Delete Function
        // cause I have to delete "the first task with matching name"
        // so I don't want to confuse the user showing double names
        
        [BindProperty]
        public TaskItem taskItem { get; set; } = new();
        
        public User? currentUser { get; set; }

        [BindProperty]
        public int priorityFilter { get; set; } = 0;

        // TempData messages for UI feedback
        [TempData] public string? AddResult { get; set; }
        [TempData] public string? DeleteResult { get; set; }
        [TempData] public string? TimeResult { get; set; }
        [TempData] public string? ExecuteResult { get; set; }


        public IActionResult OnGet()
        {
            if (!_sessionService.Load()) return RedirectToPage("Index"); // Redirects unlogined user to Login page
           
            currentUser = _sessionService.currentUser; // for UI greeting

            // Loads all tasks for UI - displaying list of tasks
            if (_sessionService.firstTask != null)
            {

                // Retriving all tasks using the first one
                Tasks = _taskService.PrintTasks(_sessionService.firstTask);

                // GPT Help: extracting unique task names from list to another
                UniqueTaskNames = Tasks.Select(t => t.Name)
                                    .Where(n => !string.IsNullOrWhiteSpace(n))
                                    .Distinct(StringComparer.OrdinalIgnoreCase)
                                    .OrderBy(n => n)
                                    .ToList();
            }
            else
            {
                Tasks = new List<TaskItem>();
            }

            return Page();
        }

        // Adds new task
        public IActionResult OnPostAdd()
        {
            // Redirects unlogined user to Login page
            if (!_sessionService.Load()) return RedirectToPage("Index");
            currentUser = _sessionService.currentUser; // for UI greeting

            // Adds task to linked list - recursive method
            _sessionService.firstTask = _taskService.AddTask(_sessionService.firstTask, taskItem);
            
            // Updates DB
            _userService.UpdateTasks(_sessionService.currentUser.Id, _sessionService.firstTask);

            // Feedback
            AddResult = $"Task Added.";
            return RedirectToPage();
        }
        
        // Deletes task
        public IActionResult OnPostDelete()
        {
            // Redirects unlogined user to Login page
            if (!_sessionService.Load()) return RedirectToPage("Index");
            currentUser = _sessionService.currentUser; // for UI greeting

            if (string.IsNullOrWhiteSpace(taskItem?.Name))
            {
                DeleteResult = "Please pick a task name.";
                return RedirectToPage();
            } // if nothing was selected

            // Removes task from linked list - recursive method
            _sessionService.firstTask = _taskService.RemoveTaskByName(_sessionService.firstTask, taskItem.Name);
           
            // Updates DB
            _userService.UpdateTasks(_sessionService.currentUser.Id, _sessionService.firstTask);
            
            // Feedback
            DeleteResult = $"Task Removed.";
            return RedirectToPage();
        }

        // Display Tasks (all or by priority)
        public IActionResult OnPostFilter()
        {
            // Redirects unlogined user to Login page
            if (!_sessionService.Load()) return RedirectToPage("Index");
            currentUser = _sessionService.currentUser; // for UI greeting

            if (priorityFilter != 0)
            {
                Tasks = _taskService.FilterTasks(_sessionService.firstTask, priorityFilter);
            } // Filters by priority
            else
            {
                Tasks = _taskService.PrintTasks(_sessionService.firstTask);
            } // Prints all tasks

            // GPT Help: extracting unique task names from list to another
            UniqueTaskNames = Tasks.Select(t => t.Name)
                                .Where(n => !string.IsNullOrWhiteSpace(n))
                                .Distinct(StringComparer.OrdinalIgnoreCase)
                                .OrderBy(n => n)
                                .ToList();
            return Page();
        }

        // Shows estimated time
        public IActionResult OnPostGetTime()
        {
            // Redirects unlogined user to Login page
            if (!_sessionService.Load()) return RedirectToPage("Index");
            currentUser = _sessionService.currentUser; // for UI greeting

            // Feedback
            TimeResult = $"All tasks will take {_taskService.GetTotalTime(_sessionService.firstTask)} sec";

            return RedirectToPage();
        }
        
    }
}
