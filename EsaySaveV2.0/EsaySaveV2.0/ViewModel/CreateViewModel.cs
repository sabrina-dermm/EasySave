using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using EsaySaveV2._0.Model;
using System.Collections.ObjectModel;

namespace EsaySaveV2._0.ViewModel
{
    public class CreateViewModel: INotifyPropertyChanged
    {
        private SaveWork saveWork;
        public SaveWork SaveWork
        {
            get { return saveWork; }
            set { saveWork = value; NotifyPropertyChanged("SaveWork"); }
        }

        private ObservableCollection<SaveWork> saveWorkList;
        public ObservableCollection<SaveWork> SaveWorkList
        {
            get { return saveWorkList; }
            set { saveWorkList = value; NotifyPropertyChanged("SaveWorkList"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String proprtyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(proprtyName));
            }
        }


        //create the propreties to bien them with the user interface

    }
}
