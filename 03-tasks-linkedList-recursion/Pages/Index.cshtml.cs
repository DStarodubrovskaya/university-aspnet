using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskSchedulerDariaSt.Models;
using TaskSchedulerDariaSt.Services;

namespace TaskSchedulerDariaSt.Pages
{
    public class IndexModel : PageModel
    {
        // Injecting session & user services
        public readonly ISessionService _sessionService;
        public readonly IUsersService _userService;
        public IndexModel(ISessionService sessionService, IUsersService userService)
        {
            _sessionService = sessionService;
            _userService = userService;
        }

        [BindProperty]
        public User? user { get; set; }

        // Full user list for dropdown
        public List<User> Users { get; set; } = new();

        public void OnGet()
        {
            // Users' list used for select dropdown
            Users = _userService.GetAllUsers();
        }
        public IActionResult OnPostSelect()
        {
            if (user == null) return RedirectToPage(); // if nothing was selected
            
            // Finds user in DB by Id
            var currentUser = _userService.GetUserById(user.Id);
            
            // Stores current user in Session
            _sessionService.SetUser(currentUser);

            return RedirectToPage("ManageTasks");
        }
    }
}
