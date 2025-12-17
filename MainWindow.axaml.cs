using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ProjectManager
{
    public partial class MainWindow : Window
    {
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

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            vm?.GenerateReport();
        }
    }
}