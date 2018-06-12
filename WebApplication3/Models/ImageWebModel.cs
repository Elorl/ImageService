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
    public class ImageWebModel
    {
        Client client;
        private string serviceStatus;
        private bool isOn;
        private static string outputDir;
        private static ConfigModel configModel;
        public event Notifychanges NotifyWeb;

        public ImageWebModel()
        {
            this.client = Client.Instance;
            this.isOn = client.isOn;
            configModel = new ConfigModel();
            configModel.Notify += Notify;
            Students = GetStudentFromFile();
        }

        public void Notify()
        {
            if (configModel.OutputDir != null || configModel.OutputDir != "")
            {
                outputDir = configModel.OutputDir;
                NumPhotos = getNumPhotos();
                NotifyWeb?.Invoke();
            }
        }

        private static int getNumPhotos()
        {
            if (outputDir == null || outputDir == "")
            {
                return 0;
            } else
            {
                int numPhotos = 0;
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(outputDir);
                    foreach (DirectoryInfo directory in dir.GetDirectories())
                    {
                        if(directory.Name.Equals("Thumbnails"))
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
                } catch (Exception e)
                {
                    return 0;
                }
            }
        }

        public static List<Student> GetStudentFromFile()
        {
            StreamReader studentsFile = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/students.txt"));
            string readline;
            string[] arr;
            List<Student> Students = new List<Student>();
            while ((readline = studentsFile.ReadLine()) != null)
            {
                arr = readline.Split(',');
                Students.Add(new Student(arr[0], arr[1]));
            }
            studentsFile.Close();
            return Students;
        }
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

        public class Student
        {
            private string name, id;

            public Student(string name, string id)
            {
                this.name = name;
                this.id = id;
            }
            public string GetName
            {
                get { return this.name; }
            }
            public string GetID
            {
                get { return this.id; }
            }
        }
    }
}