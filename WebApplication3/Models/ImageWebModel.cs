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
        private OutputPath outputPath = OutputPath.Instance;
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
                NumPhotos = getNumPhotos();
                NotifyWeb?.Invoke();
            }
        }

        /// <summary>
        /// getNumPhotos function.
        /// </summary>
        private static int getNumPhotos()
        {
            //check if the path is valid.
            if (outputDir == null || outputDir == "")
            {
                return 0;
            } else
            {
                int numPhotos = 0;
                try
                {
                    //get directory.
                    DirectoryInfo dir = new DirectoryInfo(outputDir);
                    //loop that runs all over the directories.
                    foreach (DirectoryInfo directory in dir.GetDirectories())
                    {
                        if (directory.Name.Equals("Thumbnails"))
                        {
                            continue;
                        }
                        //in case of upper case or small case letters.
                        numPhotos += directory.GetFiles("*jpg", SearchOption.AllDirectories).Length;
                        numPhotos += directory.GetFiles("*JPG", SearchOption.AllDirectories).Length;
                        numPhotos += directory.GetFiles("*gif", SearchOption.AllDirectories).Length;
                        numPhotos += directory.GetFiles("*GIF", SearchOption.AllDirectories).Length;
                        numPhotos += directory.GetFiles("*bmp", SearchOption.AllDirectories).Length;
                        numPhotos += directory.GetFiles("*BMP", SearchOption.AllDirectories).Length;
                        numPhotos += directory.GetFiles("*png", SearchOption.AllDirectories).Length;
                        numPhotos += directory.GetFiles("*PNG", SearchOption.AllDirectories).Length;
                    }
                    return numPhotos / 2;
                }
                catch (Exception e)
                {
                    return 0;
                }
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