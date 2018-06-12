using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    /// <summary>
    /// Model of images gallery.
    /// </summary>
    /// 
    public class ImagesModel
    {
        #region properties
        [Display(Name = "Images")]
        public List<Image> Images { get; set; }
        //singleton of the images path
        public OutputPath ImagesDirPath = OutputPath.Instance;
        public int ImagesCount { get; set; } = 0;
        #endregion

        #region events
        public event EventHandler ImageCountChanged;
        #endregion

        #region constructor
        /// loading images
        public ImagesModel() { LoadImages(); }
        #endregion
        /// <summary>
        /// returns a requested image as image object.
        /// </summary>
        /// <param name="thumbnailPath">path of image's thumbnail</param>
        /// <returns>requested image</returns>
        public Image GetImage(string thumbnailPath)
        {
            foreach(Image image in Images)
            {
                if(image.ThumbnailPath.Equals(thumbnailPath))
                {
                    return image;
                }
            }
            return null;
        }
        /// <summary>
        /// deletes an image.
        /// </summary>
        /// <param name="toBeDeletedPath">path of thumbnail of the image to be deleted</param>
        public void DeleteImage(string toBeDeletedPath)
        {
            Image toBeDeleted = GetImage(toBeDeletedPath);
            if (toBeDeleted == null) { return;}
            //try actual files deletion
            try
            {
                File.Delete(toBeDeleted.ThumbnailAbsPath);
                File.Delete(toBeDeleted.ImageAbsPath);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return;
            }
            //remove from list, update counter and raise counter event 
            Images.Remove(toBeDeleted);
            --ImagesCount;
            ImageCountChanged?.Invoke(this, null);
        }
        /// <summary>
        /// load images list from images path.
        /// </summary>
        public void LoadImages()
        {
            string[] extensions = { ".jpg", ".png", ".gif", ".bmp" };
            if (!Directory.Exists(ImagesDirPath.Path)) { return; }
            //work on temp list for concurrent view cases (e.g: while updating another view is trying to view images)
            List<Image> LoadedImages = new List<Image>();
            string ThumbsDirPath =  ImagesDirPath.Path +  "\\Thumbnails";
            DirectoryInfo[] thumbsDirs = new DirectoryInfo(ThumbsDirPath).GetDirectories();
            foreach (DirectoryInfo yearDirectory in thumbsDirs)
            {
                DirectoryInfo[] yearDirs = yearDirectory.GetDirectories();

                foreach (DirectoryInfo monthDirectory in yearDirs)
                {
                    foreach (FileInfo thumb in monthDirectory.GetFiles())
                    {
                        //make sure it's a tumbnail file
                        if (!extensions.Contains(thumb.Extension.ToLower())) { continue; }

                        try
                        {
                            ///create the image object and add to list
                            string name = thumb.Name;
                            string thumbnailAbsPath = thumb.FullName;
                            string thumbnailRelativePath = @"~\" + Path.GetFileName(ImagesDirPath.Path) + thumb.FullName.Replace(ImagesDirPath.Path, "");
                            string imageAbsPath = thumbnailAbsPath.Replace("Thumbnails\\", "");
                            string imageRelativepath = thumbnailRelativePath.Replace("Thumbnails\\", "");
                            Image image = new Image(name, monthDirectory.Name, yearDirectory.Name, imageRelativepath, imageAbsPath,  thumbnailRelativePath, thumbnailAbsPath);
                            LoadedImages.Add(image);
                        }
                        catch (Exception){continue;}
                    }
                }
            }
            //update member(!) images list, count and invoke count event if needed
            Images = LoadedImages;
            int newCount = Images.Count();
            if(ImagesCount != newCount)
            {
                ImagesCount = newCount;
                ImageCountChanged?.Invoke(this, null);
            }   
        }
    }
}