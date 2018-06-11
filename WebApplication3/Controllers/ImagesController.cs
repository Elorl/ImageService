using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
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
        public ActionResult Image(string imagePath)
        {
            return View(imagesModel.GetImage(imagePath));
        }

        public ActionResult DeleteImage(string imagePath)
        {
            ViewBag.waitingForDeletion = imagePath;
            ViewBag.ToBeDletedName = Path.GetFileName(imagePath);
            return View(imagesModel);
        }

        public ActionResult ConfirmDelete(string ImagePath)
        {
            //delete
            imagesModel.DeleteImage(ImagePath);
            //redirect to gallery
            return RedirectToAction("Images");
        }
    }
}