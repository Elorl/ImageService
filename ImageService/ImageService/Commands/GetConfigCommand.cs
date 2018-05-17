using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Infrastructure.Event;
using Newtonsoft.Json;
using 

namespace ImageService.ImageService.Commands
{
    class getAppConfig : ICommand
    {
        string[] args = new string[5];
        public string Execute()
        {
            args[0] = ConfigurationManager.AppSettings.Get("OutputDir");
            args[1] = ConfigurationManager.AppSettings.Get("SourceName");
            args[2] = ConfigurationManager.AppSettings.Get("LogName");
            args[3] = ConfigurationManager.AppSettings.Get("ThumbnailSize");
            args[4] = ConfigurationManager.AppSettings.Get("Handler");
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs(GetConfigCommand, args, "");
            
        }
    }
}
