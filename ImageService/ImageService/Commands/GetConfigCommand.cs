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
    public class GetConfigCommand : ICommand
    {
        string[] argsConfig = new string[5];
        public string Execute(string[] args, out bool result)
        {
            argsConfig[0] = ConfigurationManager.AppSettings.Get("OutputDir");
            argsConfig[1] = ConfigurationManager.AppSettings.Get("SourceName");
            argsConfig[2] = ConfigurationManager.AppSettings.Get("LogName");
            argsConfig[3] = ConfigurationManager.AppSettings.Get("ThumbnailSize");
            argsConfig[4] = ConfigurationManager.AppSettings.Get("Handler");
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, argsConfig, "");
            result = true;
            return JsonConvert.SerializeObject(commandRecievedEventArgs);
        }
    }
}

