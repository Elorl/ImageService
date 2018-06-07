using infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class LogsController : Controller
    {
        #region members
        private static LogsModel logModel = new LogsModel();
        #endregion
        public LogsController()
        {
            logModel.LogRecievedEvent -= LogController_LogRecieved;
            logModel.LogRecievedEvent += LogController_LogRecieved;
           
        }

        public void LogController_LogRecieved(object sender, EventArgs args)
        {
            Logs();
        }
        // GET: Logs
        public ActionResult Logs()
        {
            return View(logModel);
        }
    }
}