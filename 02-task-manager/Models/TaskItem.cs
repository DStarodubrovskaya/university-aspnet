using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TaskManagerDariaSt.Models
{
    public class TaskItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; } = "-";
        public string Status { get; set; } = "Not started";
        public DateTime DueDate { get; set; }
    }
}
