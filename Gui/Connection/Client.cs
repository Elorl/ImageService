using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Infrastracture.Events;
using Newtonsoft.Json;

namespace Gui.Connection
{
    public class Client
    {
        #region members
        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        #endregion

        #region events
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        #endregion

        /// <summary>
        /// initializes members and connects to server
        /// </summary>
        /// <param name="endPoint">end point to connect (server socket)</param>
        public void Start(IPEndPoint endPoint)
        {
            try
            {
                //Auto choose client socket (no params)
                client = new TcpClient();
                stream = client.GetStream();
                reader = new BinaryReader(stream);
                writer = new BinaryWriter(stream);
                client.Connect(endPoint);
                string rawData;
                CommandRecievedEventArgs commandArgs; 
                Task readTask = new Task(() => {
                    while(true)
                    {
                        rawData = reader.Read().ToString();

                        // todo : if it's close command $$$$$$$$$$
                        commandArgs = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(rawData);
                        CommandRecieved?.Invoke(this, commandArgs);
                    }
                    
                });
            }
            catch (Exception e)
            {
                // change to gui print? something else? $$$
                Console.Write(e.ToString());
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void SendCommand(CommandRecievedEventArgs args)
        {
            //needed because server may try to send log while we want to send request of setttings ??
            Task writeTask = new Task(()=>
            {
                writer.Write(JsonConvert.SerializeObject(args));
            });
        }
    }
}
