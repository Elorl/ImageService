using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using infrastructure;

namespace ImageService.Logging
{
    public class LogCollectionSingleton
    {
        private static LogCollectionSingleton instance;
        public ObservableCollection<LogItem> LogsCollection { get; private set; }

        private LogCollectionSingleton() { } 

        public static LogCollectionSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LogCollectionSingleton();
                }
                return instance;
            }
        }
    }
}
