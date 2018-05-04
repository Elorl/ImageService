using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui
{
    public class LogItem
    { 
        public String Type { get; set; }
        public String Message { get; set; }

        public LogItem(string Type, string Message)
        {
            this.Type = Type;
            this.Message = Message;
        }
    }
}
