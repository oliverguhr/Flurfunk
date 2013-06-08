using Flurfunk.Data.Interface;
using Flurfunk.Helper;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flurfunk.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SessionHelper.CurrentUser = new Data.Model.User() { _id = ObjectId.Empty, Name = "Heinzzz" };
               
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
    }
}
