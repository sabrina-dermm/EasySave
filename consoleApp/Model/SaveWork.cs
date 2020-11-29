using System;
using System.Collections.Generic;
using System.Text;

namespace consoleApp.Model
{
    public enum SaveWorkType
    {
        complete,
        differencial,
        unset
    }
    class SaveWork
    {

        //The actual name of the save work given by the user

        public string name { get; set; }


        //The source path of the directory to save

        public string sourcePath { get; set; }

        //The destion path to store the save

        public string destinationPath { get; set; }

        //The type of save work (complete, differencial or unset)


        public SaveWorkType type { get; set; }

        //Date of the creation of the object

        public string creationTime { get; set; }

        //Tell if a saving protocol is active or not to the current SaveWork object

        public bool isActive { get; set; }

        //Object defining the save progress when a saving protocol is active

        public SaveProgress saveProgress { get; set; }

        //SaveWork class contructor from parameters given by the user
        public SaveWork(string name, string sourcePath, string destinationPath, SaveWorkType type)
        {
            this.name = name;
            creationTime = DateTime.Now.ToString();
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
            this.type = type;
            isActive = false;
            saveProgress = null;
        }

        

        //Create a SaveProgress object when a saving protocol starts
        public void CreateSaveProgress(int totalFilesNumber, long totalSize, int filesRemaining, int progressState, long sizeRemaining)
        {
            saveProgress = new SaveProgress(totalFilesNumber, totalSize, filesRemaining, progressState, sizeRemaining);
        }

        //Delete the SaveProgress object when the saving protocol stops
        public void DeleteSaveProgress()
        {
            saveProgress = null;
        }





        //Define all the type of save protocols we can have (unset when no protocol is associate with the save work)
    }
   
}

