using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Globalization;

namespace ProjectManager
{
    public partial class MainWindow : Window
    {
        public static readonly FuncValueConverter<bool, IBrush> CompletedBackgroundConverter =
            new FuncValueConverter<bool, IBrush>(isCompleted =>
                isCompleted ? new SolidColorBrush(Color.FromRgb(200, 230, 201)) : Brushes.White);

        public static readonly FuncValueConverter<string, bool> StringNotEmptyConverter =
            new FuncValueConverter<string, bool>(str => !string.IsNullOrWhiteSpace(str));

        public static readonly FuncValueConverter<object, bool> NotNullConverter =
            new FuncValueConverter<object, bool>(obj => obj != null);

        public static readonly FuncValueConverter<object, bool> NullConverter =
            new FuncValueConverter<object, bool>(obj => obj == null);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            vm?.AddProject();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            vm?.AddTask();
        }

        private void ToggleTask_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            vm?.ToggleTaskCompletion();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            vm?.DeleteTask();
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            vm?.GenerateReport();
        }
    }
}