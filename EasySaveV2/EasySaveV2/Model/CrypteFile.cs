using System;
using System.ComponentModel;

namespace EasySaveV2.Model
{
    public class CrypteFile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private String nameSaveCrypt;
        public String NameSaveCrypt
        {
            get { return nameSaveCrypt; }
            set { nameSaveCrypt = value; OnPropertyChanged("NameSaveCrypt"); }
        }
        private String nameFileCrypt;
        public String NameFileCrypt
        {
            get { return nameFileCrypt; }
            set { nameFileCrypt = value; OnPropertyChanged("NameFileCrypt"); }
        }


    }
}
