using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace Gui.models
{
    class SettingsModel : INotifyPropertyChanged
    {
        private string _OutputDir = String.Empty;
        private string _SourceName = String.Empty;
        private string _LogName = String.Empty;
        private int _ThumbnailSize;
        public ObservableCollection<string> handlers;
        
        #region events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        public SettingsModel()
        {
            this.handlers = new ObservableCollection<string>();

        }
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
        public int ThumbnailSize
        {
            set
            {
                this._ThumbnailSize = value;
                OnPropertyChanged("ThumbnailSize");
            }
            get { return this._ThumbnailSize; }
        }

    }
}
