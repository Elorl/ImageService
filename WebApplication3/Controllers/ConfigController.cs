using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
using System.Threading;
namespace WebApplication3.Controllers
{
    public class ConfigController : Controller
    {
        #region members
        private static ConfigModel ConfigModel = new ConfigModel();
        private static string m_selectedItem;
        #endregion
        public ConfigController()
        {
            ConfigModel.Notify -= Notify;
            ConfigModel.Notify += Notify;
        }
        public void Notify()
        {
            Config();
        }
        // GET: config/RemoveHandler/
        public ActionResult RemoveHandler(string selectedItem)
        {
            m_selectedItem = selectedItem;
            //ConfigModel.RemoveHandler(m_selectedItem);
            //Thread.Sleep(500);
            return RedirectToAction("toRemove");
        }

        // GET: Config
        public ActionResult Config()
        {
            return View(ConfigModel);
        }
    
        // GET: ToRemove
        public ActionResult ToRemove()
        {
            return View(ConfigModel);
        }

        // GET: Config/ToRemove/
        public ActionResult ConfirmRemove()
        {
            ConfigModel.RemoveHandler(m_selectedItem);
            Thread.Sleep(200);
            return RedirectToAction("Config");
        }
        // GET: Config/NoDelete/
        public ActionResult NoDelete()
        {
            return RedirectToAction("Config");
        }
    }
}