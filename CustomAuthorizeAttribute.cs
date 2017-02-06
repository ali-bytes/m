using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WoodStore.Domain.Uow;

namespace WoodStore.UI.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        //public bool UseSendedUrl { get; set; }
        public string Url { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }
            //string roleId = (string)HttpContext.Current.Session["UserRoleId"];
            string url = string.Empty;

            if (!string.IsNullOrEmpty(Url))
            {
                url = Url;
            }

            //if (UseSendedUrl)
            //{
            //    url = Url;
            //}
            //else
            //{
            //    url = HttpContext.Current.Request.RawUrl;
            //}
            if (url == "/")
            {
                return true;
            }
            //UnitOfWork uow = new UnitOfWork();
            //using (uow)
            //{
            //    var b = uow.RolePrivilegeRepository.Get(a => a.RoleId == roleId && a.Privilege.Url == url).Any();
            //    return b;
            //}

            //return false;
            return true;
        }
    }
}