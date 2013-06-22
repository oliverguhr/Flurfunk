using Flurfunk.Data.Interface;
using Flurfunk.Data.Model;
using Flurfunk.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Flurfunk.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private IMessageService messageService;

        public MessageController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        public JsonResult GetOlderThan(int count = 50, string time = "", string keyword = "")
        {
            DateTime param;
            if (string.IsNullOrWhiteSpace(time))
            { 
                param = DateTime.Now; 
            }
            else
            {
                param = time.FromJsonDateTime(); 
            }

            return Json(messageService.GetOlderThan(count, param,keyword));
        }

        public JsonResult GetNewerThan(string time = "", string keyword = "")
        {
            DateTime param;
            if (string.IsNullOrWhiteSpace(time))
            {
                //todo: add propper error handling
                return Json(new {});
            }
            else
            {
                param = time.FromJsonDateTime();
            }

            return Json(messageService.GetNewerThan(param, keyword));
        }     
        
        public void Create(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            { 
                messageService.Create(text, SessionHelper.CurrentUser._id);
            }
        }
    }
}
