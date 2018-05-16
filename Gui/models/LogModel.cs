using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Infrastracture.Enums;

namespace Gui.models
{
    public class LogModel : INotifyPropertyChanged
    {
        #region properties
        public ObservableCollection<LogItem> LogsCollection { get; }
        #endregion

        // can't find a usage for it right now. maybe further.. $$$$$$$$
        #region events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public LogModel()
        {
            this.LogsCollection = new ObservableCollection<LogItem>();

            //testing binding $$$$$$$$$
            this.LogsCollection.Add(new LogItem(MessageTypeEnum.INFO, "c is too #"));
            this.LogsCollection.Add(new LogItem(MessageTypeEnum.INFO, "Pleasse help me H'"));
            this.LogsCollection.Add(new LogItem(MessageTypeEnum.FAIL, "some Log"));
            this.LogsCollection.Add(new LogItem(MessageTypeEnum.INFO, "I love Linux"));
            this.LogsCollection.Add(new LogItem(MessageTypeEnum.INFO, "some Dog"));
            this.LogsCollection.Add(new LogItem(MessageTypeEnum.FAIL, "Microsoft is JIFFA"));
        }
    }
}
