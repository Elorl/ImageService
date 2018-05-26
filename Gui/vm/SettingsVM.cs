using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Gui.models;
using Prism.Commands;
using System.Windows.Input;
using infrastructure.Events;
using infrastructure.Enums;
namespace Gui.settingsVM
{
    class SettingsVM: ISettingsVM, INotifyPropertyChanged
    {
        private SettingsModel SettingsModel;
        public ICommand RemoveCommand { get; private set; }
        #region events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        private string _SelectedItem;
        public SettingsVM()
        {
            this.SettingsModel = new SettingsModel();
            this.SettingsModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, CanRemove);
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
        public string ThumbnailSize
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
                var command = this.RemoveCommand as DelegateCommand<object>;
                command.RaiseCanExecuteChanged();
            }
        }
        public ObservableCollection<string> Handlers
        {
            get
            {
                return this.SettingsModel.handlers;
            }
            set
            {

            }
        }

        private void NotifyPropertyChanged(string Name)
        {
            PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs(Name);
            this.PropertyChanged?.Invoke(this, propertyChangedEventArgs);
        }

        private void OnRemove(Object objct)
        {
            try
            {
                string[] selected = { this.SelectedItem };
                CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, selected, "");
                this.SettingsModel.SendCommand(command);
            } catch(Exception e)
            {
                
            }
        }

        private bool CanRemove(object obj)
        {
            if(this.SelectedItem == null || this.SelectedItem.Length == 0)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
