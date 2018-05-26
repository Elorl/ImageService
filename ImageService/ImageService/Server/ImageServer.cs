using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Logging;
using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using infrastructure.Events;
using infrastructure.Enums;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        private TcpListener tcpListener;
        private List<TcpClient> clientsList;
        private Dictionary<string, IDirectoryHandler> handlers;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        public static Mutex WriteMutex { get; set; } = new Mutex();
        public static Mutex ReadMutex { get; set; } = new Mutex();
        #endregion
        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="controller">controller</param>
        /// <param name="logging"> logger</param>
        public ImageServer(IImageController controller, ILoggingService logging)
        {
            string[] folders;
            try {
                this.m_controller = controller;
                this.m_logging = logging;
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                this.tcpListener = new TcpListener(ep);
                this.tcpListener.Start();
            } catch (Exception e) { Debug.Write(e.ToString()); }
            this.clientsList = new List<TcpClient>();
            LogCollectionSingleton.Instance.LogsCollection.CollectionChanged += ImageServer_LogCollectionChanged;
            folders = ConfigurationManager.AppSettings["Handler"].Split(';');
            
            //creates handler for each given folder
            foreach(string folder in folders)
            {
                try
                {
                    IDirectoryHandler handler = new DirectoyHandler(this.m_controller, this.m_logging, folder);
                    createHandler(folder);
                }catch(Exception exception)
                {
                    this.m_logging.Log("failed to listen to the folder: " + folder + exception, MessageTypeEnum.FAIL);
                }
            }
            AcceptClients();
        }


        public void AcceptClients()
        {
            new Task(() => {
                while (true)
                {
                    TcpClient client = this.tcpListener.AcceptTcpClient();
                    clientsList.Add(client);
                    HandleClient(client);
                }

            }).Start();
        }

        private void HandleClient(TcpClient client)
        {
            new Task(() =>
               {
                   NetworkStream stream = client.GetStream();
                   BinaryReader reader = new BinaryReader(stream);
                   BinaryWriter writer = new BinaryWriter(stream);
                   bool successFlag;
                   string result;
                   while (true)
                   {
                        //todo HOW TO DISCONNECT CLIENT FROM SERVER $$$
                       ReadMutex.WaitOne();
                       String rawData = reader.ReadString();
                       ReadMutex.ReleaseMutex();
                       CommandRecievedEventArgs commandArgs = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(rawData);

                       result = m_controller.ExecuteCommand(commandArgs.CommandID, commandArgs.Args, out successFlag);
                       WriteMutex.WaitOne();
                       writer.Write(result);
                       WriteMutex.ReleaseMutex();
                   }
               }).Start();
        }

        private void NotifyChangeToAllClients(CommandRecievedEventArgs args)
        {
            
            foreach (TcpClient client in clientsList)
            {
                new Task(()=> 
                {
                    NetworkStream stream = client.GetStream();
                    BinaryReader reader = new BinaryReader(stream);
                    BinaryWriter writer = new BinaryWriter(stream);

                    WriteMutex.WaitOne();
                    writer.Write(JsonConvert.SerializeObject(args));
                    WriteMutex.ReleaseMutex();
                }).Start();
            }
        }

        /// <summary>
        /// creates handler given a folder
        /// </summary>
        /// <param name="folder">folder to be handled</param>
        private void createHandler(string folder)
        {
            IDirectoryHandler handler = new DirectoyHandler(this.m_controller, this.m_logging, folder);
            this.handlers.Add(folder, handler);
            this.CommandRecieved += handler.OnCommandRecieved;
            handler.DirectoryClose += removeHandler;
            handler.StartHandleDirectory(folder);
            this.m_logging.Log("start watch the directory: " + folder, MessageTypeEnum.INFO);
        }
        /// <summary>
        /// closing the server and notifies components related to the service operation.
        /// </summary>
        public void CloseServer()
        {
            // invoking commandRecieved event with a close command arg
            this.m_logging.Log("CloseServer start", MessageTypeEnum.INFO);
            CommandRecievedEventArgs args = new CommandRecievedEventArgs( (int)CommandEnum.CloseCommand, null, null );
            this.CommandRecieved?.Invoke(this, args);
            this.m_logging.Log("server closed", MessageTypeEnum.INFO);
        }

        /// <summary>
        /// remove handler from CommandRecieved event.
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="args">arguments</param>
        public void removeHandler(object source, DirectoryCloseEventArgs args) 
        {
            IDirectoryHandler toRemove = (IDirectoryHandler)source; 
            this.CommandRecieved -= toRemove.OnCommandRecieved;
            this.m_logging.Log(args.Message, MessageTypeEnum.INFO);
            this.m_logging.Log("Handler closed", MessageTypeEnum.INFO);
        }

        public void ImageServer_LogCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
        {
            string[] commandArgs = new string[1];
            commandArgs[0] = JsonConvert.SerializeObject(e.NewItems);
            CommandRecievedEventArgs args = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, commandArgs, "");
            this.NotifyChangeToAllClients(args);
        }
        
        public void CloseHandler(string handlerPath)
        {
            if(this.handlers.ContainsKey(handlerPath))
            {
                IDirectoryHandler handler = handlers[handlerPath];
                handler.EndHandle();
                string[] args = new string[1];
                args[0] = handlerPath;
                CommandRecievedEventArgs close = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, args, "");
                NotifyChangeToAllClients(close);
            }
        }
    }
}
