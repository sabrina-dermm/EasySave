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

namespace EasySaveV2.View
{
    /// <summary>
    /// Logique d'interaction pour CreateSaveView.xaml
    /// </summary>
    public partial class CreateSaveView : UserControl
    {
        public CreateSaveView()
        {
            InitializeComponent();
        }

        private void LunchClick(object sender, RoutedEventArgs e)
        {
            pauseButton.Visibility = Visibility.Visible;
            stopButton.Visibility = Visibility.Visible;
        }

        private void PauseClick(object sender, RoutedEventArgs e)
        {
            pauseButton.Visibility = Visibility.Collapsed;
            playButton.Visibility = Visibility.Visible;
        }

        private void ContinueClick(object sender, RoutedEventArgs e)
        {
            playButton.Visibility = Visibility.Collapsed;
            pauseButton.Visibility = Visibility.Visible;
        }

        private void StopClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
