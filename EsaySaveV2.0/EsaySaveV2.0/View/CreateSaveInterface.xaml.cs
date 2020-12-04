using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EsaySaveV2._0.Controller;


namespace EsaySaveV2._0.View
{
    
    public partial class CreateSaveInterface : UserControl
    {
        public String saveName = null;
        public String srcPath = null;
        public String destPath = null;
        public String type = null;
        public String[] choice = new String[4];
        public Controler contr = new Controler();
       
        public CreateSaveInterface()
        {
            InitializeComponent();

        }
        

      

        public String[] saveProcedure(String name, String srcPath, String destPath, String type)
        {
            String[] choice = new string[4];
           
            //Ask for Name
            if(saveName.Length != 0)
            {
                if (Regex.IsMatch(saveName, @"^[a-zA-Z0-9 _]+$"))
                {
                    saveName = saveNameInput.Text.ToString();
                    
                }
                else
                {
                    MessageBox.Show("Please only make use of alphanumeric characters, spaces or underscores.");
                }
                choice[0] = saveName;
            }
            else
            {
                MessageBox.Show("Please enter the name of the save procedure !");
            }
            //Ask for souce path
            if(srcPath.Length != 0)
            {
                if (Regex.IsMatch(srcPath, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))
                {
                    srcPath = saveSrcInput.Text.ToString();
                    
                }
                else
                {
                    MessageBox.Show("Please enter a valid souce path !");
                }
                choice[1] = srcPath;
            }
            else
            {
                MessageBox.Show("Please enter a source path !");
            }

            //Ask for destination path
            if (destPath.Length != 0)
            {
                if (Regex.IsMatch(destPath, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))
                {
                    destPath = saveDestInput.Text.ToString();
                }
                else
                {
                    MessageBox.Show("Please enter a valid destination path !");
                }
                choice[2] = destPath;
            }
            else
            {
                MessageBox.Show("Please enter a destination path !");
            }
            if (type.Equals("complete"))
            {
                choice[3] = "complete";
            }
            else
            {
                choice[3] = "differential";
            }
            return choice;
        }

        private void saveProcedureButton(object sender, RoutedEventArgs e)
        {
            saveName = saveNameInput.Text.ToString();
            srcPath = saveSrcInput.Text.ToString();
            destPath = saveDestInput.Text.ToString();

            choice = saveProcedure(saveName, srcPath, destPath, type);
            
           
        }

       

        private void comCheck(object sender, RoutedEventArgs e)
        {
            type = "complete";
        }

        private void diffCheck(object sender, RoutedEventArgs e)
        {
            type = "differential";
        }
    }
}
