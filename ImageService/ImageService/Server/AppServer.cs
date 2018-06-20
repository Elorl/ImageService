using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Logging;
using System.Net.Sockets;
using System.Net;
using infrastructure.Events;
using infrastructure.Enums;
using System.IO;
using System.Threading;
using System.Collections.Specialized;
using System.Configuration;
using Newtonsoft.Json;

namespace ImageService.Server
{
    class AppServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        private TcpListener tcpListener;
        private List<TcpClient> clientsList;
        #endregion
        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        public static Mutex WriteMutex { get; set; } = new Mutex();
        public static Mutex ReadMutex { get; set; } = new Mutex();
        public static Mutex ClientsListMutex { get; set; } = new Mutex();
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="controller">controller</param>
        /// <param name="logging"> logger</param>
        public AppServer(IImageController controller, ILoggingService logging)
        {
            try
            {
                this.m_controller = controller;
                this.m_logging = logging;
                //establish a tcp connection
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8200);
                this.tcpListener = new TcpListener(ep);
                this.tcpListener.Start();
            }
            catch (Exception e) { this.m_logging.Log("Couldn't establish app server: " + e.ToString(), MessageTypeEnum.FAIL); }
            this.m_logging.Log("Tcp App - server established: ", MessageTypeEnum.INFO);
            this.clientsList = new List<TcpClient>();
            AcceptClients();
        }
        /// <summary>
        /// AcceptClients.
        /// accept new clients that ask to connect to the server.
        /// </summary>
        /// <param name="controller">controller</param>
        /// <param name="logging"> logger</param>
        public void AcceptClients()
        {
            new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = this.tcpListener.AcceptTcpClient();
                        clientsList.Add(client);
                        IAppClientHandler clientHandler = new AppClientHandler(m_controller, m_logging);
                        clientHandler.HandleClient(client);
                    }
                    catch (Exception e)
                    {
                        this.m_logging.Log("failed connecting a App-client", MessageTypeEnum.FAIL);
                    }
                }

            }).Start();
        }







    }
}
