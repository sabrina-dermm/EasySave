using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveV2.Model
{
    public class SaveProgress
    {
        //Time at the launch of the save protocol
        public String launchTime { get; set; }

        //Total file to save

        public int totalFilesNumber { get; set; }


        //Total size to copy


        public long totalSize { get; set; }


        //Total file remaining to save


        public int filesRemaining { get; set; }

        //Percent of progress in the save protocol


        public long progressState { get; set; }


        //Size remaining to save

        public long sizeRemaining { get; set; }


        //Source path of the current file we need to save

        public string currentSourceFilePath { get; set; }


        //Target path of the current file we need to save

        public string currentDestinationFilePath { get; set; }
        public SaveProgress(int totalFilesNumber, long totalSize, int filesRemaining, long progressState, long sizeRemaining)
        {
            //Enter the current time at the creation of the object
            launchTime = DateTime.Now.ToString();
            this.totalFilesNumber = totalFilesNumber;
            this.totalSize = totalSize;
            this.filesRemaining = filesRemaining;
            this.progressState = progressState;
            this.sizeRemaining = sizeRemaining;
            currentDestinationFilePath = null;
            currentSourceFilePath = null;
        }
    }
}
