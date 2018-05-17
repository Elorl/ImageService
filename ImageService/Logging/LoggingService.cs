
using ImageService.Logging.Modal;
using infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        #region properties
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        #endregion
        /// <summary>
        /// logging info to all subscribers.
        /// </summary>
        /// <param name="message">message for logging</param>
        /// <param name="type">type of message</param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(message, type));
        }
    }
}
