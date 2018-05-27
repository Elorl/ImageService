using Gui.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.models
{
    public class MainWindowModel
    {
        #region properties
        //check if connected to server.
        public bool IsConnected
        {
           get{ return Client.Instance.isOn; }
        }
        #endregion
    }
}
