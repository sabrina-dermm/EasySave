using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasySaveV2.Model
{
    public class Priority : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        private String extentionList;
        public String ExtentionList
        {
            get { return extentionList; }
            set { extentionList = value; OnPropertyChanged("ExtentionList"); }
        }

        private String sizeFile;
        public String SizeFile
        {
            get { return sizeFile; }
            set { sizeFile = value; OnPropertyChanged("SizeFile"); }
        }
    }
}
