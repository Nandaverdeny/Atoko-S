using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AToko.DataContexts;
using ATokoEntities;
using AToko.Models;

namespace AToko.Controllers
{
    [Authorize(Roles = "admin")]
    public class KursController : Controller
    {
        private ATokoDb db = new ATokoDb();

        // GET: Kurs
       //[Authorize(Roles="admin")]
        public ActionResult Index()
        {
            return View(db.KursSG.ToList());
        }

        // GET: Kurs/Details/5
        //[Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kurs kurs = db.KursSG.Find(id);
            if (kurs == null)
            {
                return HttpNotFound();
            }
            return View(kurs);
        }

        // GET: Kurs/Create
        //[Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kurs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KursID,Currency,Rate")] Kurs kurs)
        {
            if (ModelState.IsValid)
            {
                db.KursSG.Add(kurs);
                db.SaveChanges();

                Logger.AddLog(
                     User.Identity.Name,
                     kurs.KursID,
                     Logger.Rate,
                     Logger.Add,
                     Logger.DescriptionRate(kurs.Currency, kurs.Rate)
                 );

                return RedirectToAction("Index");
            }

            return View(kurs);
        }

        // GET: Kurs/Edit/5
        //[Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kurs kurs = db.KursSG.Find(id);
            if (kurs == null)
            {
                return HttpNotFound();
            }
            return View(kurs);
        }

        // POST: Kurs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KursID,Currency,Rate")] Kurs kurs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kurs).State = EntityState.Modified;
                db.SaveChanges();

                Logger.AddLog(
                     User.Identity.Name,
                     kurs.KursID,
                     Logger.Rate,
                     Logger.Edit,
                     Logger.DescriptionRate(kurs.Currency, kurs.Rate)
                 );

                return RedirectToAction("Index");
            }
            return View(kurs);
        }

        // GET: Kurs/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kurs kurs = db.KursSG.Find(id);
            if (kurs == null)
            {
                return HttpNotFound();
            }
            return View(kurs);
        }

        // POST: Kurs/Delete/5
        //[Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kurs kurs = db.KursSG.Find(id);

            //if rate is been used on products then cannot delete
            if (db.Products.Where(o => o.KursID == kurs.KursID).Count() > 0)
            {
                ViewBag.Msg = "Can't delete, Rate is been used by Product";
                return View(kurs);
            }
            else if (db.Products.Where(o => o.KursID == kurs.KursID).Count() == 0)
            {
                db.KursSG.Remove(kurs);
                db.SaveChanges();

                Logger.AddLog(
                     User.Identity.Name,
                     kurs.KursID,
                     Logger.Rate,
                     Logger.Delete,
                     Logger.DescriptionRate(kurs.Currency, kurs.Rate)
                 );

                return RedirectToAction("Index");
            }
            //

            return View(kurs);
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
