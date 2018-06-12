using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
namespace WebApplication3.Controllers
{
    public class ImageWebController : Controller
    {
        static ImageWebModel ImageWebModel = new ImageWebModel();

        public ImageWebController()
        {
            ImageWebModel.NotifyWeb -= Notify;
            ImageWebModel.NotifyWeb += Notify;
        }

        public void Notify()
        {
            ImageWeb();
        }
        // GET: ImageWeb
        public ActionResult ImageWeb()
        {
            ViewBag.NumPhotos = ImageWebModel.NumPhotos;
            ViewBag.Status = ImageWebModel.Status;
            return View(ImageWebModel);
        }
    }
}