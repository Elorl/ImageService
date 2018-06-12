using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
namespace WebApplication3.Controllers
{
    /// <summary>
    /// ImageWebController.
    /// </summary>
    /// 
    public class ImageWebController : Controller
    {
        #region members
        static ImageWebModel ImageWebModel = new ImageWebModel();
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        public ImageWebController()
        {
            //register to the event.
            ImageWebModel.NotifyWeb -= Notify;
            ImageWebModel.NotifyWeb += Notify;
        }

        /// <summary>
        /// Notify function.
        /// </summary>
        public void Notify()
        {
            ImageWeb();
        }

        /// <summary>
        /// ImageWeb function.
        /// </summary>
        public ActionResult ImageWeb()
        {
            //transfer the number of photos and server's status.
            ViewBag.NumPhotos = ImageWebModel.NumPhotos;
            ViewBag.Status = ImageWebModel.Status;
            return View(ImageWebModel);
        }
    }
}