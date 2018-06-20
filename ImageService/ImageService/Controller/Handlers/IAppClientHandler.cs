using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace ImageService.Controller.Handlers
{
    public interface IAppClientHandler
    {
        void HandleClient(TcpClient client);
    }
}
