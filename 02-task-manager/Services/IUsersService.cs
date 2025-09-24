using TaskManagerDariaSt.Models;
namespace TaskManagerDariaSt.Services
{
    public interface IUsersService
    {
        public bool AddUser(User newUser);
        public bool DeleteUser(string id);
        public List<User> GetAllUsers();
    }
}
