using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Gui.models;

namespace Gui.settingsVM
{
    class SettingsVM: ISettingsVM, INotifyPropertyChanged
    {
        private SettingsModel SettingsModel;
        #region events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        private string _SelectedItem;
        public SettingsVM()
        {
            this.SettingsModel = new SettingsModel();

        }

        public string OutputDir
        {
            get { return this.SettingsModel.OutputDir; }
        }
        public string SourceName
        {
            get { return this.SettingsModel.SourceName; }
        }
        public string LogName
        {
            get { return this.SettingsModel.LogName; }
        }
        public int ThumbnailSize
        {
            get { return this.SettingsModel.ThumbnailSize; }
        }
        public string SelectedItem
        {
            get
            {
                return this._SelectedItem;
            }
            set
            {
                this._SelectedItem = value;
            }
        }
        public ObservableCollection<string> Handlers
        {
            get
            {
                return this.SettingsModel.handlers;
            }
            set => throw new NotImplementedException();
        }

        private void NotifyPropertyChanged(string Name)
        {
            PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs(Name);
            this.PropertyChanged?.Invoke(this, propertyChangedEventArgs);
        }
    }
}
