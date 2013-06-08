using Flurfunk.Data.Interface;
using Flurfunk.Data.Model;
using Flurfunk.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace Flurfunk.Controllers
{
    public class MessageController : Controller
    {
        private IMessageService messageService;

        public MessageController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        public JsonResult Get(int count = 50, int startIndex = 0)
        {
            return Json(messageService.Get(count, startIndex));
        }

        public JsonResult Find(int count = 50, int startIndex = 0, string keyword = "")
        {
            return Json(messageService.Get(count, startIndex, keyword));
        }

        //// GET api/message/5
        //public string Get(int id)
        //{
        //    return "value";
        //}
        
        public void Create(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            { 
                messageService.Create(text, SessionHelper.CurrentUser._id);
            }
        }
    }
}
