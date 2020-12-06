using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveV2.Model
{
    public class LogLine
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
