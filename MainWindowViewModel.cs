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
        private string _newTaskDescription = "";
        private string _newTaskAssignee = "";
        private string _reportText = "";

        // Поля для редактирования
        private string _editTaskTitle = "";
        private string _editTaskDescription = "";
        private string _editTaskAssignee = "";
        private DateTime _editTaskDueDate = DateTime.Now;
        private int _editTaskPriorityIndex = 1;

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
                LoadTaskForEdit();
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

        public string NewTaskDescription
        {
            get => _newTaskDescription;
            set { _newTaskDescription = value; OnPropertyChanged(); }
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

        // Свойства для редактирования
        public string EditTaskTitle
        {
            get => _editTaskTitle;
            set { _editTaskTitle = value; OnPropertyChanged(); }
        }

        public string EditTaskDescription
        {
            get => _editTaskDescription;
            set { _editTaskDescription = value; OnPropertyChanged(); }
        }

        public string EditTaskAssignee
        {
            get => _editTaskAssignee;
            set { _editTaskAssignee = value; OnPropertyChanged(); }
        }

        public DateTime EditTaskDueDate
        {
            get => _editTaskDueDate;
            set { _editTaskDueDate = value; OnPropertyChanged(); }
        }

        public int EditTaskPriorityIndex
        {
            get => _editTaskPriorityIndex;
            set { _editTaskPriorityIndex = value; OnPropertyChanged(); }
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

        public void DeleteProject()
        {
            if (SelectedProject != null)
            {
                Projects.Remove(SelectedProject);
                SelectedProject = null;
                CurrentTasks.Clear();
            }
        }

        public void AddTask()
        {
            if (SelectedProject != null && !string.IsNullOrWhiteSpace(NewTaskTitle))
            {
                var task = new Task
                {
                    Title = NewTaskTitle,
                    Description = NewTaskDescription ?? "",
                    AssignedTo = NewTaskAssignee ?? "Не назначено",
                    DueDate = DateTime.Now,
                    Priority = TaskPriority.Medium
                };
                SelectedProject.Tasks.Add(task);
                CurrentTasks.Add(task);
                NewTaskTitle = string.Empty;
                NewTaskDescription = string.Empty;
                NewTaskAssignee = string.Empty;
            }
        }

        public void ToggleTaskCompletion()
        {
            if (SelectedTask != null && SelectedProject != null)
            {
                SelectedTask.IsCompleted = !SelectedTask.IsCompleted;
                SelectedProject.NotifyProgressChanged();
            }
        }

        public void DeleteTask()
        {
            if (SelectedTask != null && SelectedProject != null)
            {
                SelectedProject.Tasks.Remove(SelectedTask);
                CurrentTasks.Remove(SelectedTask);
                SelectedProject.NotifyProgressChanged();
                SelectedTask = null;
            }
        }

        public void SaveTask()
        {
            if (SelectedTask != null && !string.IsNullOrWhiteSpace(EditTaskTitle))
            {
                SelectedTask.Title = EditTaskTitle;
                SelectedTask.Description = EditTaskDescription ?? "";
                SelectedTask.AssignedTo = EditTaskAssignee;
                SelectedTask.DueDate = EditTaskDueDate;
                SelectedTask.Priority = (TaskPriority)EditTaskPriorityIndex;

                SelectedProject?.NotifyProgressChanged();
                OnPropertyChanged(nameof(CurrentTasks));
            }
        }

        private void LoadTaskForEdit()
        {
            if (SelectedTask != null)
            {
                EditTaskTitle = SelectedTask.Title;
                EditTaskDescription = SelectedTask.Description;
                EditTaskAssignee = SelectedTask.AssignedTo;
                EditTaskDueDate = SelectedTask.DueDate;
                EditTaskPriorityIndex = (int)SelectedTask.Priority;
            }
            else
            {
                EditTaskTitle = "";
                EditTaskDescription = "";
                EditTaskAssignee = "";
                EditTaskDueDate = DateTime.Now;
                EditTaskPriorityIndex = 1;
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

                if (project.Tasks.Any())
                {
                    sb.AppendLine("\nЗадачи:");
                    foreach (var task in project.Tasks)
                    {
                        sb.AppendLine($"  • {task.Title} [{(task.IsCompleted ? "✓" : " ")}]");
                        if (!string.IsNullOrWhiteSpace(task.Description))
                            sb.AppendLine($"    Описание: {task.Description}");
                        sb.AppendLine($"    Исполнитель: {task.AssignedTo} | Срок: {task.DueDate:dd.MM.yyyy} | Приоритет: {task.Priority}");
                    }
                }

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