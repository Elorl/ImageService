using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        #region members
        private IImageServiceModal m_modal;
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="modal">image modal service</param>
        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }

        /// <summary>
        /// executing command
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="result">result of operation. ture in succes and false on failure</param>
        /// <returns>result message</returns>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                // The String Will Return the New Path if result = true, and will return the error message
                return m_modal.AddFile(args[0], out result);
            } catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }
    }
}
