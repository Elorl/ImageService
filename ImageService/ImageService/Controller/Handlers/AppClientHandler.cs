using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using infrastructure.Enums;
using infrastructure.Events;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using System.IO;
using ImageService.Controller;
using System.Net.Sockets;
using System.Net;
using ImageService.Controller.Handlers;
namespace ImageService.Controller.Handlers
{
    class AppClientHandler: IAppClientHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private NetworkStream stream;
        #endregion

        #region event
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="controller"> image controller</param>
        /// <param name="logging"> the logger</param>
        /// <param name="path"> the logger</param>
        public AppClientHandler(IImageController controller, ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;

        }

        /// <summary>
        /// HandleClient.
        /// </summary>
        /// <param name="client">TcpClient</param>
        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                while (client.Connected)
                {
                    try
                    {
                        m_logging.Log("start handle with app-client", MessageTypeEnum.INFO);
                        //get the stream
                        stream = client.GetStream();
                        string fileName = GetImageName(stream);
                        Byte[] success = new byte[1] { 1 };
                        //check if the client done with sending images.
                        if (fileName.Equals("DONE"))
                        {
                            m_logging.Log("All app-client images have been transfered.", MessageTypeEnum.INFO);
                            break;
                        }
                        stream.Write(success, 0, 1);
                        //get the image.
                        Byte[] image = GetImage(stream);
                        stream.Write(success, 0, 1);
                        //get the first dir the service listen to.
                        string dir = m_controller.ImageServer.folders[0];
                        File.WriteAllBytes(Path.Combine(dir, fileName), image);
                        m_logging.Log("Photo transfered", MessageTypeEnum.INFO);
                    } catch (Exception e)
                    {
                        m_logging.Log("Error transfer images from App-client.", MessageTypeEnum.WARNING);
                        break;
                    }

                }
                //close the stream and client.
                stream.Close();
                client.Close();
            }).Start();
        }

        /// <summary>
        /// GetImageName.
        /// </summary>
        /// <param name="stream">NetworkStream stream</param>
        private string GetImageName(NetworkStream stream)
        {
            Byte[] currentRead = new byte[1];
            List<byte> input = new List<byte>();
            do
            {
                stream.Read(currentRead, 0, 1);
                input.Add(currentRead[0]);
            } while (stream.DataAvailable);
            //convert the input to string.
            return Encoding.ASCII.GetString(input.ToArray(), 0, input.ToArray().Length);
        }

        /// <summary>
        /// GetSize.
        /// </summary>
        /// <param name="stream">NetworkStream stream</param>
        private int GetSize(NetworkStream stream)
        {
            Byte[] currentRead = new byte[1];
            List<byte> input = new List<byte>();
            do
            {
                stream.Read(currentRead, 0, 1);
                input.Add(currentRead[0]);
            } while (stream.DataAvailable);
            //convert the bytes to string.
            string sizeString = Encoding.ASCII.GetString(input.ToArray(), 0, input.ToArray().Length);
            int size;
            //convert the string to int.
            int.TryParse(sizeString, out size);
            Byte[] success = new byte[1] { 1 };
            stream.Write(success, 0, 1);
            return size;
        }

        /// <summary>
        /// GetImage.
        /// </summary>
        /// <param name="stream">NetworkStream stream</param>
        private Byte[] GetImage(NetworkStream stream)
        {
            int size = GetSize(stream);
            Byte[] read = new byte[size];
            int bytesReadSum = stream.Read(read, 0, size);
            //get the bytes of the image.
            while (size > bytesReadSum)
            {
                bytesReadSum += stream.Read(read, bytesReadSum, size - bytesReadSum);
            }
            return read;
        }
    }
}
