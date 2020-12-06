using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace EasySaveV2.Model
{
    public class ModelS
    {
        private static List<SaveWork> saveWorkList;

        //Create the constructer
        public ModelS()
        {
            if (!File.Exists("state.json"))
            {
                saveWorkList = new List<SaveWork>();
            }
            else
            {
                string json = File.ReadAllText("state.json");
                //Console.WriteLine(json);
                saveWorkList = JsonConvert.DeserializeObject<List<SaveWork>>(json);

            }

        }


        //methode to get all the saves

        public List<SaveWork> getAll()
        {
            return saveWorkList;
        }

        // methode to add the employee in the list and return true if it worked
        public bool addSaveWork(SaveWork save)
        {
            if ((!Regex.IsMatch(save.NameSave, @"^[a-zA-Z0-9 _]+$")))
            {
                throw new ArgumentException("Please only make use of alphanumeric characters, spaces or underscores.");
            }

            
             
            if (!Regex.IsMatch(save.SrcPath, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))
            {
                throw new ArgumentException("Please enter a valid absolute path.");

            }
            if (!Regex.IsMatch(save.DestPath, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))
            {
                throw new ArgumentException("Please enter a valid absolute path.");
            }
              if (save.Type != "complete" && save.Type != "differential")
            {
                throw new ArgumentException("You can only choose between complete and differential");
            }
            if (!File.Exists("state.json"))
            {


                saveWorkList.Add(save);
                String stringjson = JsonConvert.SerializeObject(saveWorkList, Formatting.Indented);


                File.WriteAllText("state.json", stringjson);
            }
            else
            {
                string json = File.ReadAllText("state.json");
                //Console.WriteLine(json);
                saveWorkList = JsonConvert.DeserializeObject<List<SaveWork>>(json);
                saveWorkList.Add(save);
                String stringjson = JsonConvert.SerializeObject(saveWorkList, Formatting.Indented);
                File.WriteAllText("state.json", stringjson);
            }
            CreateLogLine("Creation of a new save work , name : " + save.NameSave + ", source path : " + save.SrcPath + ", destination path : " + save.DestPath + ", type : " + save.Type);

            return true;

        }


        // methode to modift the list 
        //i need to modify this -------------------------------------------------------
        public bool updateList(SaveWork em)
        {
            bool isUpdated = false;
            for (int i = 0; i < saveWorkList.Count; i++)
            {
                if (saveWorkList[i].NameSave == em.NameSave)
                {
                    saveWorkList[i] = em;
                    isUpdated = true;
                    break;
                }
            }
            return isUpdated;
        }

        public bool lunchSave(int index)
        {
            
            SaveWork work ;
            work = saveWorkList[index-1];            
           
            
            if (Directory.Exists(work.SrcPath))
            {
                if (work.Type == "complete")
                {
                    CompleteSave(index);
                }
                else if (work.Type == "differencial")
                {
                    DifferencialSave(index);
                }
            }
             
            return true;
        }
        private void CompleteSave(int _nb)
        {
            CreateLogLine("Launching save work from position " + _nb + ", type : complete save");
            CompleteCopy(_nb, saveWorkList[_nb - 1].SrcPath, saveWorkList[_nb - 1].DestPath);
            CreateLogLine(saveWorkList[_nb - 1].NameSave + " save in position " + _nb + " DONE !");
        }
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
            saveWorkList = JsonConvert.DeserializeObject<List<SaveWork>>(json);
            saveWorkList[_nb - 1].CreateSaveProgress(nbFiles, directorySize, nbFiles, 0, directorySize);
            saveWorkList[_nb - 1].isActive = true;
            String stringjson = JsonConvert.SerializeObject(saveWorkList, Formatting.Indented);
            File.WriteAllText("state.json", stringjson);


            //initiate Copy from the source directory to the target directory
            CreateLogLine("Saving file from " + _sourceDirectory + " to " + _targetDirectory + " ...");
            CompleteCopyAll(_nb, diSource, diTarget);
            CreateLogLine("Closing complete save work program ...");
        }
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
                saveWorkList = JsonConvert.DeserializeObject<List<SaveWork>>(json);
                saveWorkList[_nb - 1].saveProgress.currentSourceFilePath = fi.FullName;
                saveWorkList[_nb - 1].saveProgress.currentDestinationFilePath = Path.Combine(_target.FullName, fi.Name);
                String stringjson = JsonConvert.SerializeObject(saveWorkList, Formatting.Indented);
                File.WriteAllText("state.json", stringjson);

                CreateLogLine("Saving " + fi.FullName + " in " + saveWorkList[_nb - 1].saveProgress.currentDestinationFilePath + ", size : " + fi.Length + " Bytes ...");

                //Copy the file and measure execution time
                Stopwatch watch = new Stopwatch();
                watch.Start();
                fi.CopyTo(Path.Combine(_target.FullName, fi.Name), true);
                watch.Stop();


                string json2 = File.ReadAllText("state.json");
                saveWorkList = JsonConvert.DeserializeObject<List<SaveWork>>(json2);
                saveWorkList[_nb - 1].saveProgress.filesRemaining--;
                saveWorkList[_nb - 1].saveProgress.sizeRemaining -= fi.Length;
                String stringjson2 = JsonConvert.SerializeObject(saveWorkList, Formatting.Indented);
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

        private void DifferencialSave(int _nb)
        {
            CreateLogLine("Launching save work from position " + _nb + ", type : differencial save");
            DifferencialCopy(_nb, saveWorkList[_nb - 1].SrcPath, saveWorkList[_nb - 1].DestPath);
            CreateLogLine(saveWorkList[_nb - 1].NameSave + " save in position " + _nb + " DONE !");
        }
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
                saveWorkList = JsonConvert.DeserializeObject<List<SaveWork>>(json);
                saveWorkList[_nb - 1].CreateSaveProgress(nbFiles, directorySize, nbFiles, 0, directorySize);
                saveWorkList[_nb - 1].isActive = true;
                String stringjson = JsonConvert.SerializeObject(saveWorkList, Formatting.Indented);
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
                    saveWorkList = JsonConvert.DeserializeObject<List<SaveWork>>(json);
                    saveWorkList[_nb - 1].saveProgress.currentSourceFilePath = fi.FullName;
                    saveWorkList[_nb - 1].saveProgress.currentDestinationFilePath = Path.Combine(_target.FullName, fi.Name);
                    String stringjson = JsonConvert.SerializeObject(saveWorkList, Formatting.Indented);
                    File.WriteAllText("state.json", stringjson);

                    CreateLogLine("Saving " + fi.FullName + " in " + saveWorkList[_nb - 1].saveProgress.currentDestinationFilePath + ", size : " + fi.Length + " Bytes ...");

                    //Copy the file and measure execution time
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    fi.CopyTo(targetPath, true);
                    watch.Stop();



                    // UpdateSaveFile(_nb);
                    string json2 = File.ReadAllText("state.json");
                    saveWorkList = JsonConvert.DeserializeObject<List<SaveWork>>(json2);
                    saveWorkList[_nb - 1].saveProgress.filesRemaining--;
                    saveWorkList[_nb - 1].saveProgress.sizeRemaining -= fi.Length;
                    String stringjson2 = JsonConvert.SerializeObject(saveWorkList, Formatting.Indented);
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
        //methode of CreateLogLine
        private void CreateLogLine(String content)
        {
            LogLine newLogLine = new LogLine(content);
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



    }
}
