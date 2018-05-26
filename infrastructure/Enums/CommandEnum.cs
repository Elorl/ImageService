using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace infrastructure.Enums
{
    public enum CommandEnum : int
    {
        NewFileCommand = 0,
        CloseCommand = 1,
        GetConfigCommand = 2,
        LogCommand = 3,
    }
}
