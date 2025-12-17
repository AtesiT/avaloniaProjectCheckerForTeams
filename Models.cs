using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManager
{
    public class Project
    {
        public string Name { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Task> Tasks { get; set; } = new();
        public int Progress => Tasks.Count > 0 ? (int)(Tasks.Count(t => t.IsCompleted) * 100.0 / Tasks.Count) : 0;
    }

    public class Task
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string AssignedTo { get; set; } = "";
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public TaskPriority Priority { get; set; }
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}