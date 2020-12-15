using EasySaveV2.ViewModel;
using System.Windows;
using System.Windows.Controls;


namespace EasySaveV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int langue =0;
        ControllerViewModel c;
        public MainWindow()
        {
            InitializeComponent();
            c = new ControllerViewModel();
            this.DataContext = c;
        }

        private void SwitchToCreateSaveView(object sender, RoutedEventArgs e)
        {
            if (langue == 0)
            {
                setActiveUserControl(CreateSaveV);

            }
            else
            {
                setActiveUserControl(CreerSauvV);

            }
        }
        
        private void HitMe(object sender, RoutedEventArgs e)
        {
            
            if(langue == 0)
            {
                setActiveUserControl(Welcome);
            }
            else
            {
                setActiveUserControl(AcceuilV);
            }
        }
        public void setActiveUserControl(UserControl control)
        {
            //Collapse all the usercontrol
           
            Welcome.Visibility = Visibility.Collapsed;
            CreateSaveV.Visibility = Visibility.Collapsed;
            LunchV.Visibility = Visibility.Collapsed;
            CryptV.Visibility = Visibility.Collapsed;
            AcceuilV.Visibility = Visibility.Collapsed;
            CreerSauvV.Visibility = Visibility.Collapsed;
            LancerSauvV.Visibility = Visibility.Collapsed;
            CrypterV.Visibility = Visibility.Collapsed;
            //show the current user control
            control.Visibility = Visibility.Visible;
        }

        private void SwitchToLunchAll(object sender, RoutedEventArgs e)
        {
            if(langue == 0)
            {
                setActiveUserControl(LunchV);
            }
            else
            {
                setActiveUserControl(LancerSauvV);
            }

        }

        private void SwitchToCrypt(object sender, RoutedEventArgs e)
        {
            if (langue == 0)
            {
                setActiveUserControl(CryptV);
            }
            else
            {
                setActiveUserControl(CrypterV);
            }
        }

        private void chageLanguageF(object sender, RoutedEventArgs e)
        {
            Title = "Enregistrer facilement";            
            homeButtonLan.Content = "Acceuil";
            saveButtonLan.Content = "Créer une procedure de sauvegarde";
            lunchButtonLan.Content = "Lancer toutes les procédures de sauvegarde";
            cryptButtonLan.Content = "Crypter un fichier";
            changeLangueF.Visibility = Visibility.Collapsed;
            changeLangueA.Visibility = Visibility.Visible;
            setActiveUserControl(AcceuilV);

            langue = 1;
        }
        private void chageLanguageA(object sender, RoutedEventArgs e)
        {
            Title = "EasySave";
            homeButtonLan.Content = "Home";
            saveButtonLan.Content = "Create A Save Procedure";
            lunchButtonLan.Content = "Launch all save procedures sequentially";
            cryptButtonLan.Content = "Crypte a save";
            changeLangueA.Visibility = Visibility.Collapsed;
            changeLangueF.Visibility = Visibility.Visible;
            setActiveUserControl(Welcome);

            langue = 0;
        }
    }
}
