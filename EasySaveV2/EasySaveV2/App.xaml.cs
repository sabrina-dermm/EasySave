using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EasySaveV2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex mutex = null;
        public bool singleInstanceValidated = false;

        const string mutex_name = "EasySaveV2";
        public App()
        {
            mutex = new Mutex(true, mutex_name, out singleInstanceValidated); 
            if (!singleInstanceValidated)
            {
                MessageBox.Show("This program is already running", "Instantiation error", MessageBoxButton.OK, MessageBoxImage.Error); Application.Current.Shutdown();
                return;
            }
            singleInstanceValidated = true;
        }    
    }
}
