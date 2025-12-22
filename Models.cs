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
        private string _title = "";
        private string _description = "";
        private string _assignedTo = "";
        private DateTime _dueDate = DateTime.Now;
        private TaskPriority _priority;

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public string AssignedTo
        {
            get => _assignedTo;
            set
            {
                if (_assignedTo != value)
                {
                    _assignedTo = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                if (_dueDate != value)
                {
                    _dueDate = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public TaskPriority Priority
        {
            get => _priority;
            set
            {
                if (_priority != value)
                {
                    _priority = value;
                    OnPropertyChanged();
                }
            }
        }

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