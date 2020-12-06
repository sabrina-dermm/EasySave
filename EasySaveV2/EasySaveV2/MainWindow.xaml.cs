using EasySaveV2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace EasySaveV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ControllerViewModel c;
        public MainWindow()
        {
            InitializeComponent();
            c = new ControllerViewModel();
            this.DataContext = c;
        }

        private void SwitchToCreateSaveView(object sender, RoutedEventArgs e)
        {
            setActiveUserControl(CreateSaveV);

        }

        private void HitMe(object sender, RoutedEventArgs e)
        {
            setActiveUserControl(EmployeeV);
        }
        public void setActiveUserControl(UserControl control)
        {
            //Collapse all the usercontrol
           
            Welcome.Visibility = Visibility.Collapsed;
            EmployeeV.Visibility = Visibility.Collapsed;
            CreateSaveV.Visibility = Visibility.Collapsed;
            //show the current user control
            control.Visibility = Visibility.Visible;
        }
    }
}
