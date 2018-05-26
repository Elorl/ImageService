using ImageService.Commands;
using ImageService.Modal;
using infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        #region members
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="modal"> image modal</param>
        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>()
            {
                {(int)CommandEnum.NewFileCommand, new NewFileCommand(this.m_modal)},
                {(int)CommandEnum.LogCommand, new LogCommand()}
            };
			// For Now will contain NEW_FILE_COMMAND
        }
        /// <summary>
        /// executing command from the dictionary by a given ID
        /// </summary>
        /// <param name="commandID">id</param>
        /// <param name="args">args</param>
        /// <param name="resultSuccesful">indication for operation success</param>
        /// <returns>result message</returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            ICommand command;
            if(commands.TryGetValue(commandID, out command))
            {
                Task<Tuple<string, bool>> task = new Task<Tuple<string, bool>>(() => {
                    bool result;
                    string msg = command.Execute(args, out result);
                    return Tuple.Create(msg, result); });
                task.Start();
                task.Wait();
                resultSuccesful = task.Result.Item2;
                return task.Result.Item1;
            }
            else
            {
                resultSuccesful = false;
                return "this is not a legal command";
            }
        }
    }
}

