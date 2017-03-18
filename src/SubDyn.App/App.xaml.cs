using System;
using System.Windows;

namespace SubDyn.App
{
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            var view = new MainWindow();
            view.Show();
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
        }
    }
}
