using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        #endregion

        #region properties
        public string OutputFolder
        {
            get
            {
                return this.m_OutputFolder;
            }
            set
            {
                this.m_OutputFolder = value;
            }
        }
        public int ThumbnailSize
        {
            get
            {
                return this.m_thumbnailSize;
            }
            set
            {
                this.m_thumbnailSize = value;
            }
        }
        #endregion

        /// <summary>
        /// The Function Addes A file to the system
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <param name="result">Indication if the Addition Was Successful</param>
        /// <returns>operation message</returns>
        public string AddFile(string path, out bool result)
        {
            string year = String.Empty, month= String.Empty, newPath = String.Empty, thumbPath = String.Empty;
            DateTime createdTimeOfPath = new DateTime();
            //try to get the created time of the file.
            try
            {
                createdTimeOfPath = File.GetCreationTimeUtc(path);
                year = createdTimeOfPath.Year.ToString();
                month = createdTimeOfPath.Month.ToString();
                newPath = createDir(year, month);
                //Thread.Sleep(500);
                newPath = newPath + Path.GetFileName(path);
                int i = 1;
                string newPathWithNum = newPath;
                while (File.Exists(newPath))
                {
                    newPathWithNum = newPath + Path.GetFileNameWithoutExtension(newPath) + i + Path.GetExtension(newPath);
                    File.Move(newPath, newPathWithNum);
                };
                Thread.Sleep(10);
                File.Move(path, newPath);
                //thumbnails directories and thumbnail creation
                Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails");
                Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month);
                thumbPath = m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month + "\\";
                // if thumbnail not existed, create it
                if (!File.Exists(thumbPath + Path.GetFileName(path)))
                {
                    Image image = Image.FromFile(newPathWithNum);
                    Image thumbnail = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero);
                    {
                        thumbnail.Save(thumbPath + Path.GetFileName(path));
                    }
                }
            } catch(Exception exeption)
            {
                result = false;
                throw exeption;
            }
            string returnMsg;
            returnMsg = " File " + Path.GetFileName(path) + " moved to " + newPath;
            result = true;
            return returnMsg;
        }

        /// <summary>
        /// creating directory by year and month
        /// </summary>
        /// <param name="year">year</param>
        /// <param name="month">month</param>
        /// <returns>path for dir</returns>
        public string createDir(string year, string month)
        {
            try
            {
                //create the basic directory.
                DirectoryInfo outPutFold = Directory.CreateDirectory(m_OutputFolder);
                outPutFold.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                //create the "year" directory.
                Directory.CreateDirectory(m_OutputFolder + "\\" + year);
                //create the "month" directory.
                Directory.CreateDirectory(m_OutputFolder + "\\" + year + "\\" + month);
            } catch(Exception exeption)
            {
                throw exeption;
            }
            return m_OutputFolder + "\\" + year + "\\" + month + "\\";
        }
       
    }
}
