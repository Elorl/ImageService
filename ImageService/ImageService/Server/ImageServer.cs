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

       public ImageServer(IImageController controller, ILoggingService logging)
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
            handler.DirectoryClose += removeHandler;
            handler.StartHandleDirectory(folder);
            this.m_logging.Log("start watch the directory: " + folder, Logging.Modal.MessageTypeEnum.INFO);
        }

        public void CloseServer()
        {
            // invoking commandRecieved event with a close command arg
            this.m_logging.Log("CloseServer start", Logging.Modal.MessageTypeEnum.INFO);
            CommandRecievedEventArgs args = new CommandRecievedEventArgs( (int)CommandEnum.CloseCommand, null, null );
            this.CommandRecieved?.Invoke(this, args);
            this.m_logging.Log("server closed", Logging.Modal.MessageTypeEnum.INFO);
        }

        /// <summary>
        /// remove handler from CommandRecieved event.
        /// </summary>
        public void removeHandler(object source, DirectoryCloseEventArgs args) 
        {
            IDirectoryHandler toRemove = (IDirectoryHandler)source; 
            this.CommandRecieved -= toRemove.OnCommandRecieved;
            this.m_logging.Log(args.Message, Logging.Modal.MessageTypeEnum.INFO);
            this.m_logging.Log("Handler closed", Logging.Modal.MessageTypeEnum.INFO);
        }
    }
}
