using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gui.models;
using System.ComponentModel;
using Infrastracture.Enums;
namespace Gui.vm
{
    public class LogVm
    {
 

        #region properties
        public ObservableCollection<LogItem> LogsCollection { set; get; }
        #endregion

        #region members
        private LogModel model;
        #endregion

        public LogVm()
        {
            this.model = new LogModel();
            this.LogsCollection = this.model.LogsCollection;
        }
    }
}
