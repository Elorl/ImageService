using Gui.models;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.vm
{
    public class MainWindowVM
    {
        #region members
        MainWindowModel mainWindowModel;
        #endregion
        #region properties
        public bool IsConnected
        {
           get{ return this.mainWindowModel.IsConnected; }
        }
        public Color Backgroundcolor
        {
            get
            {
                if (IsConnected) { return Colors.White; }
                else { return Colors.DarkGray; }
            }
        }
        #endregion

       public MainWindowVM()
        {
            this.mainWindowModel = new MainWindowModel();
        }
    }
}
