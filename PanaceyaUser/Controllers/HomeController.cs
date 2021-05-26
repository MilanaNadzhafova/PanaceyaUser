using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PanaceyaUser.Models;

namespace PanaceyaUser.Controllers
{
    public class HomeController : Controller
    {
        private static my_panaceyaEntities db = new my_panaceyaEntities();
        //private static string GetMD5Hash(string text)
        //{
        //    using (var hashAlg = MD5.Create()) // Создаем экземпляр класса реализующего алгоритм MD5
        //    {
        //        byte[] hash = hashAlg.ComputeHash(Encoding.UTF8.GetBytes(text)); // Хешируем байты строки text
        //        var builder = new StringBuilder(hash.Length * 2); // Создаем экземпляр StringBuilder. Этот класс предназначен для эффективного конструирования строк
        //        for (int i = 0; i < hash.Length; i++)
        //        {
        //            builder.Append(hash[i].ToString("X2")); // Добавляем к строке очередной байт в виде строки в 16-й системе счисления
        //        }
        //        return builder.ToString(); // Возвращаем значение хеша
        //    }
        //}


        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckAuthenticated()
        {
            if (User.Identity.IsAuthenticated) return Json(new { check = true });
            else return Json(new { check = false });

        }

        [Authorize]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("AccessError");
            else
                return View();
        }

        [HttpPost]
        public ActionResult Login(string Login, string Password)
        {
            var result = db.Users.FirstOrDefault(u => u.Email == Login && u.Password == Password);
            if (result == null)
            {
                var login = db.Users.FirstOrDefault(u => u.Email == Login);
                bool LoginCheck = true, PasswordCheck = true;
                if (login != null)
                {
                    LoginCheck = true;
                    PasswordCheck = false;
                }
                else
                {
                    PasswordCheck = true;
                    LoginCheck = false;
                }
                return Json(new { LoginCheck, PasswordCheck, AllResult = false });
            }
            else
            {
                FormsAuthentication.SetAuthCookie(Login, false);
                //Session["Menu"] = null;
                return Json(new { LoginCheck = "", PasswordCheck = "", AllResult = true });
            }
        }
      

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}