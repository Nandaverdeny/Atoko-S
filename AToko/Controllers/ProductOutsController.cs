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
    public class ProductOutsController : Controller
    {
        private ATokoDb db = new ATokoDb();

        // GET: ProductOuts
        //[Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var productsOut = db.ProductsOut.OrderByDescending(o => o.Date);

            foreach (var item in productsOut)
            {
                item.Product = db.Products.Where(o => o.ProductCode == item.ProductCode).FirstOrDefault();
            }

            return View(productsOut.ToList());
        }

        // GET: ProductOuts/Details/5
        //[Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductOut productOut = db.ProductsOut.Find(id);
            if (productOut == null)
            {
                return HttpNotFound();
            }
            return View(productOut);
        }

        // GET: ProductOuts/Create
        //[Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.ProductCode = new SelectList(db.Products, "ProductCode", "ProductCode");
            return View();
        }

        // POST: ProductOuts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductOutID,Date,ProductCode,Qty,Notes")] ProductOut productOut)
        {
            DateTime date = Date.getDate();

            productOut.Date = date;
            if (ModelState.IsValid)
            {
                DateTime fromDate = new DateTime(2014, 01, 01);

                string query = string.Format("EXEC [dbo].[sp_GetReportStock] @dateFrom = '{0}', @dateTo = '{1}'", fromDate.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"));
                IEnumerable<ReportStock> list = db.Database.SqlQuery<ReportStock>(query);

                int stock = list.Where(o => o.ProductCode == productOut.ProductCode).FirstOrDefault().Stock;

                //var obj = db.Products.Where(o => o.ProductCode == sale.ProductCode).FirstOrDefault();
                //sale. = obj.ProductID;

                if (productOut.Qty <= stock)
                {
                    db.ProductsOut.Add(productOut);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.msg = "Quantity can't be greater than stock (" + stock.ToString() + ")";
                }
            }

            ViewBag.ProductCode = new SelectList(db.Products, "ProductCode", "ProductCode", productOut.ProductCode);
            return View(productOut);
        }

        // GET: ProductOuts/Edit/5
        //[Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductOut productOut = db.ProductsOut.Find(id);
            if (productOut == null)
            {
                return HttpNotFound();
            }

            IEnumerable<Product> listProduct = db.Products;

            foreach (var item in listProduct)
            {
                item.ProductName = item.ProductCode + " - " + item.ProductName;
            }

            ViewBag.ProductCode = new SelectList(listProduct, "ProductCode", "ProductName", productOut.ProductCode);
            return View(productOut);
        }

        // POST: ProductOuts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductOutID,ProductCode,Qty,Notes")] ProductOut productOut)
        {

            if (ModelState.IsValid)
            {
                ProductOut obj = db.ProductsOut.Find(productOut.ProductOutID);

                int qtyBefore = obj.Qty;

                obj.Qty = productOut.Qty;
                obj.ProductCode = productOut.ProductCode;
                obj.Notes = productOut.Notes;

                DateTime date = Date.getDate();
                DateTime fromDate = new DateTime(2014, 01, 01);

                string query = string.Format("EXEC [dbo].[sp_GetReportStock] @dateFrom = '{0}', @dateTo = '{1}'", fromDate.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"));
                IEnumerable<ReportStock> list = db.Database.SqlQuery<ReportStock>(query);

                int stock = list.Where(o => o.ProductCode == productOut.ProductCode).FirstOrDefault().Stock + qtyBefore;

                if (productOut.Qty <= stock)
                {
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.msg = "Quantity can't be greater than stock (" + stock.ToString() + ")";
                }

            }

            IEnumerable<Product> listProduct = db.Products;

            foreach (var item in listProduct)
            {
                item.ProductName = item.ProductCode + " - " + item.ProductName;
            }

            ViewBag.ProductCode = new SelectList(listProduct, "ProductCode", "ProductName", productOut.ProductCode);
            return View(productOut);
        }

        // GET: ProductOuts/Delete/5
        //[Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductOut productOut = db.ProductsOut.Find(id);
            if (productOut == null)
            {
                return HttpNotFound();
            }
            return View(productOut);
        }

        // POST: ProductOuts/Delete/5
        //[Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductOut productOut = db.ProductsOut.Find(id);
            db.ProductsOut.Remove(productOut);
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
