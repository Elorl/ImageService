using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;
using infrastructure.Enums;
using infrastructure.Events;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;    // The Path of directory
        private string[] extensions = {".jpg", ".png", ".gif", ".bmp"};
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
        public DirectoyHandler(IImageController controller, ILoggingService logging, string path)
        {
            this.m_controller = controller;
            this.m_logging = logging;
            this.m_path = path;
            this.m_dirWatcher = new FileSystemWatcher(path);
        }

        /// <summary>
        /// StartHandleDirectory function.
        /// start handle a new directory.
        /// </summary>
        /// <param name="dirPath"></param>
        public void StartHandleDirectory(string dirPath)
        {
            m_logging.Log("start HandleDirectory to " + dirPath, MessageTypeEnum.INFO);
            this.m_dirWatcher.Created += new FileSystemEventHandler(handleNewFile);
            this.m_dirWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// handleNewFile function.
        /// handle with new file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleNewFile(object sender, FileSystemEventArgs e)
        {
            this.m_logging.Log("handle a new file in directory: " + e.FullPath, MessageTypeEnum.INFO);
            string fileExtention = Path.GetExtension(e.FullPath);
            //check if the new file extention is relevant.
            if (extensions.Any(fileExtention.Equals))
            {
                string[] args = { e.FullPath };
                CommandRecievedEventArgs commandREventArgs = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, args, "");
                this.OnCommandRecieved(this, commandREventArgs);
            }
        }


        /// <summary>
        /// EndHandle function.
        /// stop handle directory.
        /// </summary>
        public void EndHandle()
        {
            //disable watcher
            this.m_dirWatcher.EnableRaisingEvents = false;
            //raising DirectoryClose event
            DirectoryCloseEventArgs args = new DirectoryCloseEventArgs(this.m_path, "closing directory: " + this.m_path);
            DirectoryClose?.Invoke(this, args);
        }

        /// <summary>
        /// OnCommandRecieved function.
        /// handle with new command the service recieved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool result;
            // if a close command
            if(e.CommandID == (int)CommandEnum.CloseCommand) {
                
                this.EndHandle();
                return;
            }
            string massage = this.m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
            if (result)
            {
                this.m_logging.Log(massage, MessageTypeEnum.INFO);
            }
            else
            {
                this.m_logging.Log(massage, MessageTypeEnum.FAIL);
            }
        }
        public string GetPath()
        {
            return this.m_path;
        }
    }
}

