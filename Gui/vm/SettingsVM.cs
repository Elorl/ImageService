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
    class SettingsVM : ISettingsVM, INotifyPropertyChanged
    {
        #region members
        private SettingsModel SettingsModel;
        private string _SelectedItem;
        #endregion

        #region properties
        public ICommand RemoveCommand { get; private set; }
        #endregion

        #region events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        /// <summary>
        /// constructor.
        /// </summary>
        /// <returns></returns>
        public SettingsVM()
        {
            this.SettingsModel = new SettingsModel();
            this.SettingsModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged(e.PropertyName + "VM");
            };
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, CanRemove);
        }

        public string OutputDirVM
        {
            get { return this.SettingsModel.OutputDir; }
        }
        public string SourceNameVM
        {
            get { return this.SettingsModel.SourceName; }
        }
        public string LogNameVM
        {
            get { return this.SettingsModel.LogName; }
        }
        public string ThumbnailSizeVM
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

        /// <summary>
        /// NotifyPropertyChanged function.
        /// notify if there is a change with the properties vlue.
        /// </summary>
        /// <param name="Name"></param>
        private void NotifyPropertyChanged(string Name)
        {
            PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs(Name);
            this.PropertyChanged?.Invoke(this, propertyChangedEventArgs);
        }

        /// <summary>
        /// OnRemove function.
        /// get the selection from the user to delete
        /// and send the command to the server.
        /// </summary>
        /// <param name="obj"></param>
        private void OnRemove(Object obj)
        {
            try
            {
                string[] selected = { this.SelectedItem };
                CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, selected, "");
                this.SettingsModel.SendCommand(command);
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// CanRemove function.
        /// check if the remove can be done.
        /// </summary>
        /// <param name="obj"></param>
        private bool CanRemove(object obj)
        {
            if (this.SelectedItem == null || this.SelectedItem.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
