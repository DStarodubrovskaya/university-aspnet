namespace TaskManagerDariaSt.Models
{
    public class TaskManagerDatabaseSettings
    {
        public string? UsersCollectionName { get; set; } = null!;
        public string? TasksCollectionName { get; set; } = null!;
        public string? ConnectionString { get; set; } = null!;
        public string? DatabaseName { get; set; } = null!;
    }
}
