using EasySaveV2.Command;
using EasySaveV2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace EasySaveV2.ViewModel
{
    public class ControllerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyChanged)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyChanged));
            }
        }
        ModelS model;
        private ObservableCollection<SaveWork> saveWorkList;
        public ObservableCollection<SaveWork> SaveWorkList
        {
            get { return saveWorkList; }
            set { saveWorkList = value; OnPropertyChanged("SaveWorkList"); }
        }
        private SaveWork currentSaveWork;
        public SaveWork CurrentSaveWork
        {
            get { return currentSaveWork; }
            set { currentSaveWork = value; OnPropertyChanged("CurrentSaveWork"); }
        }

        private String messageSave;
        public String MessageSave
        {
            get { return messageSave; }
            set { messageSave = value; OnPropertyChanged("MessageSave"); }
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get { return saveCommand; }
        }
        public void getSaveWorkList()
        {
            SaveWorkList = new ObservableCollection<SaveWork>(model.getAll());

        }
        public void saveCreate()
        {
            try
            {
                var IsSaved = model.addSaveWork(CurrentSaveWork);
                getSaveWorkList();
                if (IsSaved)
                {
                    MessageSave = "Save operation succed";
                }
                else
                {
                    MessageSave = "Save operation failed";
                }
            }
            catch (Exception ex)
            {
                MessageSave = ex.Message;
            }

        }
        public ControllerViewModel()
        {
            model = new ModelS();
            getSaveWorkList();
            currentSaveWork = new SaveWork();
            saveCommand = new RelayCommand(saveCreate);
        }
        /*
                    model = new ModelS();

           
        

        //Constructer
       

        
         */
    }
}
