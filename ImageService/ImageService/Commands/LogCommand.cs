using ImageService.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging;
using Newtonsoft.Json;
using infrastructure.Events;
using infrastructure.Enums;
using infrastructure;

namespace ImageService.Commands
{
    public class LogCommand : ICommand
    {
        #region members
        LogCollectionSingleton logCollectionSingleton;
        #endregion

        public LogCommand()
        {
            this.logCollectionSingleton = LogCollectionSingleton.Instance;
        }
        /// <summary>
        /// returns the logs list after convertion to json.
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="result">succes flag</param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            result = true;
            try
            {
                string[] str = new string[1];
                str[0] = JsonConvert.SerializeObject(logCollectionSingleton.LogsCollection.ToList());
                CommandRecievedEventArgs argsToReturn = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, str, "");
                return JsonConvert.SerializeObject(argsToReturn);
            } catch(Exception e)
            {
                result = false;
                return e.ToString();
            }
        }
    }
}
