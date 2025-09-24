using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using TaskSchedulerDariaSt.Models;
using TaskSchedulerDariaSt.Services;

namespace TaskSchedulerDariaSt.Pages
{
    public class AddUserModel : PageModel
    {
        // Injecting user services
        public readonly IUsersService _userService;
        public AddUserModel(IUsersService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public User? user { get; set; } = new User();
        public List<User> Users { get; set; }

        // TempData messages for UI feedback
        [TempData] public string? AddUserResult { get; set; }
        [TempData] public string? DeleteUserResult { get; set; }

        public void OnGet()
        {
            // Users' list used for delete dropdown
            Users = _userService.GetAllUsers();
        }

        public IActionResult OnPostAdd()
        { 
            // Adds user
            var result = _userService.AddUser(user);
            // Feedback to user
            if (result)
            {
                AddUserResult = $"Success! The user has been added.";
            }
            else
            {
                AddUserResult = $"The user with that name already exists.\nTry another name.";
            }
            return RedirectToPage();
        }
        // Deletes user
        public IActionResult OnPostDelete()
        {
            if (string.IsNullOrWhiteSpace(user?.Id))
            {
                DeleteUserResult = "Please pick a user name.";
                return RedirectToPage();
            } // if nothing was selected

            // Deletes user
            var result = _userService.DeleteUser(user.Id);

            // Feedback to user
            if (result)
            {
                DeleteUserResult = $"Success! The user has been deleted.";
            }
            else
            {
                DeleteUserResult = $"Action failed.";
            }
            return RedirectToPage();
        }
    }
}
