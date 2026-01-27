using Labb3.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Labb3
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowViewModel();
            mainWindow.Show();
        }
    }

}
