using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using TaskSchedulerDariaSt.Models;

namespace TaskSchedulerDariaSt.Services
{
    // Here is a "bonus part" - not recursive functions, Mongo interactions..
    public class UsersService : IUsersService
    {
        // Connecting to MongoDB users collection
        private readonly IMongoCollection<User> _usersCollection;
        public UsersService(
            IOptions<TaskSchedulerDatabaseSettings> taskSchedulerDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                taskSchedulerDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                taskSchedulerDatabaseSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(
                taskSchedulerDatabaseSettings.Value.UsersCollectionName);
        }
        // METHODS
        // Adds new user to DB
        public bool AddUser(User newUser)
        {
            if (CheckIfExist(newUser.UserName))
            {
                return false;
            }
                    
            _usersCollection.InsertOne(newUser);
            return true;
        }
        private bool CheckIfExist(string name)
        {
            User user = _usersCollection.Find(x => x.UserName == name).FirstOrDefault();
            if (user == null)
            {
                return false;
            } // In case user doesn't exist
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
        public User? GetUserById(string id)
        {
            User user = _usersCollection.Find(x => x.Id == id).FirstOrDefault();
            if (user == null)
            {
                Console.WriteLine("Error: User not found");
            } // In case user doesn't exist
            return user;
        }
        // Used for Load Session
        public Node<TaskItem>? GetTasksByUser(string id)
        {
            User user = _usersCollection.Find(x => x.Id == id).FirstOrDefault();
            if (user == null)
            {
                Console.WriteLine("Error: User not found");
                return null;

            } // In case user doesn't exist

            // Sends to convert regular List to Linked List
            return ListToNode(user.Tasks);
        }
        // Converts: List --> Linked List
        private Node<TaskItem>? ListToNode(List<TaskItem> tasks)
        {
            if (tasks == null || tasks.Count == 0)
                return null;

            Node<TaskItem> first = new Node<TaskItem>(tasks[0]);
            Node<TaskItem> current = first;

            for (int i = 1; i < tasks.Count; i++)
            {
                Node<TaskItem> nextTask = new Node<TaskItem>(tasks[i]);
                current.SetNext(nextTask);
                current = nextTask;
            }
            return first;
        }
        // Converts: Linked List --> List
        private List<TaskItem> NodeToList(Node<TaskItem>? first)
        {
            List<TaskItem> tasks = new List<TaskItem>();
            Node<TaskItem> current = first;
            while (current != null) 
            {
                tasks.Add(current.GetValue());
                current = current.GetNext(); 
            }
            return tasks;
        }
        public bool UpdateTasks(string id, Node<TaskItem>? first)
        {
            try
            {
                // Looks up the right document
                User user = _usersCollection.Find(t => t.Id == id).FirstOrDefault();
                
                
                if (user == null)
                {
                    Console.WriteLine("Error: User not found for update");
                    return false;
                } // Prevents errors

                // Updates the tasks' attribute by converting linked list to a regular one
                user.Tasks = NodeToList(first);
                // Saves current time
                user.LastUpdate = DateTime.UtcNow;

                // Builds a filter
                var filter = Builders<User>.Filter.Eq(t => t.Id, id);
                // Updates DB
                _usersCollection.ReplaceOne(filter, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            return true;
        }
    }
}
