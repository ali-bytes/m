using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;

namespace WoodStore.UI.Helpers
{
    public class SiteLanguages
    {
        public List<string> Languages = new List<string>()
        {
            "en-GB","ar"
        };

        //public static bool IsLangAvailable(string lang)
        //{
        //    return Languages.Where(x => x.Equals(lang)).FirstOrDefault() != null ? true : false;
        //}

        public static string GetDefaultLanguage()
        {
            return "en-GB";
        }
        public void SetLanguage(string lang)
        {
            try
            {
                //if (!IsLangAvailable(lang))
                //    lang = "en";
                var cultureInfo = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
                HttpCookie langCookie = new HttpCookie("culture", lang);
                langCookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(langCookie);
            }
            catch 
            {

            }
        }
    }
}
