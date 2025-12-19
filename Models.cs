using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ProjectManager
{
    public class Project : INotifyPropertyChanged
    {
        public string Name { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Task> Tasks { get; set; } = new();

        public int Progress => Tasks.Count > 0 ? (int)(Tasks.Count(t => t.IsCompleted) * 100.0 / Tasks.Count) : 0;

        public void NotifyProgressChanged()
        {
            OnPropertyChanged(nameof(Progress));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Task : INotifyPropertyChanged
    {
        private bool _isCompleted;

        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string AssignedTo { get; set; } = "";
        public DateTime DueDate { get; set; }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged();
                }
            }
        }

        public TaskPriority Priority { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}