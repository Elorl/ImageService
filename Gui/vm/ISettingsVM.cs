using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace Gui.settingsVM
{
    interface ISettingsVM
    {
        string OutputDirVM { get; }
        string SourceNameVM { get; }
        string LogNameVM { get; }
        string ThumbnailSizeVM { get; }
        ObservableCollection<string> Handlers { get; set; }
    }
}
