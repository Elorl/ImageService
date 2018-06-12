using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    /// <summary>
    /// controller of the images gallery
    /// </summary>
    public class ImagesController : Controller
    {
        #region members
        public static ImagesModel imagesModel = new ImagesModel();
        #endregion
        // GET: Images
        public ActionResult Images()
        {
            imagesModel.LoadImages();
            return View(imagesModel);
        }

        /// <summary>
        /// view a single image
        /// </summary>
        /// <param name="imagePath">path of image to be viewed</param>
        /// <returns>view contains image</returns>
        public ActionResult Image(string imagePath)
        {
            return View(imagesModel.GetImage(imagePath));
        }
        /// <summary>
        /// delete an image by path
        /// </summary>
        /// <param name="imagePath">path of image to be deleted</param>
        /// <returns>view</returns>
        public ActionResult DeleteImage(string imagePath)
        {
            ViewBag.waitingForDeletion = imagePath;
            ViewBag.ToBeDletedName = Path.GetFileName(imagePath);
            return View(imagesModel);
        }

        /// <summary>
        /// return the view of image deletion confirmation.
        /// </summary>
        /// <param name="ImagePath">path of image to confirm deletion</param>
        /// <returns>view</returns>
        public ActionResult ConfirmDelete(string ImagePath)
        {
            //delete
            imagesModel.DeleteImage(ImagePath);
            //redirect to gallery
            return RedirectToAction("Images");
        }
    }
}