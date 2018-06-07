using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Connection;
using infrastructure;
using infrastructure.Enums;
using infrastructure.Events;
using Newtonsoft.Json;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace Gui.models
{
    class SettingsModel : INotifyPropertyChanged
    {
        #region members
        private string _OutputDir;
        private string _SourceName;
        private string _LogName;
        private string _ThumbnailSize;
        public ObservableCollection<string> handlers;
        private Client client;
        private object SettingsCollectionLock;
        #endregion

        #region events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        /// <returns></returns>
        public SettingsModel()
        {
            this.handlers = new ObservableCollection<string>();
            this.SettingsCollectionLock = new object();
            BindingOperations.EnableCollectionSynchronization(handlers, SettingsCollectionLock);
            this.client = Client.Instance;
            this.client.CommandRecieved += SettingsModel_CommandRecieved;
            bool returnVal ;
            returnVal = this.client.Start();
            //check if there is an error while connecting to the server.
            if(!returnVal)
            {
                Application.Current.MainWindow.Background = new SolidColorBrush(Colors.DarkGray);

            }
            this._OutputDir = string.Empty;
            this._OutputDir = string.Empty;
            this._LogName = string.Empty;
            this._ThumbnailSize = string.Empty;
            //send the getConfig command to get the app.config info.
            CommandRecievedEventArgs initializeSettings = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, null, "");
            this.client.SendCommand(initializeSettings);
        }

        /// <summary>
        /// SendCommand function.
        /// send the command to the client(to send to the server).
        /// </summary>
        /// <param name="command"></param>
        public void SendCommand(CommandRecievedEventArgs command)
        {
            this.client.SendCommand(command);
        }

        /// <summary>
        /// OnPropertyChanged function.
        /// run the propertychange func if there is a change.
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public string OutputDir
        {
            set
            {
                this._OutputDir = value;
                OnPropertyChanged("OutputDir");
            }
            get { return this._OutputDir; }
        }
        public string SourceName
        {
            set
            {
                this._SourceName = value;
                OnPropertyChanged("SourceName");
            }
            get { return this._SourceName; }
        }
        public string LogName
        {
            set
            {
                this._LogName = value;
                OnPropertyChanged("LogName");
            }
            get { return this._LogName; }
        }
        public string ThumbnailSize
        {
            set
            {
                this._ThumbnailSize = value;
                OnPropertyChanged("ThumbnailSize");
            }
            get { return this._ThumbnailSize; }
        }

        /// <summary>
        /// SettingsModel_CommandRecieved function.
        /// handle with arrived command from the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void SettingsModel_CommandRecieved(object sender, CommandRecievedEventArgs args)
        {
            if (args.CommandID == (int)CommandEnum.GetConfigCommand)
            {
                GetConfigHandle(args);
            }
            else if (args.CommandID == (int)CommandEnum.CloseCommand)
            {
                CloseCommandHandle(args);
            }
        }

        /// <summary>
        /// GetConfigHandle function.
        /// handle with a getConfig command.
        /// </summary>
        /// <param name="args"></param>
        private void GetConfigHandle(CommandRecievedEventArgs args)
        {
            this.OutputDir = args.Args[0];
            this.SourceName = args.Args[1];
            this.LogName = args.Args[2];
            this.ThumbnailSize = args.Args[3];
            string[] incomeDirs = args.Args[4].Split(';');
            //add all the folders to the list.
            foreach (string dir in incomeDirs)
            {
                this.handlers.Add(dir);
            }
        }

        /// <summary>
        /// CloseCommandHandle function.
        /// handle with a clsoe command.
        /// </summary>
        /// <param name="args"></param>
        private void CloseCommandHandle(CommandRecievedEventArgs args)
        {
            if (this.handlers.Contains(args.Args[0]))
            {
                this.handlers.Remove(args.Args[0]);
            }
        }
    }
}
