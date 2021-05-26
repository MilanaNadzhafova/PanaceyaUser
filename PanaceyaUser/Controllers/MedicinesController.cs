using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PanaceyaUser.Models;

namespace PanaceyaUser.Controllers
{
    public class MedicinesController : Controller
    {
        private my_panaceyaEntities db = new my_panaceyaEntities();

        // GET: Medicines
        public ActionResult Index()
        {
                var medicines = db.Medicines.Include(m => m.Categories);
                ViewBag.ID_Category = new SelectList(db.Categories, "ID_Category", "Name");
                return View(medicines.ToList());
        }


      

        public ActionResult ElementesMedicines(string id)
        {
            int idCat = Convert.ToInt32(id);
            var medicines = db.Medicines.Include(m => m.Categories).Where(e => e.ID_Category == idCat);
            return PartialView(medicines.ToList());
        }



        // GET: Medicines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicines medicines = db.Medicines.Find(id);
            if (medicines == null)
            {
                return HttpNotFound();
            }
            return View(medicines);
        }

        // GET: Medicines/Create
        public ActionResult Create()
        {
            ViewBag.ID_Category = new SelectList(db.Categories, "ID_Category", "Name");
            return View();
        }

        // POST: Medicines/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Medicine,ID_Category,Name,Description,Price,Amount,Presence,Photo")] Medicines medicines)
        {
            if (ModelState.IsValid)
            {
                db.Medicines.Add(medicines);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Category = new SelectList(db.Categories, "ID_Category", "Name", medicines.ID_Category);
            return View(medicines);
        }

        // GET: Medicines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicines medicines = db.Medicines.Find(id);
            if (medicines == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Category = new SelectList(db.Categories, "ID_Category", "Name", medicines.ID_Category);
            return View(medicines);
        }

        // POST: Medicines/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Medicine,ID_Category,Name,Description,Price,Amount,Presence,Photo")] Medicines medicines)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicines).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Category = new SelectList(db.Categories, "ID_Category", "Name", medicines.ID_Category);
            return View(medicines);
        }

        // GET: Medicines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicines medicines = db.Medicines.Find(id);
            if (medicines == null)
            {
                return HttpNotFound();
            }
            return View(medicines);
        }

        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medicines medicines = db.Medicines.Find(id);
            db.Medicines.Remove(medicines);
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
