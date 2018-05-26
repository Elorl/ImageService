using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using infrastructure.Events;
using infrastructure.Enums;

namespace Gui.Connection
{
    /// <summary>
    /// this class is a --SINGLETON-- aims to connect to ImageService server.
    /// Instantiated by all Models in Gui. 
    /// </summary>
    public class Client
    {
        #region members
        //single instance
        private static Client instance;
        private bool isOn;
        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        #endregion

        #region properties

        public static Client Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Client();
                }
                return instance;
            }
        }
        #endregion

        #region events
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        #endregion

        private Client(){ this.isOn = false; }

        /// <summary>
        /// initializes members and connects to server
        /// </summary>
        /// <param name="endPoint">end point to connect (server socket)</param>
        public bool Start()
        {
            if(this.isOn) { return true; }

            this.isOn = true;

            try
            {
                //Auto choose client socket (no params)
                client = new TcpClient();
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                client.Connect(endPoint);
                stream = client.GetStream();
                reader = new BinaryReader(stream);
                writer = new BinaryWriter(stream);
            }
            catch (Exception e) { return false; }
            string rawData;
            CommandRecievedEventArgs commandArgs;
            // ask for all logs by now
            CommandRecievedEventArgs logCommandArgs = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, "");
            string SerialArgs = JsonConvert.SerializeObject(logCommandArgs);
            writer.Write(SerialArgs);
            new Task(() => {
                while(true)
                {
                    try
                    {
                        rawData = reader.ReadString();

                        commandArgs = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(rawData);
                        CommandRecieved?.Invoke(this, commandArgs);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.ToString());
                    }
                }
            }).Start();
            return true;
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
                try
                {
                    writer.Write(JsonConvert.SerializeObject(args));
                } catch(Exception e) {;}
            });
            writeTask.Start();
        }
    }
}
