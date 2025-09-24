using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerDariaSt.Models;
using TaskManagerDariaSt.Services;

namespace TaskManagerDariaSt.Pages
{
    public class UpdateTaskModel : PageModel
    {
        // Injecting task service
        private readonly ITasksService _tasksService;

        public UpdateTaskModel(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }
        [BindProperty]
        public TaskItem taskItem { get; set; } = new();
        // Result flag for update action
        [BindProperty]
        public bool result { get; set; }

        // TempData message for UI feedback
        [TempData]
        public string? UpdateResult { get; set; }

        // Loads task data by Id when page opens
        public void OnGet(string id)
        {
            taskItem = _tasksService.GetTaskById(id);
        }

        // Handles task update logic
        public IActionResult OnPostUpdate()
        {
            result = _tasksService.UpdateTask(taskItem);
            if (result)
            {
                UpdateResult = $"Success! The task has been Updated.";

                // GPT helped: Reloads the same page with task Id to keep updated values visible
                return RedirectToPage(new { id = taskItem.Id });
            }
            else
            {
                UpdateResult = $"Action failed.";
                return Page();
            }
        }
    }
}
