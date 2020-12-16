using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace EasySaveV2.View
{
    /// <summary>
    /// Logique d'interaction pour Acceuil.xaml
    /// </summary>
    public partial class Acceuil : UserControl
    {
        public Acceuil()
        {
            InitializeComponent();
        }
        private void openStateFileClick(object sender, RoutedEventArgs e)
        {
            Process p = new Process();
            Process.Start("Notepad++");
        }
    }
}
