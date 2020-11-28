using System;
using System.Collections.Generic;
using System.Text;

namespace consoleApp.Model
{
    class SaveEnvirement
    {
        public String name { get; set; }
        public String srcPath { get; set; }
        public String destPath { get; set; }
        public SaveEnvirement(String name, String srcPath, String destPath)
        {
            this.name = name;
            this.srcPath = srcPath;
            this.destPath = destPath;
        }
    }
}
