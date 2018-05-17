using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using infrastructure.Events;
using infrastructure.Enums;
using Newtonsoft.Json;

namespace ImageService.Commands
{
    class getAppConfig : ICommand
    {
        string[] args = new string[5];
        public string Execute(string[] args, out bool result)
        {
            args[0] = ConfigurationManager.AppSettings.Get("OutputDir");
            args[1] = ConfigurationManager.AppSettings.Get("SourceName");
            args[2] = ConfigurationManager.AppSettings.Get("LogName");
            args[3] = ConfigurationManager.AppSettings.Get("ThumbnailSize");
            args[4] = ConfigurationManager.AppSettings.Get("Handler");
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, args, "");
            result = true;
            return JsonConvert.SerializeObject(commandRecievedEventArgs);
        }
    }
}
