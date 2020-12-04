using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EsaySaveV2._0.Model
{
   
    public class SaveWork : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropretyChanged(String proprtyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(proprtyName));
            }
        }




        private String nameSave;
        public String NameSave
        {
            get { return nameSave; }
            set { nameSave = value; OnPropretyChanged(NameSave); }
        }

        private String srcPath;
        public String SrcPath
        {
            get { return srcPath; }
            set { srcPath = value; OnPropretyChanged(SrcPath); }
        }


        private String destPath;
        public String DestPath
        {
            get { return destPath; }
            set { destPath = value; OnPropretyChanged(DestPath); }
        }
        //The type of save work (complete, differencial or unset)

        private String type;
        public String Type
        {
            get { return type; }
            set { type = value; OnPropretyChanged(Type); }
        }

        //Date of the creation of the object

        public string creationTime { get; set; }

        //Tell if a saving protocol is active or not to the current SaveWork object

        public bool isActive { get; set; }

        //Object defining the save progress when a saving protocol is active

       // public SaveProgress saveProgress { get; set; }

        //SaveWork class contructor from parameters given by the user
       /*
        public SaveWork(string name, string sourcePath, string destinationPath, String type)
        {
            this.name = name;
            creationTime = DateTime.Now.ToString();
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
            this.type = type;
            isActive = false;
            //saveProgress = null;
        }
        */
        


       




    }
   
}

