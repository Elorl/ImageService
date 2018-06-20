using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ImageService.Server;
using ImageService.Controller;
using ImageService.Modal;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Configuration;
using infrastructure.Enums;
using infrastructure;
using ImageService.Controller.Handlers;


namespace ImageService
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };

    public partial class ImageService : ServiceBase
    {
        private System.ComponentModel.IContainer components; 
        private System.Diagnostics.EventLog eventLog1;
        private int eventId = 1;
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        private IImageServiceModal model;
        private IImageController controller;
        private ImageServer m_imageServer;          // The Image Server
        private Logging.ILoggingService logger;
        private LogCollectionSingleton logCollectionSingleton;
        private AppServer m_appServer;

        public ImageService(string[] args)
        {
            InitializeComponent();
            this.logCollectionSingleton = LogCollectionSingleton.Instance;
            eventLog1 = new System.Diagnostics.EventLog();
            //app config values
            string eventSourceName = ConfigurationManager.AppSettings.Get("SourceName");
            string logName = ConfigurationManager.AppSettings.Get("LogName");
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
            this.logger = new LoggingService();
            //register this messageRecived function to the MessageRecieved event in the logger
            this.logger.MessageRecieved += MessageRecievedOperation;
            this.model = new ImageServiceModal()
            {
                OutputFolder = ConfigurationManager.AppSettings.Get("OutputDir"),
                ThumbnailSize = Int32.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"))
            };
            this.controller = new ImageController(this.model);
            this.m_imageServer = new ImageServer(this.controller, this.logger);
            this.controller.ImageServer = this.m_imageServer;
            this.m_appServer = new AppServer(this.controller, this.logger);
        }
        /// <summary>
        /// beeing activated in service start
        /// </summary>
        /// <param name="args">arguments</param>
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
            eventLog1.WriteEntry("In OnStart");
            logCollectionSingleton.LogsCollection.Add(new LogItem(MessageTypeEnum.INFO, "In OnStart"));
            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }
        /// <summary>
        /// beeing activated in service stop
        /// </summary>
        protected override void OnStop()
        {
            //DirectoryCloseEventArgs server and update log info
            eventLog1.WriteEntry("In onStop.");
            logCollectionSingleton.LogsCollection.Add(new LogItem(MessageTypeEnum.INFO, "In onStop."));
            this.m_imageServer.CloseServer();
            eventLog1.WriteEntry("everything stoped.");
            logCollectionSingleton.LogsCollection.Add(new LogItem(MessageTypeEnum.INFO, "everything stoped."));
        }
        /// <summary>
        /// activated when timer invokes
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">args of the invokation</param>
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
            logCollectionSingleton.LogsCollection.Add(new LogItem(MessageTypeEnum.INFO, "Monitoring the System"));
        }

        /// <summary>
        /// when continuing service
        /// </summary>
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
            logCollectionSingleton.LogsCollection.Add(new LogItem(MessageTypeEnum.INFO, "In OnContinue."));
        }
        /// <summary>
        /// ivoked by event and wties to system log. specifically here by logger's event of messaging
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">arguments</param>
        private void MessageRecievedOperation(object sender, MessageRecievedEventArgs args)
        {
            // get message type
            EventLogEntryType messageType;
            switch (args.Status)
            {
                case MessageTypeEnum.WARNING: 
                    messageType = EventLogEntryType.Warning; break;
                case MessageTypeEnum.INFO:
                    messageType = EventLogEntryType.Information; break;
                case MessageTypeEnum.FAIL:
                    messageType = EventLogEntryType.Error; break;
                default:
                    messageType = EventLogEntryType.Information; break;
            }
            eventLog1.WriteEntry(args.Message, messageType);
            logCollectionSingleton.LogsCollection.Add(new LogItem(args.Status, args.Message));
        }

        public void RunAsConsole(string[] args)
        {
            OnStart(args);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            OnStop();
        }
    }
}
