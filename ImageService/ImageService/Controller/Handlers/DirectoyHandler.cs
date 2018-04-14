using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;

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

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed
        public DirectoyHandler(IImageController controller, ILoggingService logging, string path)
        {
            this.m_controller = controller;
            this.m_logging = logging;
            this.m_path = path;
            this.m_dirWatcher = new FileSystemWatcher(path);
        }
        public void StartHandleDirectory(string dirPath)
        {
            m_logging.Log("start HandleDirectory to " + dirPath, MessageTypeEnum.INFO);
            this.m_dirWatcher.Created += new FileSystemEventHandler(handleNewFile);
            this.m_dirWatcher.EnableRaisingEvents = true;
        }
        private void handleNewFile(object sender, FileSystemEventArgs e)
        {
            this.m_logging.Log("handle a new file in directory: " + e.FullPath, MessageTypeEnum.INFO);
            string fileExtention = Path.GetExtension(e.FullPath);
            if(extensions.Any(fileExtention.Equals))
            {
                string[] args = { e.FullPath };
                CommandRecievedEventArgs commandREventArgs = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, args, "");
                this.OnCommandRecieved(this, commandREventArgs);
            }
        }
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool result;
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
    }
}
