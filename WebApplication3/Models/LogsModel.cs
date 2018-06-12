using Connection;
using infrastructure;
using infrastructure.Enums;
using infrastructure.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class LogsModel
    {
        #region properties
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LogsList")]
        public List<LogItem> LogsList { get; set; }
        #endregion
        #region events
        public EventHandler LogRecievedEvent;
        #endregion

        public LogsModel()
        {
            LogsList = new List<LogItem>();
            Client client = Client.Instance;
            client.CommandRecieved += LogRecieved;
            client.Start();
            //request all logs by far
            CommandRecievedEventArgs logCommandArgs = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, "");
            client.SendCommand(logCommandArgs);
        }

        private void LogRecieved(object sender, CommandRecievedEventArgs args)
        {
            //ignore if it isn't a log command
            if (args.CommandID != (int)CommandEnum.LogCommand) { return; }

            IList<LogItem> newItems = JsonConvert.DeserializeObject<IList<LogItem>>(args.Args[0]);
            //add new log entries to the list
            foreach (LogItem item in newItems)
            {
                this.LogsList.Add(item);

            }
            LogRecievedEvent?.Invoke(this, null);
        }
    }
}