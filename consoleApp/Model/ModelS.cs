using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace consoleApp.Model
{
    class ModelS
    {
        //Store all 5 (max) save works
        private SaveWork[] workList;

        public SaveWork[] WorkList
        {
            get { return workList; }
            set { workList = value; }
        }

        //the constructer that will create the stateFile
        public ModelS()
        {
            //If the state file has not been initialized then create 5 SaveWork object from nothing
            if (!File.Exists("stateFile.json"))
            {
                WorkList = new SaveWork[5];
                for (int i = 0; i < 5; i++)
                {
                    WorkList[i] = new SaveWork("", "", "", SaveWorkType.unset);
                }
            }
            //Then if the State file already exist, use the objects in it to create the WorkList
            else
            {
                string stateFile = File.ReadAllText("stateFile.json");
                var tempWorkList = JsonConvert.DeserializeObject<List<SaveWork>>(stateFile);
                WorkList = tempWorkList.ToArray();
            }
        }

        

        //Can create a save work from simple parameters
        public void CreateWork(int _nb, string _name, string _sourcePath, string _destinationPath, SaveWorkType _type)
        {
            SaveWork tempSave = new SaveWork(_name, _sourcePath, _destinationPath, _type);
            WorkList[_nb - 1] = tempSave;
            UpdateSaveFile(_nb);
            CreateLogLine("Creation of a new save work in position " + _nb + ", name : " + tempSave.name + ", source path : " + tempSave.sourcePath + ", destination path : " + tempSave.destinationPath + ", type : " + tempSave.type);
        }

        //Modify value of save works objects stored in workList, if there is any null parameters the value attached isn't changed
        public void ChangeWork(int _nb, string _name, string _sourcePath, string _destinationPath, SaveWorkType _type)
        {
            if (_name != "") { WorkList[_nb - 1].name = _name; }
            if (_sourcePath != "") { WorkList[_nb - 1].sourcePath = _sourcePath; }
            if (_destinationPath != "") { WorkList[_nb - 1].destinationPath = _destinationPath; }
            if (_type != SaveWorkType.unset) { WorkList[_nb - 1].type = _type; }

            UpdateSaveFile(_nb);
            CreateLogLine("Modification of a existing save work in position " + _nb + ", current parameters : name : " + WorkList[_nb - 1].name + ", source path : " + WorkList[_nb - 1].sourcePath + ", destination path : " + WorkList[_nb - 1].destinationPath + ", type : " + WorkList[_nb - 1].type);
        }

        //Can delete a save work (set to null)
        public void DeleteWork(int _nb)
        {
            workList[_nb - 1] = new SaveWork("", "", "", SaveWorkType.unset);
            UpdateSaveFile(_nb);
            CreateLogLine("Supression of save work in position" + _nb);
        }

        //Can initiate a type of save from the numbers of the save work in workList.
        public void DoSave(int _nb)
        {
            SaveWork work = WorkList[_nb - 1];

            if (Directory.Exists(WorkList[_nb - 1].sourcePath))
            {
                if (work.type == SaveWorkType.complete)
                {
                    CompleteSave(_nb);
                }
                else if (work.type == SaveWorkType.differencial)
                {
                    DifferencialSave(_nb);
                }
            }
        }

        //Launch a complete save from a SaveWork type parameter
        private void CompleteSave(int _nb)
        {
            CreateLogLine("Launching save work from position " + _nb + ", type : complete save");
            CompleteCopy(_nb, WorkList[_nb - 1].sourcePath, WorkList[_nb - 1].destinationPath);
            CreateLogLine(WorkList[_nb - 1].name + " save in position " + _nb + " DONE !");
        }

        //Do a complete copy from a folder to another
        private void CompleteCopy(int _nb, string _sourceDirectory, string _targetDirectory)
        {
            //Search directory info from source and target path
            var diSource = new DirectoryInfo(_sourceDirectory);
            var diTarget = new DirectoryInfo(_targetDirectory);

            //Calculate the number of file in the source directory and the total size of it
            int nbFiles = SourceDirectoryInfo.GetFilesNumberInSourceDirectory(diSource);
            long directorySize = SourceDirectoryInfo.GetSizeInSourceDirectory(diSource);
            CreateLogLine(nbFiles + " files to save found from " + _sourceDirectory + ",Total size of the directory: " + directorySize + " Bytes");

            WorkList[_nb - 1].CreateSaveProgress(nbFiles, directorySize, nbFiles, 0, directorySize);
            WorkList[_nb - 1].isActive = true;
            UpdateSaveFile(_nb);

            //initiate Copy from the source directory to the target directory
            CreateLogLine("Saving file from " + _sourceDirectory + " to " + _targetDirectory + " ...");
            CompleteCopyAll(_nb, diSource, diTarget);

            //Closing the complete save protocol
            WorkList[_nb - 1].DeleteSaveProgress();
            WorkList[_nb - 1].isActive = false;
            UpdateSaveFile(_nb);
            CreateLogLine("Closing complete save work program ...");
        }

        //Copy each file from a directory, and do the same for each subdirectory using recursion
        private void CompleteCopyAll(int _nb, DirectoryInfo _source, DirectoryInfo _target)
        {

            //First create the new target directory where all the files are saved later on
            CreateLogLine("Creating target directory ...");
            Directory.CreateDirectory(_target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in _source.GetFiles())
            {
                WorkList[_nb - 1].saveProgress.currentSourceFilePath = fi.FullName;
                WorkList[_nb - 1].saveProgress.currentDestinationFilePath = Path.Combine(_target.FullName, fi.Name);
                UpdateSaveFile(_nb);

                CreateLogLine("Saving " + fi.FullName + " in " + WorkList[_nb - 1].saveProgress.currentDestinationFilePath + ", size : " + fi.Length + " Bytes ...");

                //Copy the file and measure execution time
                Stopwatch watch = new Stopwatch();
                watch.Start();
                fi.CopyTo(Path.Combine(_target.FullName, fi.Name), true);
                watch.Stop();


                WorkList[_nb - 1].saveProgress.filesRemaining--;
                WorkList[_nb - 1].saveProgress.sizeRemaining -= fi.Length;
                UpdateSaveFile(_nb);
                CreateLogLine(fi.Name + " succesfully saved ! Time spend : " + watch.Elapsed.TotalSeconds.ToString());
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in _source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    _target.CreateSubdirectory(diSourceSubDir.Name);
                CreateLogLine("Entering subdirectory : " + diSourceSubDir.Name);
                CompleteCopyAll(_nb, diSourceSubDir, nextTargetSubDir);
                CreateLogLine("Exiting subdirectory : " + diSourceSubDir.Name);
            }
        }

        //Launch a diffrencial save from a SaveWork parameter
        private void DifferencialSave(int _nb)
        {
            CreateLogLine("Launching save work from position " + _nb + ", type : differencial save");
            DifferencialCopy(_nb, WorkList[_nb - 1].sourcePath, WorkList[_nb - 1].destinationPath);
            CreateLogLine(WorkList[_nb - 1].name + " save in position " + _nb + " DONE !");
        }

        //Do a différential copy from a folder to another
        private void DifferencialCopy(int _nb, string _sourceDirectory, string _targetDirectory)
        {
            //Search directory info from source and target path
            var diSource = new DirectoryInfo(_sourceDirectory);
            var diTarget = new DirectoryInfo(_targetDirectory);

            //Calculate the number of file in the source directory and the total size of it (of all )
            int nbFiles = SourceDirectoryInfo.DifferencialGetFilesNumberInSourceDirectory(diSource, diTarget);
            long directorySize = SourceDirectoryInfo.DifferencialGetSizeInSourceDirectory(diSource, diTarget);

            //If there is at least one file to save then initiate the differencial saving protocol
            if (nbFiles != 0)
            {
                CreateLogLine(nbFiles + " files to save found from " + _sourceDirectory + ",Total size of the directory: " + directorySize + " Bytes");

                WorkList[_nb - 1].CreateSaveProgress(nbFiles, directorySize, nbFiles, 0, directorySize);
                WorkList[_nb - 1].isActive = true;

                UpdateSaveFile(_nb);

                //initiate Copy from the source directory to the target directory (only the file / directory that has been modified or are new)
                CreateLogLine("Saving file from " + _sourceDirectory + " to " + _targetDirectory + " ...");
                DifferencialCopyAll(_nb, diSource, diTarget);

                WorkList[_nb - 1].DeleteSaveProgress();
                WorkList[_nb - 1].isActive = false;
                UpdateSaveFile(_nb);
            }
            //If there is no file to save then cancel the saving protocol
            else
            {
                CreateLogLine("There is no file to save in the target directory");
            }

            CreateLogLine("Closing differencial save work program ...");
        }

        //Copy each files (that has been modified since the last save) from a directory, and do the same for each subdirectory using recursion
        private void DifferencialCopyAll(int _nb, DirectoryInfo _source, DirectoryInfo _target)
        {
            CreateLogLine("Creating target directory ...");
            Directory.CreateDirectory(_target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in _source.GetFiles())
            {
                //Calculate the path of the future file we need to save
                string targetPath = Path.Combine(_target.FullName, fi.Name);

                //Check if the file already exist or not (new one), and verify if it has been modified or not
                if (!File.Exists(targetPath) || fi.LastWriteTime != File.GetLastWriteTime(targetPath))
                {
                    WorkList[_nb - 1].saveProgress.currentSourceFilePath = fi.FullName;
                    WorkList[_nb - 1].saveProgress.currentDestinationFilePath = Path.Combine(_target.FullName, fi.Name);
                    UpdateSaveFile(_nb);
                    CreateLogLine("Saving " + fi.FullName + " in " + WorkList[_nb - 1].saveProgress.currentDestinationFilePath + ", size : " + fi.Length + " Bytes ...");

                    //Copy the file and measure execution time
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    fi.CopyTo(targetPath, true);
                    watch.Stop();

                    WorkList[_nb - 1].saveProgress.filesRemaining--;
                    WorkList[_nb - 1].saveProgress.sizeRemaining -= fi.Length;
                    UpdateSaveFile(_nb);
                    CreateLogLine(fi.Name + " succesfully saved ! Time spend : " + watch.Elapsed.TotalSeconds.ToString());
                }


            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in _source.GetDirectories())
            {
                string targetDirectoryPath = Path.Combine(_target.FullName, diSourceSubDir.Name);
                CreateLogLine("Entering subdirectory : " + diSourceSubDir.Name);

                //Check if the directory already exist to decide if it is required to create a new one or not
                if (!Directory.Exists(targetDirectoryPath))
                {
                    DirectoryInfo nextTargetSubDir = _target.CreateSubdirectory(diSourceSubDir.Name);
                    DifferencialCopyAll(_nb, diSourceSubDir, nextTargetSubDir);
                }
                else
                {
                    DirectoryInfo nextTargetSubDir = new DirectoryInfo(targetDirectoryPath);
                    DifferencialCopyAll(_nb, diSourceSubDir, nextTargetSubDir);
                }

                CreateLogLine("Exiting subdirectory : " + diSourceSubDir.Name);

            }
        }



        // Create the line to record in the log file
        public void CreateLogLine(string _content)
        {
            //New LogLine object with Time and content
            LogLine newLogLine = new LogLine(_content);

            //Create a raw string from the json log file
            string JsonLog = File.ReadAllText("log.json");

            //Convert the raw string into a LogLine object list
            var LogList = JsonConvert.DeserializeObject<List<LogLine>>(JsonLog);

            //Add the new object to the list
            LogList.Add(newLogLine);

            //Convert the LogLine object list into a json formated string
            var convertedJson = JsonConvert.SerializeObject(LogList, Formatting.Indented);

            //Write the new string into the json log file
            File.WriteAllText("log.json", convertedJson);

        }

        //Update the state file with the work list value
        public void UpdateSaveFile(int _nb)
        {
            //Check is a save protocol is active or not
            if (WorkList[_nb - 1].isActive)
            {
                long sizeDifference = WorkList[_nb - 1].saveProgress.totalSize - WorkList[_nb - 1].saveProgress.sizeRemaining;

                //Check if the difference in size is equal to 0, to avoid division by 0
                if (sizeDifference != 0)
                {
                    WorkList[_nb - 1].saveProgress.progressState = ((WorkList[_nb - 1].saveProgress.totalSize - WorkList[_nb - 1].saveProgress.sizeRemaining) / WorkList[_nb - 1].saveProgress.totalSize * 100);
                }
            }

            //Convert the work list to a json string then write it in a json file
            var convertedJson = JsonConvert.SerializeObject(WorkList, Formatting.Indented);
            File.WriteAllText("stateFile.json", convertedJson);
        }

    }
}
