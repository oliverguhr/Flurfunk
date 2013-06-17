using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using Flurfunk.Helper;
using Flurfunk.Data.Interface;
using Flurfunk.Data.Model;

namespace Wevision.Controllers
{
    public class FacebookLoginController : Controller
    {
        private IUserService userService;

        public FacebookLoginController(IUserService userService)
        {
            this.userService = userService;
        }


        private Uri CallBackUri { get { return new Uri(Url.Action("Callback", "FacebookLogin", null, "http")); } }

        // Callback after Twitter Login
        public ActionResult Callback()
        {
            var client = new FacebookClient(ConfigurationManager.AppSettings["facebookAppId"],
                                           ConfigurationManager.AppSettings["facebookAppSecret"]);



            AuthenticationResult result = client.VerifyAuthentication(HttpContext, CallBackUri);
             
            if (result.IsSuccessful)
            {                
                User user = userService.GetByProviderId(result.ProviderUserId);

                if (user == null) 
                {
                    user = userService.Create(result.ExtraData["name"], result.Provider, result.ProviderUserId);
                }

                FormsAuthentication.SetAuthCookie(user.Name, false);

                ControllerContext.HttpContext.Response.Cookies.Add(new HttpCookie("loggedIn", "true"));

                SessionHelper.CurrentUser = user;
            }      

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Login()
        {
            var client = new FacebookClient(ConfigurationManager.AppSettings["facebookAppId"],
                                           ConfigurationManager.AppSettings["facebookAppSecret"]);

            client.RequestAuthentication(HttpContext, CallBackUri);

            return new EmptyResult();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            
            HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["loggedIn"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }

            HttpCookie cookie2 = this.ControllerContext.HttpContext.Request.Cookies["ASP.NET_SessionId"];
            if (cookie2 != null)
            {
                cookie2.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie2);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}