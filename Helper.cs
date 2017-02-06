using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using WoodStore.Domain.Models;
using WoodStore.Domain.Uow;

namespace WoodStore.UI.Helpers
{
    public class Helper : IDisposable
    {
        static UnitOfWork uow = new UnitOfWork();

        public static void Notify(Controller controller, bool success, string message)
        {
            controller.TempData["Msg"] = message;
            if (success)
            {
                controller.TempData["MsgClass"] = "success";
            }
            else
            {
                controller.TempData["MsgClass"] = "error";
            }
        }

        public static List<fn_GetMenu_Result> Menu()
        {
            List<fn_GetMenu_Result> menu =new List<fn_GetMenu_Result>();
            if (HttpContext.Current.Session["MenuList"] != null)
            {
                menu = (List<fn_GetMenu_Result>)HttpContext.Current.Session["MenuList"];
                if (menu.Count > 0)
                {
                    return menu;
                }
                
            }
            
            string roleId = (string)HttpContext.Current.Session["UserRoleId"];
            if (string.IsNullOrEmpty(roleId))
            {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                roleId = uow.UserRepository.Get(a => a.Id == userId).FirstOrDefault().Roles.FirstOrDefault().Id;
            }
            StoresDbContext db = new StoresDbContext();
            var list = db.fn_GetMenu(roleId).OrderBy(a => a.PrivOrder);
            HttpContext.Current.Session["UserRoles"] = roleId;
            menu = list.ToList();
            HttpContext.Current.Session["MenuList"] = menu;

            return menu;
        }
        public static string GetShortDate(DateTime dateTime)
        {
            return dateTime.ToShortDateString();
        }
        public static string[] GetColors()
        {
            string[] colors = new string[21];
            colors[0] = "#678900";
            colors[1] = "#06d6b7";
            colors[2] = "#ff8d00";
            colors[3] = "#f70a61";
            colors[4] = "#bd0af7";
            colors[5] = "#0a69f7";
            colors[6] = "#0ad6f7";
            colors[7] = "#0af78a";
            colors[8] = "#f3f70a";
            colors[9] = "#f70a0a";
            colors[10] = "#d3f70a";

            colors[11] = "#678900";
            colors[12] = "#06d6b7";
            colors[13] = "#f70a61";
            colors[14] = "#bd0af7";
            colors[15] = "#0a69f7";
            colors[16] = "#0ad6f7";
            colors[17] = "#0af78a";
            colors[18] = "#f3f70a";
            colors[19] = "#f70a0a";
            colors[20] = "#d3f70a";
            return colors;
        }
        public static TTo Mapper<TFrom, TTo>(TFrom from)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TFrom, TTo>();
            });
            IMapper mapper = config.CreateMapper();

            TTo dest = mapper.Map<TFrom, TTo>(from);
            return dest;
        }
        public static List<TTo> Mapper<TFrom, TTo>(List<TFrom> from)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TFrom, TTo>();
            });
            IMapper mapper = config.CreateMapper();

            List<TTo> dest = mapper.Map<List<TFrom>, List<TTo>>(from);
            return dest;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

}