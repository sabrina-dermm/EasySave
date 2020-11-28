using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace consoleApp.Model
{
    class ModelS
    {
        public SaveConfiguration saveConfiguration { get; set; }
        public SaveEnvirement saveEnvirement { get; set; }
        public SaveConfiguration[] saveConfigurationList { get; set; }
        public ModelS()
        {

        }

        //Can create a save work from simple parameters
        public void createSave( string name, string sourcePath, string destinationPath, SaveType saveType)
        {
            SaveConfiguration tempSave = new SaveConfiguration(name, sourcePath, destinationPath, saveType);
            saveConfiguration = tempSave;
            SaveEnvirement saveEn = new SaveEnvirement(name, sourcePath, destinationPath);
            saveEnvirement = saveEn;
            
            updateSaveFile();
            updateSaveEnvirement();
            // CreateLogLine("Creation of a new save work in position " + _nb + ", name : " + tempSave.Name + ", source path : " + tempSave.SourcePath + ", destination path : " + tempSave.DestinationPath + ", type : " + tempSave.Type);
        }
        //Update the state file with the save list value
        public void updateSaveFile()
        {
            var convertedJson = JsonConvert.SerializeObject(saveConfiguration,Formatting.Indented);
            if (!File.Exists("stateFile.json"))
            {

                File.WriteAllText("stateFile.json", convertedJson);
            }
            else
            {
                File.AppendAllText("stateFile.json", convertedJson);
            }
            

        }
        public void updateSaveEnvirement()
        {
            var convertedJson = JsonConvert.SerializeObject(saveEnvirement, Formatting.Indented);

            if (!File.Exists("saveEnvirement.json"))
            {
                File.WriteAllText("saveEnvirement.json", convertedJson);
            }
            else
            {
                File.AppendAllText("saveEnvirement.json", convertedJson);
            }
        }
    }
}
