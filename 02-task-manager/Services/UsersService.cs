using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskManagerDariaSt.Models;

namespace TaskManagerDariaSt.Services
{
    public class UsersService : IUsersService
    {
        // Connecting to MongoDB users collection
        private readonly IMongoCollection<User> _usersCollection;
        public UsersService(
            IOptions<TaskManagerDatabaseSettings> taskManagerDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                taskManagerDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                taskManagerDatabaseSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(
                taskManagerDatabaseSettings.Value.UsersCollectionName);
        }
        // METHODS
        // Adds new user to DB
        public bool AddUser(User newUser)
        {
            try
            {
                _usersCollection.InsertOne(newUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            return true;
        }
        // Deletes user by Id
        public bool DeleteUser(string id)
        {
            try
            {
                _usersCollection.DeleteOne(x => x.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            return true;
        }
        // Returns full user list
        public List<User> GetAllUsers()
        {
            return _usersCollection.Find(_ => true).ToList();
        }
    }
}
