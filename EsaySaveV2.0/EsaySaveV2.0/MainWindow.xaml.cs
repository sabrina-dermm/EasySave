using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EsaySaveV2._0.Model;
using EsaySaveV2._0.Controller;
using EsaySaveV2._0.View;
namespace EsaySaveV2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Controler controller = new Controler();
        }

        private void LunchSaveButton(object sender, RoutedEventArgs e)
        {
            DataContext = new LunchSaveInterface();
        }

       
        private void CreateSaveButton(object sender, RoutedEventArgs e)
        {
            DataContext = new CreateSaveInterface();
            
        }

        private void ModifySaveButton(object sender, RoutedEventArgs e)
        {

        }
    }
}
