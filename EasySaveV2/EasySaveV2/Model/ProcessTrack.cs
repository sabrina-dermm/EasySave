using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasySaveV2.Model
{
    public class ProcessTrack : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private String processName;
        public String ProcessName
        {
            get { return processName; }
            set { processName = value; OnPropertyChanged("processName"); }
        }
    }
}
