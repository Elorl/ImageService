using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Server;

namespace ImageService.Controller
{
    public interface IImageController
    {
        ImageServer ImageServer { set; }
        string ExecuteCommand(int commandID, string[] args, out bool result);          // Executing the Command Requet
    }
}

