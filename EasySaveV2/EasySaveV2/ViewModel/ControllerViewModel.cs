using EasySaveV2.Command;
using EasySaveV2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasySaveV2.ViewModel
{
    public class ControllerViewModel : INotifyPropertyChanged
    {
        ModelS model;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyChanged)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyChanged));
            }
        }

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

        private ProcessTrack currentProcessTrack;
        public ProcessTrack CurrentProcessTrack
        {
            get { return currentProcessTrack; }
            set { currentProcessTrack = value; OnPropertyChanged("currentProcessTrack"); }
        }

        private CrypteFile currentFile;
        public CrypteFile CurrentFile
        {
            get { return currentFile; }
            set { currentFile = value; OnPropertyChanged("CurrentFile"); }
        }

        private Priority currentPriority;
        public Priority CurrentPriority
        {
            get { return currentPriority; }
            set { currentPriority = value; OnPropertyChanged("CurrentPriority"); }
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

        private String messageProcess;
        public String MessageProcess
        {
            get { return messageProcess; }
            set { messageProcess = value; OnPropertyChanged("MessageProcess"); }
        }
        private String messageProprty;
        public String MessageProprty
        {
            get { return messageProprty; }
            set { messageProprty = value; OnPropertyChanged("MessageProprty"); }
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

        private RelayCommand processCommand;
        public RelayCommand ProcessCommand
        {
            get { return processCommand; }
        }
        
        private RelayCommand priorityCommand;
        public RelayCommand PriorityCommand
        {
            get { return priorityCommand; }
        }
        private RelayCommand pauseSaveCommand;
        public RelayCommand PauseSaveCommand
        {
            get { return pauseSaveCommand; }
        }
        private RelayCommand continueSaveCommand;
        public RelayCommand ContinueSaveCommand
        {
            get { return continueSaveCommand; }
        }
        private RelayCommand stopSaveCommand;
        public RelayCommand StopSaveCommand
        {
            get { return stopSaveCommand; }
        }
        private ManualResetEvent resetEvent = new ManualResetEvent(true);
        private Thread thread;

        public ControllerViewModel()
        {
            model = new ModelS();
            getSaveWorkList();
            currentSaveWork = new SaveWork();
            saveCommand = new RelayCommand(saveCreate);
            lunchSaveCommand = new RelayCommand(lunchSave);
            lunchAllSaveCommand = new RelayCommand(lunchAllSaveSyc);
            currentFile = new CrypteFile();
            cryptCommand = new RelayCommand(cryptFile);
            currentProcessTrack = new ProcessTrack();
            processCommand = new RelayCommand(isOnProcess);
            currentPriority = new Priority();
            priorityCommand = new RelayCommand(isPropretyCheck);
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
                if (model.processTrack(CurrentProcessTrack))
                {
                    MessageLunchSave = "You can't save because " + CurrentProcessTrack.ProcessName + " is on";
                }
                else
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
                   
            }
            catch(Exception ex)
            {
                MessageLunchSave = ex.Message;
            }

            
        }
        
        Semaphore semaphoreObject = new Semaphore(initialCount: 1, maximumCount: 4, name: "SaveApp");
       
       
        public void SaveSync(object index)
        {
            int j = 1 + (int)index; 
            try
            {
                //Blocks the current thread until the current WaitHandle receives a signal.   
                semaphoreObject.WaitOne();
                model.lunchSave(j);
                MessageLunchAllSave = String.Concat("\n Save "+j+ " succeded", MessageLunchAllSave);
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
            finally
            {
                semaphoreObject.Release();
            }
        }
    
        public void lunchAllSaveSyc()
            {
            List<SaveWork> s = model.getAll();
            List<SaveWork> prioty =model.modifyTheOrderOfTheList(s,CurrentPriority);
            //get the index list of priorty from s
            int[] indexes = new int[s.Count];

            for(int i=0; i<indexes.Length; i++)
            {
                indexes[i] = s.IndexOf(prioty[i]);
            }
            try
            {  
                for(int i=0; i< indexes.Length; i++){
                    Thread thread = new Thread(SaveSync);
                    thread.Start(indexes[i]);
                }                                             
            }
            catch (Exception ex)
            {
                MessageLunchAllSave = ex.Message;
            }           
        }
        
        
        public void isOnProcess()
        {
            try
            {
                if (model.processTrack(CurrentProcessTrack))
                {
                    MessageProcess = "The process is on";
                }
                else
                {
                    MessageProcess = "The process is not on";
                }
            }catch(Exception ex)
            {
                MessageProcess = ex.Message;
            }
        }
        public void isPropretyCheck()
        {
            try
            {
                if (model.isPropretyCheck(CurrentPriority))
                {
                    MessageProprty = "The propreties are saved";
                }
                else
                {
                    MessageProprty = "The propreties are not valid";
                }
            }
            catch (Exception ex)
            {
                MessageProprty = ex.Message;
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
       
        


    }
}
