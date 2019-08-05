using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace testLists
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string date = "10/10/2018";
            DateTime endDate = DateTime.Parse(date);
            DateTime today = DateTime.Today;
            
            if(today > endDate)
            {
                MessageBox.Show("Непредвиденная Ошибка!\nЗапуск приложения невозможен!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            } 
        }
    }
}
