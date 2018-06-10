using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Image
    {
        #region Properties
        /// <summary>
        /// representing all image info.
        /// </summary>

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public string Month { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public string Year { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbnailPath")]
        public string ThumbnailPath { get; set; }
        #endregion

        #region constructor
        public Image(string name, string month, string year, string imageRelativePath, string thumbnailRelativePath)
        {
            Name = name;
            Month = month;
            Year = year;
            ImagePath = imageRelativePath;
            ThumbnailPath = thumbnailRelativePath;
        }
        #endregion

    }
}