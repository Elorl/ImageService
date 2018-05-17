using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace Gui.SettingsVM
{
    interface ISettingsVM
    {
        string OutputDir { get; }
        string SourceName { get; }
        string LogName { get; }
        int ThumbnailSize { get; }
        ObservableCollection<string> Handlers { get; set; }
    }
}
