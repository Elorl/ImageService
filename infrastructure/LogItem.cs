using infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure
{
    public class LogItem
    {
        public MessageTypeEnum Type { get; set; }
        public string Message { get; set; }

        public LogItem(MessageTypeEnum type, string message)
        {
            this.Type = type;
            this.Message = message;
        }
    }
}
