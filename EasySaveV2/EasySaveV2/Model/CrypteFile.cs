using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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

        private String srcPathCrypt;
        public String SrcPathCrypt
        {
            get { return srcPathCrypt; }
            set { srcPathCrypt = value; OnPropertyChanged("SrcPathCrypt"); }
        }
        private String destPathCrypt;
        public String DestPathCrypt
        {
            get { return destPathCrypt; }
            set { destPathCrypt = value; OnPropertyChanged("DestPathCrypt"); }
        }


    }
}
