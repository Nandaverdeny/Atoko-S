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
    [Authorize(Roles = "admin, sales")]
    public class SalesController : Controller
    {
        private ATokoDb db = new ATokoDb();

        // GET: Sales
        //[Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var sales = db.Sales.OrderByDescending(o => o.Date);

            foreach (var item in sales)
            {
                item.Product = db.Products.Where(o => o.ProductCode == item.ProductCode).FirstOrDefault();
            }

            return View(sales.ToList());
        }

        // GET: Sales/Details/5
        //[Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // GET: Sales/Create
        //[Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.ProductCode = new SelectList(db.Products, "ProductCode", "ProductCode");
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SaleID,Date,ProductCode,Qty,Notes")] Sale sale)
        {
            DateTime date = Date.getDate();

            sale.Date = date;
            if (ModelState.IsValid)
            {
                DateTime fromDate = new DateTime(2014, 01, 01);
                
                string query = string.Format("EXEC [dbo].[sp_GetReportStock] @dateFrom = '{0}', @dateTo = '{1}'", fromDate.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"));
                IEnumerable<ReportStock> list = db.Database.SqlQuery<ReportStock>(query);

                int stock = list.Where(o => o.ProductCode == sale.ProductCode).FirstOrDefault().Stock;

                //var obj = db.Products.Where(o => o.ProductCode == sale.ProductCode).FirstOrDefault();
                //sale. = obj.ProductID;

                //var oldobj = db.Sales.Find(sale.SaleID);
                var productName = db.Products.Where(o => o.ProductCode == sale.ProductCode).FirstOrDefault().ProductName;

                if (sale.Qty <= stock)
                {
                    db.Sales.Add(sale);
                    db.SaveChanges();

                    Logger.AddLog(
                         User.Identity.Name,
                         sale.SaleID,
                         Logger.Sales,
                         Logger.Add,
                         Logger.DescriptionQty(sale.ProductCode, productName, sale.Qty, sale.Qty)
                     );

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.msg = "Quantity can't be greater than stock (" + stock.ToString() + ")";
                }

            }

            ViewBag.ProductCode= new SelectList(db.Products, "ProductCode", "ProductCode", sale.ProductCode);
            return View(sale);
        }

        // GET: Sales/Edit/5
        //[Authorize(Roles = "admin")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }

            IEnumerable<Product> listProduct = db.Products;

            foreach (var item in listProduct)
            {
                item.ProductName = item.ProductCode + " - " + item.ProductName;
            }

            ViewBag.ProductCode = new SelectList(listProduct, "ProductCode", "ProductName", sale.ProductCode);
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SaleID,ProductCode,Qty,Notes")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                Sale obj = db.Sales.Find(sale.SaleID);

                int qtyBefore = obj.Qty;

                obj.Qty = sale.Qty;
                obj.ProductCode = sale.ProductCode;
                obj.Notes = sale.Notes;

                DateTime date = Date.getDate();
                DateTime fromDate = new DateTime(2014, 01, 01);

                string query = string.Format("EXEC [dbo].[sp_GetReportStock] @dateFrom = '{0}', @dateTo = '{1}'", fromDate.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"));
                IEnumerable<ReportStock> list = db.Database.SqlQuery<ReportStock>(query);

                int stock = list.Where(o => o.ProductCode == sale.ProductCode).FirstOrDefault().Stock + qtyBefore;

                var oldobj = db.Sales.Find(sale.SaleID);
                var productName = db.Products.Where(o => o.ProductCode == sale.ProductCode).FirstOrDefault().ProductName;

                if (sale.Qty <= stock)
                {
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();

                    Logger.AddLog(
                         User.Identity.Name,
                         sale.SaleID,
                         Logger.Sales,
                         Logger.Edit,
                         Logger.DescriptionQty(sale.ProductCode, productName, sale.Qty, qtyBefore)
                     );

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.msg = "Quantity can't be greater than stock ("+ stock.ToString() +")";
                }
            }

            IEnumerable<Product> listProduct = db.Products;

            foreach (var item in listProduct)
            {
                item.ProductName = item.ProductCode + " - " + item.ProductName;
            }

            ViewBag.ProductCode = new SelectList(listProduct, "ProductCode", "ProductName", sale.ProductCode);
            return View(sale);
        }

        // GET: Sales/Delete/5
        //[Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        //[Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sale sale = db.Sales.Find(id);
            var productName = db.Products.Where(o => o.ProductCode == sale.ProductCode).FirstOrDefault().ProductName;

            db.Sales.Remove(sale);
            db.SaveChanges();

            Logger.AddLog(
                 User.Identity.Name,
                 sale.SaleID,
                 Logger.Sales,
                 Logger.Delete,
                 Logger.DescriptionQty(sale.ProductCode, productName, sale.Qty, sale.Qty)
             );

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
