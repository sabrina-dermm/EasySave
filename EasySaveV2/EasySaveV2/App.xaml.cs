using SingleInstanceCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EasySaveV2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstance
    {
        public void OnInstanceInvoked(string[] args)
        {
            //What to do with the args another instance has sent
        }
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			bool isFirstInstance = SingleInstance<App>.InitializeAsFirstInstance("EasySaveV2");
			if (!isFirstInstance)
			{
				//If it's not the first instance, arguments are automatically passed to the first instance
				//OnInstanceInvoked will be raised on the first instance
				//You may shut down the current instance
				Current.Shutdown();
			}
		}

		private void Application_Exit(object sender, ExitEventArgs e)
		{
			SingleInstance<App>.Cleanup();
		}
	}
}
