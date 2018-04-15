using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion

       public ImageServer(ImageController controller, ILoggingService logging)
        {
            string[] folders;
            this.m_controller = controller;
            this.m_logging = logging;
            folders = ConfigurationManager.AppSettings["Handler"].Split(';');

            foreach(string folder in folders)
            {
                try
                {
                    IDirectoryHandler handler = new DirectoyHandler(this.m_controller, this.m_logging, folder);
                    createHandler(folder);
                }catch(Exception exception)
                {
                    this.m_logging.Log("failed to listen to the folder: " + folder, Logging.Modal.MessageTypeEnum.FAIL);
                }
            }
        }
        private void createHandler(string folder)
        {
            IDirectoryHandler handler = new DirectoyHandler(this.m_controller, this.m_logging, folder);
            this.CommandRecieved += handler.OnCommandRecieved;
            handler.StartHandleDirectory(folder);
            this.m_logging.Log("start watch the directory: " + folder, Logging.Modal.MessageTypeEnum.INFO);
        }
    }
}
