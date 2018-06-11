using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class OutputPath
    {
        private static OutputPath instance;
        public string Path { set; get; } = "C:\\Users\\avn\\source\\repos\\ImageService\\WebApplication3\\OutputImages";

        private OutputPath() { }
        public static OutputPath Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OutputPath();
                }
                return instance;
            }
        }
    }
}