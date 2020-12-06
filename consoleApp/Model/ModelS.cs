using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace consoleApp.Model
{
    class ModelS
    {


        public List<SaveWork> backupJobList { get; set; }

        //the constructer that will create the stateFile
        public ModelS()
        {
            //If the state file has not been initialized then create 5 SaveWork object from nothing

            if (!File.Exists("state.json"))
            {
                backupJobList = new List<SaveWork>();
            }
            else
            {
                string json = File.ReadAllText("state.json");
                //Console.WriteLine(json);
                backupJobList = JsonConvert.DeserializeObject<List<SaveWork>>(json);

            }
        }



        //Can create a save work from simple parameters
        public void CreateWork(string _name, string _sourcePath, string _destinationPath, SaveWorkType _type)
        {
            SaveWork tempSave = new SaveWork(_name, _sourcePath, _destinationPath, _type);
            UpdateSaveFile(tempSave);
            CreateLogLine("Creation of a new save work , name : " + tempSave.name + ", source path : " + tempSave.sourcePath + ", destination path : " + tempSave.destinationPath + ", type : " + tempSave.type);
        }

        //Modify value of save works objects stored in workList, if there is any null parameters the value attached isn't changed
        public void ChangeWork(int _nb, string _name, string _sourcePath, string _destinationPath, SaveWorkType _type)
        {
            //modify the stateFile
            SaveWork backUpJobModified = new SaveWork(_name, _sourcePath, _destinationPath, _type);

            modifyStateFile(backUpJobModified, _nb);

            CreateLogLine("Modification of a existing save work in position " + _nb + ", current parameters : name : " + backupJobList[_nb - 1].name + ", source path : " + backupJobList[_nb - 1].sourcePath + ", destination path : " + backupJobList[_nb - 1].destinationPath + ", type : " + backupJobList[_nb - 1].type);
        }
        public void modifyStateFile(SaveWork backUpJobModified, int _nb)
        {
            string json = File.ReadAllText("state.json");
            backupJobList = JsonConvert.DeserializeObject<List<SaveWork>>(json);
            backupJobList[_nb - 1] = backUpJobModified;
            String stringjson = JsonConvert.SerializeObject(backupJobList, Formatting.Indented);
            File.WriteAllText("state.json", stringjson);
        }
        //Can delete a save work (set to null)
        public void DeleteWork(int _nb)
        {

            deleteJobInStateFile(_nb);
            CreateLogLine("Supression of save work in position" + _nb);
        }
        public void deleteJobInStateFile(int _nb)
        {
            string json = File.ReadAllText("state.json");
            backupJobList = JsonConvert.DeserializeObject<List<SaveWork>>(json);
            backupJobList.RemoveAt(_nb - 1);
            String stringjson = JsonConvert.SerializeObject(backupJobList, Formatting.Indented);
            File.WriteAllText("state.json", stringjson);
        }
        //Can initiate a type of save from the numbers of the save work in workList.
        public void DoSave(int _nb)
        {
            SaveWork work = backupJobList[_nb - 1];

            if (Directory.Exists(backupJobList[_nb - 1].sourcePath))
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
            CompleteCopy(_nb, backupJobList[_nb - 1].sourcePath, backupJobList[_nb - 1].destinationPath);
            CreateLogLine(backupJobList[_nb - 1].name + " save in position " + _nb + " DONE !");
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

            //update the state File
            string json = File.ReadAllText("state.json");
            backupJobList = JsonConvert.DeserializeObject<List<SaveWork>>(json);
            backupJobList[_nb - 1].CreateSaveProgress(nbFiles, directorySize, nbFiles, 0, directorySize);
            backupJobList[_nb - 1].isActive = true;
            String stringjson = JsonConvert.SerializeObject(backupJobList, Formatting.Indented);
            File.WriteAllText("state.json", stringjson);


            //initiate Copy from the source directory to the target directory
            CreateLogLine("Saving file from " + _sourceDirectory + " to " + _targetDirectory + " ...");
            CompleteCopyAll(_nb, diSource, diTarget);
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
                // UpdateSaveFile(_nb);
                string json = File.ReadAllText("state.json");
                backupJobList = JsonConvert.DeserializeObject<List<SaveWork>>(json);
                backupJobList[_nb - 1].saveProgress.currentSourceFilePath = fi.FullName;
                backupJobList[_nb - 1].saveProgress.currentDestinationFilePath = Path.Combine(_target.FullName, fi.Name);
                String stringjson = JsonConvert.SerializeObject(backupJobList, Formatting.Indented);
                File.WriteAllText("state.json", stringjson);

                CreateLogLine("Saving " + fi.FullName + " in " + backupJobList[_nb - 1].saveProgress.currentDestinationFilePath + ", size : " + fi.Length + " Bytes ...");

                //Copy the file and measure execution time
                Stopwatch watch = new Stopwatch();
                watch.Start();
                fi.CopyTo(Path.Combine(_target.FullName, fi.Name), true);
                watch.Stop();


                string json2 = File.ReadAllText("state.json");
                backupJobList = JsonConvert.DeserializeObject<List<SaveWork>>(json2);
                backupJobList[_nb - 1].saveProgress.filesRemaining--;
                backupJobList[_nb - 1].saveProgress.sizeRemaining -= fi.Length;
                String stringjson2 = JsonConvert.SerializeObject(backupJobList, Formatting.Indented);
                File.WriteAllText("state.json", stringjson2);
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
            DifferencialCopy(_nb, backupJobList[_nb - 1].sourcePath, backupJobList[_nb - 1].destinationPath);
            CreateLogLine(backupJobList[_nb - 1].name + " save in position " + _nb + " DONE !");
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
                //update the state File
                string json = File.ReadAllText("state.json");
                backupJobList = JsonConvert.DeserializeObject<List<SaveWork>>(json);
                backupJobList[_nb - 1].CreateSaveProgress(nbFiles, directorySize, nbFiles, 0, directorySize);
                backupJobList[_nb - 1].isActive = true;
                String stringjson = JsonConvert.SerializeObject(backupJobList, Formatting.Indented);
                File.WriteAllText("state.json", stringjson);



                //initiate Copy from the source directory to the target directory (only the file / directory that has been modified or are new)
                CreateLogLine("Saving file from " + _sourceDirectory + " to " + _targetDirectory + " ...");
                DifferencialCopyAll(_nb, diSource, diTarget);



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

                    //updateStateFile
                    string json = File.ReadAllText("state.json");
                    backupJobList = JsonConvert.DeserializeObject<List<SaveWork>>(json);
                    backupJobList[_nb - 1].saveProgress.currentSourceFilePath = fi.FullName;
                    backupJobList[_nb - 1].saveProgress.currentDestinationFilePath = Path.Combine(_target.FullName, fi.Name);
                    String stringjson = JsonConvert.SerializeObject(backupJobList, Formatting.Indented);
                    File.WriteAllText("state.json", stringjson);

                    CreateLogLine("Saving " + fi.FullName + " in " + backupJobList[_nb - 1].saveProgress.currentDestinationFilePath + ", size : " + fi.Length + " Bytes ...");

                    //Copy the file and measure execution time
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    fi.CopyTo(targetPath, true);
                    watch.Stop();



                    // UpdateSaveFile(_nb);
                    string json2 = File.ReadAllText("state.json");
                    backupJobList = JsonConvert.DeserializeObject<List<SaveWork>>(json2);
                    backupJobList[_nb - 1].saveProgress.filesRemaining--;
                    backupJobList[_nb - 1].saveProgress.sizeRemaining -= fi.Length;
                    String stringjson2 = JsonConvert.SerializeObject(backupJobList, Formatting.Indented);
                    File.WriteAllText("state.json", stringjson2);
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
            var convertedJson = JsonConvert.SerializeObject(newLogLine, Formatting.Indented);

            if (!File.Exists("log.json"))
            {

                File.WriteAllText("log.json", convertedJson);
            }
            else
            {
                File.AppendAllText("log.json", convertedJson);
            }

        }



        public void UpdateSaveFile(SaveWork saveW)
        {



            //Check is a save protocol is active or not
            if (saveW.isActive)
            {
                long sizeDifference = saveW.saveProgress.totalSize - saveW.saveProgress.sizeRemaining;

                //Check if the difference in size is equal to 0, to avoid division by 0
                if (sizeDifference != 0)
                {
                    saveW.saveProgress.progressState = ((saveW.saveProgress.totalSize - saveW.saveProgress.sizeRemaining) / saveW.saveProgress.totalSize * 100);
                }
            }

            if (!File.Exists("state.json"))
            {


                backupJobList.Add(saveW);
                String stringjson = JsonConvert.SerializeObject(backupJobList, Formatting.Indented);


                File.WriteAllText("state.json", stringjson);
            }
            else
            {
                string json = File.ReadAllText("state.json");
                //Console.WriteLine(json);
                backupJobList = JsonConvert.DeserializeObject<List<SaveWork>>(json);
                backupJobList.Add(saveW);
                String stringjson = JsonConvert.SerializeObject(backupJobList, Formatting.Indented);
                File.WriteAllText("state.json", stringjson);
            }

        }

    }
}