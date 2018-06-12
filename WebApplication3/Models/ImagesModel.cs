using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class ImagesModel
    {
        #region properties
        [Display(Name = "Images")]
        public List<Image> Images { get; set; }
        public OutputPath ImagesDirPath = OutputPath.Instance;
        public int ImagesCount { get; set; } = 0;
        #endregion

        #region events
        public event EventHandler ImageCountChanged;
        #endregion

        public ImagesModel() { LoadImages(); }

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

        public void DeleteImage(string toBeDeletedPath)
        {
            Image toBeDeleted = GetImage(toBeDeletedPath);
            if (toBeDeleted == null) { return;}
            try
            {
                File.Delete(toBeDeleted.ThumbnailAbsPath);
                File.Delete(toBeDeleted.ImageAbsPath);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return;
            }
            Images.Remove(toBeDeleted);
            --ImagesCount;
            ImageCountChanged?.Invoke(this, null);
        }

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
            //update member list, count and invoke count event if needed
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