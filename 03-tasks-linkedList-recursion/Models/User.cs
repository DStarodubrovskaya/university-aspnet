using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using TaskSchedulerDariaSt.Models;

namespace TaskSchedulerDariaSt.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; } = null!;
        public List<TaskItem> Tasks { get; set; } = new();
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }
}
