using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class ImageAndThumbnailPair
    {
        public ImageAndThumbnailPair(string imagePath, string thumbnailPath)
        {
            ImagePath = imagePath;
            ThumbnailPath = thumbnailPath;
        }

        public string ImagePath { set; get; }
        public string ThumbnailPath { set; get; }
    }
}