using System;
using System.Collections.Generic;
using System.Text;

namespace consoleApp.Model
{
    class LogLine
    {
        public String time { get; set; }
        public String content { get; set; }
        
        //Constructer
        public LogLine(String content)
        {
            time = DateTime.Now.ToString();
            this.content = content;
        }

       




    }
}
