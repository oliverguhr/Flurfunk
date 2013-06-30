using Flurfunk.Data.Interface;
using Flurfunk.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;

namespace Flurfunk.Controllers
{
    public class GroupController : Controller
    {
        private IGroupService groupService;
        private IUserService userService;

        public GroupController(IGroupService groupService, IUserService userService)
        {
            this.groupService = groupService;
            this.userService = userService;
        }

        public JsonResult Find(string keyword)
        {
            var filter = groupService.Find(keyword).Take(10).Select(x => new { id = x._id.ToString(), name = x.Name });
            return Json(filter);
        }

        public void Create(string name)
        {
            var group = groupService.Find(name).SingleOrDefault(x => x.Name.ToLower() == name.ToLower());
            //if a group with the same name exists..
            if (group != null)
            {
                //join that group
                Join(group._id.ToString());                
            }
            else
            {
                //if no group exists, create a new one
                var newgroup = groupService.Create(name);
                Join(newgroup._id.ToString());
            }
        }

        public void Leave(string groupId)
        {
            groupService.Leave(groupId, SessionHelper.CurrentUser._id.ToString());
            SessionHelper.CurrentUser = userService.Get(SessionHelper.CurrentUser._id.ToString());
        }

        public void Join(string groupId)
        {
            groupService.Join(groupId, SessionHelper.CurrentUser._id.ToString());
            SessionHelper.CurrentUser = userService.Get(SessionHelper.CurrentUser._id.ToString());
        }

        public JsonResult GetFromUser(string userId)
        {
            if(SessionHelper.CurrentUser.Groups != null)
                return Json(SessionHelper.CurrentUser.Groups.Select(x => new { id = x.Key, name = x.Value }), JsonRequestBehavior.AllowGet);

            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}
