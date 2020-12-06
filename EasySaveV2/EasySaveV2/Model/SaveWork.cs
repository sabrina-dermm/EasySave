using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasySaveV2.Model
{
    public class SaveWork : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // create saveName variable
        private String nameSave;
        public String NameSave
        {
            get { return nameSave; }
            set { nameSave = value; OnPropertyChanged("NameSave"); }
        }

        //create the souce path variable
        private String srcPath;
        public String SrcPath
        {
            get { return srcPath; }
            set { srcPath = value; OnPropertyChanged("SrcPath"); }
        }

        //create the destination path variable
        private String destPath;
        public String DestPath
        {
            get { return destPath; }
            set { destPath = value; OnPropertyChanged("DestPath"); }
        }
        
        //create the type variable
        private String type;
        public String Type
        {
            get { return type; }
            set { type = value; OnPropertyChanged("Type"); }
        }
    }
}
