using DnsClient.Protocol;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TaskManagerDariaSt.Models;

namespace TaskManagerDariaSt.Services
{
    public class TasksService : ITasksService
    {
        // Connecting to MongoDB collection
        private readonly IMongoCollection<TaskItem> _tasksCollection;

        public TasksService(
            IOptions<TaskManagerDatabaseSettings> taskManagerDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                taskManagerDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                taskManagerDatabaseSettings.Value.DatabaseName);

            _tasksCollection = mongoDatabase.GetCollection<TaskItem>(
                taskManagerDatabaseSettings.Value.TasksCollectionName);
        }
        // METHODS
        // Adds new task to DB
        public bool AddTask(TaskItem newTask)
        {
            try
            {
                _tasksCollection.InsertOne(newTask);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            return true;
        }
        // Assigns task to specific user by Id and name
        public bool AssignTask(string idUser, string nameUser, string idTask)
        {
            try
            {
                var existingTask = _tasksCollection.Find(t => t.Id == idTask).FirstOrDefault();
                if (existingTask == null)
                {
                    Console.WriteLine("Error: Task not found for assignment");
                    return false;
                }

                var filter = Builders<TaskItem>.Filter.Eq(t => t.Id, idTask);
                var update = Builders<TaskItem>.Update.Set(t => t.AssignedToUserId, idUser).Set(t => t.AssignedToUserName, nameUser);
                _tasksCollection.UpdateOne(filter, update);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            return true;
        }

        // Removes user assignment from all tasks
        public bool UnAssignTask(string idUser)
        {
            try
            {
                var filter = Builders<TaskItem>.Filter.Eq(t => t.AssignedToUserId, idUser);
                var update = Builders<TaskItem>.Update.Set(t => t.AssignedToUserId, null).Set(t => t.AssignedToUserName, "-");
                _tasksCollection.UpdateMany(filter, update);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            return true;
        }
        // Gets tasks assigned to specific user
        public List<TaskItem> DisplayTasksByUser(string idUser)
        {
            return _tasksCollection.Find(x => x.AssignedToUserId == idUser).ToList();
        }

        // Gets tasks by status (Not started / In progress / Completed)
        public List<TaskItem> DisplayTasksByStatus(string status)
        {
            return _tasksCollection.Find(x => x.Status == status).ToList();
        }

        // Gets tasks by both user and status
        public List<TaskItem> DisplayTasksByUserAndStatus(string idUser, string status)
        {
            return _tasksCollection.Find(x => x.AssignedToUserId == idUser && x.Status == status).ToList();
        }

        // Updates task details (fully replaces document)
        public bool UpdateTask(TaskItem updatedTask)
        {
            try
            {
                var existingTask = _tasksCollection.Find(t => t.Id == updatedTask.Id).FirstOrDefault();
                if (existingTask == null)
                {
                    Console.WriteLine("Error: Task not found for update");
                    return false;
                }

                var filter = Builders<TaskItem>.Filter.Eq(t => t.Id, updatedTask.Id);
                _tasksCollection.ReplaceOne(filter, updatedTask);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            return true;
        }


        // Deletes task if status allows
        public bool DeleteTask(string idTask)
        {
            try
            {
                TaskItem task = _tasksCollection.Find(x => x.Id == idTask).FirstOrDefault();
                if (task == null)
                {
                    Console.WriteLine("Error: Task not found");
                    return false;
                }
                else if (!string.IsNullOrEmpty(task.Status) && task.Status.ToLower() != "completed")
                {
                    _tasksCollection.DeleteOne(x => x.Id == idTask);
                } 
                else
                {
                    Console.WriteLine("Error: Deletion was prevented because the status is 'completed' or null");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            return true;
        }

        // Returns all tasks
        public List<TaskItem> GetAllTasks()
        {
            return _tasksCollection.Find(_ => true).ToList();
        }

        // Returns task by Id
        public TaskItem GetTaskById(string idTask)
        {
            return _tasksCollection.Find(x => x.Id == idTask).FirstOrDefault();
        }
    }
}
