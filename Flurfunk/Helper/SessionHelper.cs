using Flurfunk.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flurfunk.Helper
{
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
                //if (HttpContext.Current.Session["CurrentUser"] == null)
                //    throw new UserNotLoggedInException();

                //wenn der user angemeldet ist aber die session kaputt ist
                //if (HttpContext.Current.Session["CurrentUser"] == null && HttpContext.Current.User.Identity.IsAuthenticated)
                //{
                //    HttpContext.Current.Session["CurrentUser"] = WevisionData.UserService.GetByUsername(HttpContext.Current.User.Identity.Name);
                //}

                return (User)HttpContext.Current.Session["CurrentUser"];
            }
        }
    }
}