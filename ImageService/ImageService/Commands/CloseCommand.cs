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
        #region members
        private ImageServer imageServer;
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        /// <returns></returns>
        public CloseCommand(ImageServer server)
        {
            this.imageServer = server;
        }

        /// <summary>
        /// Execute function.
        /// returns the logs list after convertion to json.
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="result">succes flag</param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                result = true;
                //check if there is no arguments in the args.
                if (args.Length == 0 || args == null)
                {
                    throw new Exception("there is no directory path in the close command");
                }
                StringBuilder newHandlers = new StringBuilder();
                string[] folders = ConfigurationManager.AppSettings.Get("Handler").Split(';');
                //concat all the folders to the newHandlers string.
                foreach (string folder in folders)
                {
                    if (folder != args[0])
                    {
                        newHandlers.Append(folder + ";");
                    }
                }
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //remove the handler row
                configuration.AppSettings.Settings.Remove("Handler");
                String newRowHandlers = newHandlers.ToString().TrimEnd(';');
                //add the new handlers row.
                configuration.AppSettings.Settings.Add("Handler", newRowHandlers);
                configuration.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
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

