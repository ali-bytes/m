using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WoodStore.Domain.Helpers;
using WoodStore.Domain.Uow;
using WoodStore.UI.Helpers;

namespace WoodStore.UI.Controllers
{
    //[Authorize]
    public class BaseController : Controller
    {
        //protected UnitOfWork uow = new UnitOfWork();

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            //var culture = CultureInfo.InstalledUICulture.IetfLanguageTag;

            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            string routLang = (string)ControllerContext.RouteData.Values["lang"];


            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            else
            {
                lang = "en-GB";
            }
            if (!string.IsNullOrEmpty(routLang) && routLang != lang)
            {
                if (routLang =="en")
                {
                    lang = "en-GB";
                }
                else
                {
                    lang = routLang;
                }
               
            }
            ViewData.Add("lang", lang ?? "en-GB");

            new SiteLanguages().SetLanguage(lang ?? "en-GB");
            //new SiteLanguages().SetLanguage("ar");

            return base.BeginExecuteCore(callback, state);
        }

        protected string GetUserId()
        {
            return User.Identity.GetUserId();
        }
       
        protected DateTime GetCurrentDateTime()
        {
            return DateTime.Now.GetCurrentTime(User.Identity.GetUserId());
        }
        //protected int GetUserBranch(string userId)
        //{
        //    var id = userId;
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //         id = GetUserId();
        //    }
        //    var branchId = uow.UserRepository.FindById(id).BranchId;
        //    return branchId;
        //}
       
    }
}