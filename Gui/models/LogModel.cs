﻿using Gui.Connection;
using infrastructure;
using infrastructure.Enums;
using infrastructure.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;


namespace Gui.models
{
    public class LogModel 
    {
        #region members
        private object logsCollectionLock;
        private ObservableCollection<LogItem> logsCollection;
        private Client client;
        #endregion
        #region properties
        public ObservableCollection<LogItem> LogsCollection
        { get { return logsCollection; }
            set
            {  logsCollection = value; }
        }
        #endregion

        public LogModel()
        {
            this.LogsCollection = new ObservableCollection<LogItem>();
            this.logsCollectionLock = new object();
            BindingOperations.EnableCollectionSynchronization(logsCollection, logsCollectionLock);
            this.client = Client.Instance;
            this.client.CommandRecieved += LogModel_CommandRecieved;
            this.client.Start();
            //testing binding $$$$$$$$$
            this.LogsCollection.Add(new LogItem(MessageTypeEnum.INFO, "c is too #"));
            this.LogsCollection.Add(new LogItem(MessageTypeEnum.INFO, "Pleasse help me H'"));
            this.LogsCollection.Add(new LogItem(MessageTypeEnum.FAIL, "some Log"));
            this.LogsCollection.Add(new LogItem(MessageTypeEnum.INFO, "I love Linux"));
            this.LogsCollection.Add(new LogItem(MessageTypeEnum.INFO, "some Dog"));
            this.LogsCollection.Add(new LogItem(MessageTypeEnum.FAIL, "Microsoft is JIFFA"));
            this.LogsCollection.Add(new LogItem(MessageTypeEnum.WARNING, "Microsoft is JIFFA"));
        }

        public void LogModel_CommandRecieved(object sender, CommandRecievedEventArgs args)
        {
            //ignore if it isn't a log command
            if (args.CommandID != (int)CommandEnum.LogCommand) { return; }

            IList<LogItem> newItems = JsonConvert.DeserializeObject<IList<LogItem>>(args.Args[0]);
            //add new log entries to the collection
            foreach(LogItem item in newItems)
            {
                this.logsCollection.Add(item);
            }
        }
    }
}
