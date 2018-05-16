using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel; 

namespace ImageService.Logging
{
    class LogCollectionSingelton
    {
        private static LogCollectionSingelton instance;
        public ObservableCollection<Log> list { get; private set; }

        private LogCollectionSingelton() { } 

        public static LogCollectionSingelton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LogCollectionSingelton();
                }
                return instance;
            }
        }
    }
}
