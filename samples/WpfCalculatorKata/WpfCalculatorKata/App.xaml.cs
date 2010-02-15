using System.Windows;

namespace WpfCalculatorKata
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow(new AdderViewModel(new ConreteAdder()));
            window.Show();
        }
    }
}
