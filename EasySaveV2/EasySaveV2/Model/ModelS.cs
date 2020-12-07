using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Security.Cryptography;

namespace EasySaveV2.Model
{
    public class ModelS
    {

        private static List<SaveWork> saveWorkList;

        static readonly char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
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

        public bool isProcessOn(String p)
        {
            bool isOn = false;
            Process[] liste = Process.GetProcesses();

            foreach (Process k in liste)

            {
                if (k.ProcessName == p)
                {
                    isOn = true;
                }
                

            }
            return isOn;
        }

        #region complete Save
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
        #endregion


        #region differential Save
        private void DifferencialSave(int _nb)
        {
            CreateLogLine("Launching save work from position " + _nb + ", type : differencial save");
            DifferencialCopy(_nb, saveWorkList[_nb - 1].SrcPath, saveWorkList[_nb - 1].DestPath);
            CreateLogLine("The time of encryption is 0");
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
        #endregion

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


        public bool cryptFile(CrypteFile cryptFile)
        {
            
            int i = 0;
            
            for(int j=0; j<saveWorkList.Count; j++)
            {
                if(saveWorkList[j].NameSave.Equals(cryptFile.NameSaveCrypt))
                {
                    i = j;
                }
            }
        
           

            if (saveWorkList[i].NameSave.Equals(cryptFile.NameSaveCrypt))
            {
                if (!File.Exists(saveWorkList[i].SrcPath + "/" + cryptFile.NameFileCrypt))
                {
                    throw new ArgumentException("The file you want to crypt does not exist");
                }
                else
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    try
                    {
                        if (!File.Exists(saveWorkList[i].DestPath + "/" + cryptFile.NameFileCrypt))
                        {
                            File.WriteAllText(saveWorkList[i].DestPath, "");
                        }
                        EncryptDecrypt(saveWorkList[i].SrcPath + "/" + cryptFile.NameFileCrypt, saveWorkList[i].DestPath + "/" + cryptFile.NameFileCrypt);
                        sw.Stop();
                        CreateLogLine("the cryption of "+cryptFile.NameFileCrypt +" file is succed Time spend : " + (int)sw.ElapsedMilliseconds);
                        //Console.WriteLine((int)sw.ElapsedMilliseconds);
                        Environment.Exit((int)sw.ElapsedMilliseconds);
                    }
                    catch (Exception e)
                    {
                        sw.Stop();
                        // Console.WriteLine((int)sw.ElapsedMilliseconds);
                        Console.WriteLine(e);
                        Environment.Exit(-1);
                    }
                }

            }
            else
            {
                throw new ArgumentException("This save is not created yet");

            }
            
            return true;
        }


        private static void EncryptDecrypt(string sourcepath, string targetpath)
        {
            string pathToKey = @"./key.txt";
            if (!File.Exists(pathToKey))
            {
                File.WriteAllText(pathToKey, GetUniqueKey(264));
            }
            byte[] key = Encoding.ASCII.GetBytes(File.ReadAllText(pathToKey));

            byte[] buffer = new byte[4096];

            FileStream fsSource;
            FileStream fsTarget;

            using (fsSource = new FileStream(sourcepath, FileMode.Open, FileAccess.Read))
            {
                //open writting stream
                using (fsTarget = new FileStream(targetpath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    int bytesRead = 0;

                    //read each byte and call the xor method before write them 
                    while ((bytesRead = fsSource.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fsTarget.Write(xorMeThisPlz(buffer, key), 0, bytesRead);
                    }

                    //clear buffer and write data in the file
                    fsTarget.Flush();
                    buffer = null;
                }
            }

            //return new string(output);
        }
        

        private static string GetUniqueKey(int size)
        {
            byte[] data = new byte[4 * size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }


        private static byte[] xorMeThisPlz(byte[] data, byte[] key)
        {
            /*char[] cryptedData = new char[input.Length];
			for (int i = 0; i < input.Length; i++)
			{
				output[i] = (char)(input[i] ^ key[i % key.Length]);
			}*/

            byte[] cryptedData = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                cryptedData[i] = (byte)(data[i] ^ key[i % key.Length]);
            }

            return cryptedData;
        }

        public bool processTrack(ProcessTrack p)
        {
            bool isOn = false;
            Process[] liste = Process.GetProcesses();

            foreach (Process k in liste)

            {
                if (k.ProcessName == p.ProcessName)
                {
                    isOn = true;
                    break;
                }
                

            }
            return isOn;
        }

    }
}
