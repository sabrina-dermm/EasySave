using System;
using System.Collections.Generic;
using System.Text;

namespace consoleApp.Model
{
    public enum SaveType
    {
        complete,
        differencial,
        notSet
    }
    class SaveConfiguration
    {
        public String name { get; set; }
        public String srcPath { get; set; }
        public String destPath { get; set; }
        // public SaveType type { get; set; }
        public String creationTime { get; set; }
        public bool isActive { get; set; }
        public SaveConfiguration(String name, String srcPath, String destPath, SaveType saveType)
        {
            this.name = name;
            this.srcPath = srcPath;
            this.destPath = destPath;

            creationTime = DateTime.Now.ToString();
            isActive = false;
            saveType = SaveType.notSet;

        }
    }
}
