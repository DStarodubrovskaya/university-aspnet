using TaskSchedulerDariaSt.Models;

namespace TaskSchedulerDariaSt.Services
{
    // Here are only Recursive Functions - as a part of the main assingment
    public class TasksService : ITasksService
    { 
        public Node<TaskItem>? AddTask(Node<TaskItem>? first, TaskItem newTask)
        {
            // Base 1 - in case the List is empty
            if (first == null)
            {
                return new Node<TaskItem>(newTask); // new first
            }
            // Base 2
            if (newTask.Priority < first.GetValue().Priority)
            {
                Node<TaskItem> task = new Node<TaskItem>(newTask);
                task.SetNext(first);
                return task; // new first
            }
            // Recursion
            first.SetNext(AddTask(first.GetNext(), newTask));
            return first;
        }
        public List<TaskItem> PrintTasks(Node<TaskItem>? first)
        {
            List<TaskItem> result = new List<TaskItem>();
            CollectTasks(first, result);
            return result;
        }
        private void CollectTasks(Node<TaskItem>? first, List<TaskItem> taskList)
        {
            if (first == null) return; // Base
            taskList.Add(first.GetValue());
            CollectTasks(first.GetNext(), taskList); // Recursion
        }
        public List<TaskItem> FilterTasks(Node<TaskItem>? first, int priority)
        {
            List<TaskItem> result = new List<TaskItem>();
            CollectTasksByFilter(first, result, priority);
            return result;
        }
        private void CollectTasksByFilter(Node<TaskItem>? first, List<TaskItem> taskList, int priority)
        {
            if (first == null) return; // Base
            
            if (first.GetValue().Priority == priority)
            {
                taskList.Add(first.GetValue());
            }
            CollectTasksByFilter(first.GetNext(), taskList, priority); // Recursion
        }
        public int GetTotalTime(Node<TaskItem>? first)
        {
            if (first == null) return 0;
            int sum = first.GetValue().EstimatedTime + GetTotalTime(first.GetNext());
            return sum;
        }
        // This Remove Function is for assingment's requirement to delete THE FIRST task with matching name
        public Node<TaskItem>? RemoveTaskByName(Node<TaskItem>? first, string name)
        {
            if (first == null) { return null; } // Base 1 - if there is no node

            if (first.GetValue().Name == name)
            {
                return first.GetNext();
            } // Base 2 - removes the node

            first.SetNext(RemoveTaskByName(first.GetNext(), name)); // Recursion
            return first;
        }
        // The difference with previous one is in the increased probability of deleting exactly the task that needs to be deleted
        public Node<TaskItem>? RemoveTaskById(Node<TaskItem>? first, string id)
        {
            if (first == null) { return null; } // Base 1 - if there is no node

            if (first.GetValue().Id == id)
            {
                return first.GetNext();
            } // Base 2 - removes the node

            first.SetNext(RemoveTaskById(first.GetNext(), id)); // Recursion
            return first;
        }
        // This function looks up a node by task Id in the linked list 
        // It's used for printing details while execution
        public TaskItem? GetTaskById(Node<TaskItem>? first, string id)
        {
            if (first == null) { return null; } // Base 1 - if there is no node

            TaskItem task = first.GetValue();
            if (task.Id == id)
            {
                return task;
            } // Base 2 - returns the task

            task = GetTaskById(first.GetNext(), id); // Recursion
            return task;
        }
    }
}
