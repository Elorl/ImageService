using Connection;
using infrastructure;
using infrastructure.Enums;
using infrastructure.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

namespace WebApplication3.Models
{    
     /// <summary>
     /// Model of config page.
     /// </summary>
     /// 
    public class ConfigModel
    {
        #region events
        public delegate void Notifychanges();
        public event Notifychanges Notify;
        #endregion

        #region members
        Client client;
        #endregion

        #region properties
        [Required]
        [Display(Name = "Output Directory")]
        public string OutputDir { get; set; }

        [Required]
        [Display(Name = "OutputPath")]
        public OutputPath OutputPath { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Source Name")]
        public string SourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Name")]
        public string LogName { get; set; }

        [Required]
        [Display(Name = "Thumbnail Size")]
        public string ThumbnailSize { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Handlers")]
        public ObservableCollection<string> Handlers { get; set; }
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        public ConfigModel()
        {
            this.client = Client.Instance;
            this.client.CommandRecieved += ConfigRecieved;
            this.client.Start();
            this.OutputDir = String.Empty;
            this.OutputPath = OutputPath.Instance;
            this.SourceName = String.Empty;
            this.LogName = String.Empty;
            this.ThumbnailSize = String.Empty;
            this.Handlers = new ObservableCollection<string>();
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, null, "");
            client.SendCommand(command);
        }

        /// <summary>
        /// RemoveHandler function.
        /// </summary>
        /// <param name="path"> path</param>
        public void RemoveHandler(string path)
        {
            try
            {
                string[] selected = { path };
                //create command and send it to the service.
                CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, selected, "");
                this.client.SendCommand(command);
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// ConfigRecieved function.
        /// </summary>
        /// <param name="sender"> object sender</param>
        /// <param name="args"> CommandRecievedEventArgs args</param>
        private void ConfigRecieved(object sender, CommandRecievedEventArgs args)
        {
            //check which command arrived.
            if (args.CommandID == (int)CommandEnum.GetConfigCommand)
            {
                GetConfigHandle(args);
            }
            else if (args.CommandID == (int)CommandEnum.CloseCommand)
            {
                CloseCommandHandle(args);
            }
            Notify?.Invoke();
        }

        /// <summary>
        /// GetConfigHandle function.
        /// </summary>
        /// <param name="args"> CommandRecievedEventArgs args</param>
        private void GetConfigHandle(CommandRecievedEventArgs args)
        {
            this.OutputDir = args.Args[0];
            this.OutputPath.Path = args.Args[0];
            this.SourceName = args.Args[1];
            this.LogName = args.Args[2];
            this.ThumbnailSize = args.Args[3];
            string[] incomeDirs = args.Args[4].Split(';');
            //add all the folders to the list.
            foreach (string dir in incomeDirs)
            {
                this.Handlers.Add(dir);
            }
        }

        /// <summary>
        /// CloseCommandHandle function.
        /// </summary>
        /// <param name="args"> CommandRecievedEventArgs args</param>
        private void CloseCommandHandle(CommandRecievedEventArgs args)
        {
            if (this.Handlers.Contains(args.Args[0]))
            {
                this.Handlers.Remove(args.Args[0]);
                Notify?.Invoke();
            }
        }



    }
}