using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WoodStore.Domain.Helpers
{
   public class Security :IDisposable
    {
        public bool IsValidString(string text)
        {
            var reg= new Regex(@"^[0-9a-zA-Z ]+$");
            if (reg.IsMatch(text))
            {
               return true;
            }
            return false;
        }

       public bool IsPositiveNumber(string number)
       {
           Regex regex = new Regex(@"^\s*(\d{1,9}|\d+)(\.\d{1,4})?\s*$");
           Match match = regex.Match(number);
           if (match.Success)
           {
               return true;
           }
           return false;
       }

       public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
