using LogView.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogView.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetData()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            List<string> loglevel = new List<string>() {"Warning", "Error", "Info","Fatal","Debug" };
            List<string> logger = new List<string>() { "App1", "App2", "App3", "App4", "App5" };
            List<LogEntry> log = new List<LogEntry>();


            for (int i = 0; i < 10000; i++)
            {
                log.Add(new LogEntry { Id = i.ToString(),Level = loglevel.ElementAt(rand.Next(0,5)), Date = DateTime.Now.Subtract(new TimeSpan(0,5*i,0)).ToString(),Logger = logger.ElementAt(rand.Next(0,4)), Message=" Text text  text..." });
            }

            return Json(log, JsonRequestBehavior.AllowGet);
        }
    }
}
