using Flurfunk.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;

namespace Flurfunk.Helper
{
    //todo: refactor to service or remove session
    public static class SessionHelper
    {
        /// <summary>
        /// Current User 
        /// </summary>        
        public static User CurrentUser
        {
            set
            {
                HttpContext.Current.Session["CurrentUser"] = value;
            }
            get
            {
                //todo: remove (add filter for that)
                if (HttpContext.Current.Session["CurrentUser"] == null && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    HttpContext.Current.Response.Redirect("~/FacebookLogin/LogOff", true);               
                }

                return (User)HttpContext.Current.Session["CurrentUser"]; 
            }
        }
    }
}