using System.Windows;

namespace WpfCalculatorKata
{
    public partial class MainWindow : Window
    {
        public MainWindow(AdderViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
