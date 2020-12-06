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
        private CrypteFile currentFile;
        public CrypteFile CurrentFile
        {
            get { return currentFile; }
            set { CurrentFile = value; OnPropertyChanged("CurrentFile"); }
        }

        private String messageSave;
        public String MessageSave
        {
            get { return messageSave; }
            set { messageSave = value; OnPropertyChanged("MessageSave"); }
        }
        private String messageLunchSave;
        public String MessageLunchSave
        {
            get { return messageLunchSave; }
            set { messageLunchSave = value; OnPropertyChanged("MessageLunchSave"); }
        }
        private String messageLunchAllSave;
        public String MessageLunchAllSave
        {
            get { return messageLunchAllSave; }
            set { messageLunchAllSave = value; OnPropertyChanged("MessageLunchAllSave"); }
        }
        private String messageCrypt;
        public String MessageCrypt
        {
            get { return messageCrypt; }
            set { messageCrypt = value; OnPropertyChanged("MessageCrypt"); }
        }
        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get { return saveCommand; }
        }
        private RelayCommand lunchSaveCommand;
        public RelayCommand LuncgSaveCommand
        {
            get { return lunchSaveCommand; }
        }
        private RelayCommand cryptCommand;
        public RelayCommand CryptCommand
        {
            get { return cryptCommand; }
        }
        private RelayCommand lunchAllSaveCommand;
        public RelayCommand LunchAllSaveCommand
        {
            get { return lunchAllSaveCommand; }
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
                    MessageSave = "Save operation failed !";
                }
            }
            catch (Exception ex)
            {
                MessageSave = ex.Message;
            }

        }

        public void lunchSave()
        {
            var IsSaved = false;
            try
            {               
                    IsSaved = model.lunchSave(saveWorkList.Count);
                    getSaveWorkList();
                
                
                if (IsSaved)
                {
                    MessageLunchSave = "Lunch Save succed";
                }
                else
                {
                    MessageLunchSave = "Lunch Save failed !";
                }
            }
            catch(Exception ex)
            {
                MessageLunchSave = ex.Message;
            }

            
        }
        public void lunchAllSave()
        {
            var IsSaved = false;
            try
            {
                for(int i=1; i <= saveWorkList.Count; i++)
                {
                    IsSaved = model.lunchSave(i);
                    getSaveWorkList();
                }
                


                if (IsSaved)
                {
                    MessageLunchAllSave = "Lunch All Saves succed";
                }
                else
                {
                    MessageLunchAllSave = "Lunch All Saves failed !";
                }
            }
            catch (Exception ex)
            {
                MessageLunchSave = ex.Message;
            }


        }
        private void cryptFile()
        {
            var isCrypted = false;
            try
            {
                isCrypted = model.cryptFile(CurrentFile);
                if (isCrypted)
                {
                    MessageCrypt = "Crype File succed";
                }
                else
                {
                    MessageCrypt = "Crypt File failed";
                }

            }catch(Exception ex)
            {
                MessageCrypt = ex.Message;
            }
        }
        public ControllerViewModel()
        {
            model = new ModelS();
            getSaveWorkList();
            currentSaveWork = new SaveWork();
            saveCommand = new RelayCommand(saveCreate);
            lunchSaveCommand = new RelayCommand(lunchSave);
            lunchAllSaveCommand = new RelayCommand(lunchAllSave);
            currentFile = new CrypteFile();
            cryptCommand = new RelayCommand(cryptFile);
        }
        
    }
}
