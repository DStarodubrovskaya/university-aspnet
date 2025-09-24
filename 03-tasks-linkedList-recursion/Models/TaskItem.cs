namespace TaskSchedulerDariaSt.Models
{
    public class TaskItem
    {
        // GPT help: To ensure that each task has a unique ID automatically generated
        public string Id { get; set; } = Guid.NewGuid().ToString(); // for execution
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Priority { get; set; }
        public int EstimatedTime { get; set; }
     }
}
