using infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    public class MessageRecievedEventArgs : EventArgs
    {
        private string message;
        private MessageTypeEnum status;
        public MessageRecievedEventArgs(string message, MessageTypeEnum status)
        {
            this.message = message;
            this.status = status;
        }

        public MessageTypeEnum Status
        {
            get
            {
                return status;
            }
            set
            {
                this.status = value;
            }
        }
        public string Message {
            get
            {
                return message;
            }
            set
            {
                this.message = value;
            }
        }
    }
}
