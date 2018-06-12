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
using System.IO;
using static WebApplication3.Models.ConfigModel;

namespace WebApplication3.Models
{
    /// <summary>
    /// Model of ImageWeb page.
    /// </summary>
    /// 
    public class ImageWebModel
    {
        #region members
        Client client;
        private string serviceStatus;
        private bool isOn;
        private static string outputDir;
        public OutputPath outputPath = OutputPath.Instance;
        private static ConfigModel configModel;
        #endregion

        #region events
        public event Notifychanges NotifyWeb;
        #endregion

        #region properties
        [Required]
        [Display(Name = "Service Status")]
        public bool Status { get; set; }

        [Required]
        [Display(Name = "Number of photos")]
        public int NumPhotos { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Students")]
        public List<Student> Students { get; set; }
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        public ImageWebModel()
        {
            this.client = Client.Instance;
            client.Start();
            this.isOn = client.isOn;
            configModel = new ConfigModel();
            configModel.Notify += Notify;
            Students = GetStudentFromFile();
        }

        /// <summary>
        /// Notify function.
        /// </summary>
        public void Notify()
        {
            //check if the path is valid.
            if (configModel.OutputDir != null || configModel.OutputDir != "")
            {
                outputDir = configModel.OutputDir;
                Status = this.isOn;
                NumPhotos = outputPath.ImagesCounter;
                NotifyWeb?.Invoke();
            }
        }

        

        /// <summary>
        /// GetStudentFromFile function.
        /// </summary>
        public static List<Student> GetStudentFromFile()
        {
            //open a stream to a file.
            StreamReader studentsFile = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/students.txt"));
            string readline;
            string[] arr;
            List<Student> Students = new List<Student>();
            //loop that read every line inside the file.
            while ((readline = studentsFile.ReadLine()) != null)
            {
                arr = readline.Split(',');
                //add the student name and id to the list of students.
                Students.Add(new Student(arr[0], arr[1]));
            }
            studentsFile.Close();
            return Students;
        }

        /// <summary>
        /// Student.
        /// </summary>
        public class Student
        {
            #region members
            private string name, id;
            #endregion

            /// <summary>
            /// constructor.
            /// </summary>
            public Student(string name, string id)
            {
                this.name = name;
                this.id = id;
            }
            #region properties
            public string GetName
            {
                get { return this.name; }
            }
            public string GetID
            {
                get { return this.id; }
            }
            #endregion
        }
    }
}