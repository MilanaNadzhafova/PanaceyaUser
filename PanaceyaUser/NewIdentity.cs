//using PanaceyaUser.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Principal;
//using System.Web;

//namespace PanaceyaUser
//{
    
//        public static class MyIdentity
//        {
//            private static my_panaceyaEntities db = new my_panaceyaEntities();

//            public static string FullName(this IIdentity identity)
//            {
//                string FullName = db.Users.Where(u => u.Email == identity.Name).Select(u => u.LName + " " + u.FName + " " + u.MName).FirstOrDefault();
//                return FullName;
//            }
//        }
    
//}