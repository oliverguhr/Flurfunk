using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flurfunk.Helper
{
    public static class HtmlExtensions
    {
        public static string ProfilePicture(this UrlHelper helper, Flurfunk.Data.Model.User user)
        {
            return string.Format("http://graph.facebook.com/{0}/picture?type=normal", user.ProviderId);
        }
    }
}