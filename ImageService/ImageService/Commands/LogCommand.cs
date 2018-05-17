using ImageService.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging;
using Newtonsoft.Json;

namespace ImageService.ImageService.Commands
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
        public string Execute(string[] args, out bool result)
        {
            //return JsonConvert.SerializeObject(logCollectionSingleton);
        }
    }
}
