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

namespace AToko.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {
        private ATokoDb db = new ATokoDb();

        // GET: Products
        //[Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Supplier).OrderByDescending(o => o.ProductCode);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        //[Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        //[Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            var selectrate = (from b in db.KursSG
                              select new { b.Rate, text = b.Currency + "-" + b.Rate });


            ViewBag.Kurs = new SelectList(selectrate.ToList(), "Rate", "text");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductCode,ProductName,SupplierID,Price,Notes")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.KursID = db.KursSG.Where(o => o.Currency == "SGD").Select(o => o.KursID).FirstOrDefault();
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var selectrate = (from b in db.KursSG
                              select new { b.Rate, text = b.Currency + "-" + b.Rate });

            ViewBag.Kurs = new SelectList(selectrate.ToList(), "Rate", "text");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", product.SupplierID);
            return View(product);
        }

        // GET: Products/Edit/5
        //[Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var selectrate = (from b in db.KursSG
                              select new { b.Rate, text = b.Currency + "-" + b.Rate });
            ViewBag.Kurs = new SelectList(selectrate.ToList(), "Rate", "text");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName");
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductCode,ProductName,SupplierID,Price,Notes")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.KursID = db.KursSG.Where(o => o.Currency == "SGD").Select(o => o.KursID).FirstOrDefault();
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var selectrate = (from b in db.KursSG
                              select new { b.Rate, text = b.Currency + "-" + b.Rate });
            ViewBag.Kurs = new SelectList(selectrate.ToList(), "Rate", "text");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", product.SupplierID);
            return View(product);
        }

        // GET: Products/Delete/5
        //[Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        //[Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
