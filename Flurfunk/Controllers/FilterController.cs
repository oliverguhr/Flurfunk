using Flurfunk.Data.Interface;
using Flurfunk.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Flurfunk.Controllers
{
    public class FilterController : Controller
    {
        private IUserService userService;

        public FilterController(IUserService userService)
        {
            this.userService = userService;
        }

        public void Add(string keyword)
        {
            //todo: use validation stuff
            if (!string.IsNullOrWhiteSpace(keyword) && keyword.Count() >2) 
            {                
                userService.AddFilter(keyword, SessionHelper.CurrentUser._id);
                //todo: this is why we should also go session less, updates are ugly
                SessionHelper.CurrentUser = userService.Get(SessionHelper.CurrentUser._id);
            }
        }
        
        public JsonResult Get()
        {
            var filter = userService.Get(SessionHelper.CurrentUser._id).Filter;
            if(filter != null)
                return Json(filter.OrderBy(x => x), JsonRequestBehavior.AllowGet);
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public void Remove(string keyword)
        {
            userService.RemoveFilter(keyword, SessionHelper.CurrentUser._id);
        }

    }
}
