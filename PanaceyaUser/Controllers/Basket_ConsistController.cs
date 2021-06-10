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
    public class Basket_ConsistController : Controller
    {
        private my_panaceyaEntities db = new my_panaceyaEntities();

        // GET: Basket_Consist
        public ActionResult Index()
        {
            int idUser = db.Users.Where(p => p.Email == User.Identity.Name).FirstOrDefault().ID_User;
            int idBasket = db.Baskets.Where(p => p.ID_User == idUser).FirstOrDefault().ID_Basket;
            var basket_Consist = db.Basket_Consist.Include(b => b.Baskets).Include(b => b.Medicines).Where(p => p.ID_Basket == idBasket);
            ViewBag.IssuePoints = new SelectList(db.Pharmacies, "ID_Pharm", "Address");
            ViewBag.PayMethod = new SelectList(db.Pay_Method, "ID_Pay", "Method");
            return View(basket_Consist.ToList());
        }
        public void CreateOrder(string pay, string point)
        {
            int idUser = db.Users.Where(p => p.Email == User.Identity.Name).FirstOrDefault().ID_User;
            int idBasket = db.Baskets.Where(p => p.ID_User == idUser).FirstOrDefault().ID_Basket;
            Orders order = new Orders { ID_Basket = idBasket, ID_Pay = Convert.ToInt32(pay), ID_Status = 1, ID_Pharm = Convert.ToInt32(point) };
            db.Orders.Add(order);
            db.SaveChanges();
            db.Basket_Consist.RemoveRange(db.Basket_Consist.Where(x => x.ID_Basket == idBasket));
            db.SaveChanges();

        }

        public ActionResult ChangeElement(string idMed, string amount)
        {
            int IDMed = Convert.ToInt32(idMed);
            int Amount = Convert.ToInt32(amount);
            decimal priceOne = db.Medicines.Where(p => p.ID_Medicine == IDMed).FirstOrDefault().Price;
            decimal newPrice = priceOne * Amount;
            int idUser = db.Users.Where(p => p.Email == User.Identity.Name).FirstOrDefault().ID_User;
            int idBasket = db.Baskets.Where(p => p.ID_User == idUser).FirstOrDefault().ID_Basket;
            var row = db.Basket_Consist.Where(p => p.ID_Basket == idBasket && p.ID_Medicines == IDMed).FirstOrDefault();
            row.Price = newPrice;
            row.Amount = Amount;
            db.SaveChanges();
            return Json(new { newPrice});
        }

        // GET: Basket_Consist/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Basket_Consist basket_Consist = db.Basket_Consist.Find(id);
            if (basket_Consist == null)
            {
                return HttpNotFound();
            }
            return View(basket_Consist);
        }

        // GET: Basket_Consist/Create
        public ActionResult Create()
        {
            ViewBag.ID_Basket = new SelectList(db.Baskets, "ID_Basket", "ID_Basket");
            ViewBag.ID_Medicines = new SelectList(db.Medicines, "ID_Medicine", "Name");
            return View();
        }

        // POST: Basket_Consist/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string idMedicines, string Amount, string Price)
        {
            int IDMedicines = Convert.ToInt32(idMedicines);
            int AcceptableAmount =0;
            bool Presence = db.Medicines.Where(p => p.ID_Medicine == IDMedicines).FirstOrDefault().Presence;
            if(Presence)
            {
                int amount = Convert.ToInt32(Amount);
                decimal price = Convert.ToDecimal(Price);
                decimal endPrice = amount * price;
                int id = db.Users.Where(p => p.Email == User.Identity.Name).FirstOrDefault().ID_User;
                int idBasket = db.Baskets.Where(p => p.ID_User == id).FirstOrDefault().ID_Basket;
                var item = db.Basket_Consist.Where(p => p.ID_Basket == idBasket && p.ID_Medicines == IDMedicines).FirstOrDefault();
                
                if (item != null)
                {
                    int MaxAmount = db.Medicines.Where(p => p.ID_Medicine == item.ID_Medicines).FirstOrDefault().Amount;
                    int LimitAmount = amount + item.Amount;
                    if(LimitAmount > MaxAmount)
                    {
                        AcceptableAmount = LimitAmount - MaxAmount;
                        AcceptableAmount = amount - AcceptableAmount;
                        return Json(new { Presence, AcceptableAmount});

                    }
                    item.Amount += amount;
                    item.Price += endPrice;
                    db.SaveChanges();
                    //var medicine = db.Medicines.Where(p => p.ID_Medicine == IDMedicines).FirstOrDefault();
                    //medicine.Amount -= amount;
                    //if (medicine.Amount == 0)
                    //{
                    //    medicine.Presence = false;
                    //}
                    //db.SaveChanges();

                }
                else
                {
                    Basket_Consist basket = new Basket_Consist { ID_Basket = idBasket, ID_Medicines = IDMedicines, Amount = amount, Price = endPrice };
                    db.Basket_Consist.Add(basket);
                    db.SaveChanges();
                    //var medicine = db.Medicines.Where(p => p.ID_Medicine == IDMedicines).FirstOrDefault();
                    //medicine.Amount -= amount;
                    //if (medicine.Amount == 0)
                    //{
                    //    medicine.Presence = false;
                    //}
                    //db.SaveChanges();
                }
            }
            return Json(new { Presence, AcceptableAmount});

        }


        // GET: Basket_Consist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Basket_Consist basket_Consist = db.Basket_Consist.Find(id);
            if (basket_Consist == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Basket = new SelectList(db.Baskets, "ID_Basket", "ID_Basket", basket_Consist.ID_Basket);
            ViewBag.ID_Medicines = new SelectList(db.Medicines, "ID_Medicine", "Name", basket_Consist.ID_Medicines);
            return View(basket_Consist);
        }

        // POST: Basket_Consist/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Consist,ID_Basket,ID_Medicines,Amount,Price")] Basket_Consist basket_Consist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(basket_Consist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Basket = new SelectList(db.Baskets, "ID_Basket", "ID_Basket", basket_Consist.ID_Basket);
            ViewBag.ID_Medicines = new SelectList(db.Medicines, "ID_Medicine", "Name", basket_Consist.ID_Medicines);
            return View(basket_Consist);
        }

        // GET: Basket_Consist/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Basket_Consist basket_Consist = db.Basket_Consist.Find(id);
        //    if (basket_Consist == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(basket_Consist);
        //}

        // POST: Basket_Consist/Delete/5
        [HttpPost]
        public void DeleteConfirmed(string ID)
        {
            int id = Convert.ToInt32(ID);
            Basket_Consist basket_Consist = db.Basket_Consist.Where(p => p.ID_Consist == id).FirstOrDefault();
            int IdMedicine = basket_Consist.ID_Medicines;
            int Amount = basket_Consist.Amount;
            db.Basket_Consist.Remove(basket_Consist);
            db.SaveChanges();
          
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
