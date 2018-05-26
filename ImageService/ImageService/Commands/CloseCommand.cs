using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Server;
using System.Configuration;

namespace ImageService.Commands
{
    class CloseCommand : ICommand
    {
        private ImageServer imageServer;
        public CloseCommand(ImageServer server)
        {
            this.imageServer = server;
        }


        public string Execute(string[] args, out bool result)
        {
            try
            {
                result = true;
                if (args.Length == 0)
                {
                    throw new Exception("there is no directory path in the close command");
                }
                StringBuilder newHandlers = new StringBuilder();
                string[] folders = ConfigurationManager.AppSettings.Get("Handler").Split(';');
                foreach (string folder in folders)
                {
                    if (folder != args[0])
                    {
                        newHandlers.Append(folder + ";");
                    }
                }
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configuration.AppSettings.Settings.Remove("Handler");
                String newRowHandlers = newHandlers.ToString().TrimEnd(';');
                configuration.AppSettings.Settings.Add("Handler", newRowHandlers);
                configuration.Save(ConfigurationSaveMode.Modified);
                this.imageServer.CloseHandler(args[0]);
                return "directory" + args[0] + "successfully closed.";
            }
            catch
            {
                result = false;
                return "close command have been failed";
            }
        }
    }
}

