using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using WoodStore.Domain.Uow;

namespace WoodStore.Domain.Helpers
{
    
    public static class Extensions
    {
        public static DateTime GetCurrentTime(this DateTime date,string userId)
        {
            UnitOfWork uow = new UnitOfWork();
            using (uow)
            {
                //date = date.("d", new CultureInfo("en-GB"));
                var tz = uow.UserRepository.FindById(userId).Branch.Branch_TimeZone;

                DateTime utcTime = DateTime.UtcNow;
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(tz);
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);

                return localTime;
            }
           
        }
    }
}