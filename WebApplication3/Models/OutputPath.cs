using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    /// <summary>
    /// a SINGLETON contains images directory path. 
    /// Aimes to support updates\getting of images path, without illegal referencing of the config controller or model.
    /// </summary>
    public class OutputPath
    {
        private static OutputPath instance;
        public string Path { set; get; }

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