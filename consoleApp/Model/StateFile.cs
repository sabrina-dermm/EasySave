using System;
using System.Collections.Generic;
using System.Text;

namespace consoleApp.Model
{
    class StateFile
    {
        public String name { get; set; }
        public String srcPath { get; set; }
        public String destPath { get; set; }

        public StateFile()
        {
            name = "";
            srcPath = "";
            destPath = "";
        }
    }
}
