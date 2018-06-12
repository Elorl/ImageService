using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
using System.Threading;
namespace WebApplication3.Controllers
{
    /// <summary>
    /// ConfigController.
    /// </summary>
    /// 
    public class ConfigController : Controller
    {
        #region members
        private static ConfigModel ConfigModel = new ConfigModel();
        private static string m_selectedItem;
        #endregion

        /// <summary>
        /// constructor.
        /// </summary>
        public ConfigController()
        {
            ConfigModel.Notify -= Notify;
            ConfigModel.Notify += Notify;
        }

        /// <summary>
        /// Notify function.
        /// </summary>
        public void Notify()
        {
            Config();
        }

        /// <summary>
        /// RemoveHandler function.
        /// </summary>
        /// <param name="selectedItem"> string selectedItem</param>
        public ActionResult RemoveHandler(string selectedItem)
        {
            //get the selected handler.
            m_selectedItem = selectedItem;
            return RedirectToAction("toRemove");
        }

        /// <summary>
        /// Config function.
        /// </summary>
        public ActionResult Config()
        {
            return View(ConfigModel);
        }

        /// <summary>
        /// ToRemove.
        /// </summary>
        public ActionResult ToRemove()
        {
            return View(ConfigModel);
        }

        /// <summary>
        /// ConfirmRemove function.
        /// </summary>
        public ActionResult ConfirmRemove()
        {
            //remove the handler from the handlers list.
            ConfigModel.RemoveHandler(m_selectedItem);
            Thread.Sleep(200);
            return RedirectToAction("Config");
        }

        /// <summary>
        /// NoDelete function.
        /// </summary>
        public ActionResult NoDelete()
        {
            return RedirectToAction("Config");
        }
    }
}