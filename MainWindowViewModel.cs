using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProjectManager
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Project _selectedProject;
        private Task _selectedTask;
        private string _newProjectName = "";
        private string _newTaskTitle = "";
        private string _newTaskAssignee = "";
        private string _reportText = "";

        public ObservableCollection<Project> Projects { get; set; } = new();
        public ObservableCollection<Task> CurrentTasks { get; set; } = new();

        public Project SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                OnPropertyChanged();
                LoadTasks();
            }
        }

        public Task SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged();
            }
        }

        public string NewProjectName
        {
            get => _newProjectName;
            set { _newProjectName = value; OnPropertyChanged(); }
        }

        public string NewTaskTitle
        {
            get => _newTaskTitle;
            set { _newTaskTitle = value; OnPropertyChanged(); }
        }

        public string NewTaskAssignee
        {
            get => _newTaskAssignee;
            set { _newTaskAssignee = value; OnPropertyChanged(); }
        }

        public string ReportText
        {
            get => _reportText;
            set { _reportText = value; OnPropertyChanged(); }
        }

        public void AddProject()
        {
            if (!string.IsNullOrWhiteSpace(NewProjectName))
            {
                Projects.Add(new Project
                {
                    Name = NewProjectName,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(1)
                });
                NewProjectName = string.Empty;
            }
        }

        public void AddTask()
        {
            if (SelectedProject != null && !string.IsNullOrWhiteSpace(NewTaskTitle))
            {
                var task = new Task
                {
                    Title = NewTaskTitle,
                    AssignedTo = NewTaskAssignee ?? "Не назначено",
                    DueDate = DateTime.Now.AddDays(7),
                    Priority = TaskPriority.Medium
                };
                SelectedProject.Tasks.Add(task);
                CurrentTasks.Add(task);
                NewTaskTitle = string.Empty;
                NewTaskAssignee = string.Empty;
            }
        }

        public void ToggleTaskCompletion()
        {
            if (SelectedTask != null && SelectedProject != null)
            {
                SelectedTask.IsCompleted = !SelectedTask.IsCompleted;

                // Уведомляем проект об изменении прогресса
                SelectedProject.NotifyProgressChanged();
            }
        }

        public void GenerateReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== ОТЧЁТ ПО ПРОЕКТАМ ===\n");

            foreach (var project in Projects)
            {
                sb.AppendLine($"Проект: {project.Name}");
                sb.AppendLine($"Период: {project.StartDate:dd.MM.yyyy} - {project.EndDate:dd.MM.yyyy}");
                sb.AppendLine($"Прогресс: {project.Progress}%");
                sb.AppendLine($"Всего задач: {project.Tasks.Count}");
                sb.AppendLine($"Завершено: {project.Tasks.Count(t => t.IsCompleted)}");
                sb.AppendLine($"В работе: {project.Tasks.Count(t => !t.IsCompleted)}");
                sb.AppendLine();
            }

            ReportText = sb.ToString();
        }

        private void LoadTasks()
        {
            CurrentTasks.Clear();
            if (SelectedProject != null)
            {
                foreach (var task in SelectedProject.Tasks)
                {
                    CurrentTasks.Add(task);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}