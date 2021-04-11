using System;

namespace SimpleTask.Models
{
    public class TaskModel
    {
        public Guid TaskId { get; set; }
        public Guid AssignedUser { get; set; }
        public string TaskName { get; set; }
    }
}