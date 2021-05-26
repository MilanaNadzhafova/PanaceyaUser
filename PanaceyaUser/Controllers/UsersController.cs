using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PanaceyaUser.Models;

namespace PanaceyaUser.Views
{
    public class UsersController : Controller
    {
        private my_panaceyaEntities db = new my_panaceyaEntities();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public void Create(string UserName, string UserSurname, string UserPatronymic, string UserPhone, string UserEmail, string UserDateBirth, string UserPassword)
        {
            Users users = new Users();
            users.Name = UserName;
            users.Surname = UserSurname;
            users.Patronymic = UserPatronymic;
            users.Number_phone = UserPhone;
            users.Email = UserEmail;
            users.Birthday = Convert.ToDateTime(UserDateBirth);
            users.Password = UserPassword;
            users.Roled = "U";
            db.Users.Add(users);
            db.SaveChanges();
            int id = db.Users.Where(p => p.Email == users.Email).FirstOrDefault().ID_User;
            Baskets baskets = new Baskets { ID_User = id };
            db.Baskets.Add(baskets);
            db.SaveChanges();
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }
        [HttpPost]
        public ActionResult CheckRepeatLogin(string UserEmail)
        {
            var user = db.Users.FirstOrDefault(p => p.Email == UserEmail);
            if (user != null)
            {
                return Json(new { msg = true });
            }
            else
            {
                return Json(new { msg = false });
            }
        }

        // POST: Users/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_User,Email,Password,Surname,Name,Patronymic,Birthday,Number_phone,Roled")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
