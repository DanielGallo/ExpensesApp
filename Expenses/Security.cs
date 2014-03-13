using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expenses
{
    public class Security
    {
        public static int? GetUserID()
        {
            int? UserID = null;
            HttpContext context = HttpContext.Current;

            if ((context == null) || (context.User == null) || (context.User.Identity == null) || (context.User.Identity.Name == "") )
                return null;
            else
            {
                UserID = int.Parse(context.User.Identity.Name);
            }
            
            return UserID;
        }
    }
}