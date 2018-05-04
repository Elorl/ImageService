using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Gui;
using Infrastructure.Enums;
using Infrastracture.Enums;
using Gui.vm;

namespace Gui.models
{
    public class LogModel
    {

        #region Properties
        public ObservableCollection<LogItem> LogsCollection { get; }
        #endregion

        public LogModel()
        {
            //todo : add request from server to all the logs wtitten by now @#@#@#$#$%#$@#@#@#
            this.LogsCollection = new ObservableCollection<LogItem>();
            this.LogsCollection.Add(new LogItem("INFO", "bla"));

        }
    }
}
