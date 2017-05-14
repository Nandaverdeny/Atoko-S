using AToko.DataContexts;
using AToko.Models;
using ATokoEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AToko.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private ATokoDb db = new ATokoDb();
        // GET: Report

        [Authorize(Roles = "admin, sales")]
        public ActionResult ReportIn()
        {

            return View();
        }

        [Authorize(Roles = "admin, sales")]
        public ActionResult ReportIns(string date1 , string date2)
        {
            if (date1 != null && date2 != null)
            {
                //string query = "select  ProductIns.ProductInID , ProductIns.Date,ProductIns.ProductCode ,ProductIns.Notes,Products.ProductName, ProductIns.Qty  ,ProductIns.Price, ProductIns.Price*ProductIns.Qty as Total from ProductIns JOIN Products on ProductIns.ProductCode = Products.ProductCode where ProductIns.Date >= '" + date1 + "' AND  ProductIns.Date <='" + date2 + "'";
                string query = string.Format("EXEC [dbo].[sp_GetReportIn] @dateFrom = '{0}', @dateTo = '{1}'", date1, date2);
                IEnumerable<Report> reportin = db.Database.SqlQuery<Report>(query);
                ViewBag.reportin = reportin.ToList();
            }

            return PartialView();
        }

        [Authorize(Roles = "admin, sales")]
        public ActionResult ReportOut()
        {

            return View();
        }

        [Authorize(Roles = "admin, sales")]
        public ActionResult ReportOuts(string date1 , string date2)
        {
            if (date1 != null && date2 != null)
            {
                //string query = "select ProductOuts.ProductOutID,ProductOuts.Date, ProductOuts.ProductCode ,ProductOuts.Notes,Products.ProductName, Products.Price , ProductOuts.Qty ,Products.Price*ProductOuts.Qty as Total FROM ProductOuts JOIN Products on ProductOuts.ProductCode=Products.ProductCode where ProductOuts.Date >='" + date1 + "' AND ProductOuts.Date <= '" + date2 + "'";
                string query = string.Format("EXEC [dbo].[sp_GetReportOut] @dateFrom = '{0}', @dateTo = '{1}'", date1, date2);
                IEnumerable<Report> reportout = db.Database.SqlQuery<Report>(query);
                ViewBag.reportout = reportout.ToList();
            }

            return PartialView();
        }

        
        public ActionResult ReportStock()
        {
            var supplierName = User.Identity.Name;
            //var datetoday = System.DateTime.Today;
            //string query = "select ProductIns.ProductCode  , SUM (ProductIns.Qty) as ProductIn ,  SUM (ProductOuts.Qty) as ProductOut  ,SUM (ProductIns.Qty) -  SUM (ProductOuts.Qty) as Stock from ProductIns join ProductOuts on ProductIns.ProductCode=ProductIns.ProductCode where ProductIns.Date <= '"+datetoday+"' AND ProductOuts.Date <= '"+datetoday+"' group by ProductIns.ProductCode";

            

            string query = string.Empty;

            if (Date.IsMockDate == "1")
            {
                DateTime fromDate = new DateTime(2014,01,01);
                DateTime date = Date.getDate();

                if (User.IsInRole("admin") || User.IsInRole("sales"))
                {
                    query = string.Format("EXEC [dbo].[sp_GetReportStock] @dateFrom = '{0}', @dateTo = '{1}'", fromDate.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"));
                }
                else
                {
                    query = string.Format("EXEC [dbo].[sp_GetReportStock] @dateFrom = '{1}', @dateTo = '{2}', @supplierName = '{0}'", supplierName, fromDate.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"));
                } 
            }
            else
            {
                if (User.IsInRole("admin") || User.IsInRole("sales"))
                {
                    query = "EXEC [dbo].[sp_GetReportStock] @dateFrom = NULL, @dateTo = NULL";
                }
                else
                {
                    query = string.Format("EXEC [dbo].[sp_GetReportStock] @dateFrom = NULL, @dateTo = NULL, @supplierName = '{0}'", supplierName);
                }
            }

            IEnumerable<ReportStock> stocktoday = db.Database.SqlQuery<ReportStock>(query);
            ViewBag.reporttoday = stocktoday.ToList();

            return View();
        }

        public ActionResult ReportStocks(string date1, string date2)
        {
            var supplierName = User.Identity.Name;

            if (date1 != null && date2 != null)
            {
                //string query = "select ProductIns.ProductCode  , SUM (ProductIns.Qty) as ProductIn ,  SUM (ProductOuts.Qty) as ProductOut  ,SUM (ProductIns.Qty) -  SUM (ProductOuts.Qty) as Stock from ProductIns join ProductOuts on ProductIns.ProductCode=ProductIns.ProductCode where ProductIns.Date >= '"+date1+"' AND ProductIns.Date <= '"+date2+"' AND ProductOuts.Date >= '"+date1+"' AND ProductOuts.Date <= '"+date2+"' group by ProductIns.ProductCode ";
                string query = string.Empty;

                if (User.IsInRole("admin") || User.IsInRole("sales"))
                {
                    query = string.Format("EXEC [dbo].[sp_GetReportStock] @dateFrom = '{0}', @dateTo = '{1}'", date1, date2);
                }
                else
                {
                    query = string.Format("EXEC [dbo].[sp_GetReportStock] @dateFrom = '{0}', @dateTo = '{1}', @supplierName = '{2}'", date1, date2, supplierName);
                }

                IEnumerable<ReportStock> reportstock = db.Database.SqlQuery<ReportStock>(query);
                ViewBag.reportstock = reportstock.ToList();
            }

            return PartialView();
        }


        public ActionResult ReportSale()
        {
            var supplierName = User.Identity.Name;
            //var datetoday = System.DateTime.Today;
            //string query = "select ProductIns.ProductCode  , SUM (ProductIns.Qty) as ProductIn ,  SUM (ProductOuts.Qty) as ProductOut  ,SUM (ProductIns.Qty) -  SUM (ProductOuts.Qty) as Stock from ProductIns join ProductOuts on ProductIns.ProductCode=ProductIns.ProductCode where ProductIns.Date <= '"+datetoday+"' AND ProductOuts.Date <= '"+datetoday+"' group by ProductIns.ProductCode";

            string query = string.Empty;

            if (Date.IsMockDate == "1")
            {
                DateTime fromDate = new DateTime(2014, 01, 01);
                DateTime date = Date.getDate();

                if (User.IsInRole("admin") || User.IsInRole("sales"))
                {
                    query = string.Format("EXEC [dbo].[sp_GetReportSales] @dateFrom = '{0}', @dateTo = '{1}'", fromDate.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"));
                }
                else
                {
                    query = string.Format("EXEC [dbo].[sp_GetReportSales] @dateFrom = '{1}', @dateTo = '{2}', @supplierName = '{0}'", supplierName, fromDate.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"));
                }
            }
            else
            {
                if (User.IsInRole("admin") || User.IsInRole("sales"))
                {
                    query = "EXEC [dbo].[sp_GetReportSales] @dateFrom = NULL, @dateTo = NULL";
                }
                else
                {
                    query = string.Format("EXEC [dbo].[sp_GetReportSales] @dateFrom = NULL, @dateTo = NULL, @supplierName = '{0}'", supplierName);
                }
            }

            IEnumerable<ReportSales> salestoday = db.Database.SqlQuery<ReportSales>(query);
            ViewBag.reporttoday = salestoday.ToList();

            return View();
        }

        public ActionResult ReportSales(string date1, string date2)
        {
            var supplierName = User.Identity.Name;

            if (date1 != null && date2 != null)
            {
                //string query = "select ProductIns.ProductCode  , SUM (ProductIns.Qty) as ProductIn ,  SUM (ProductOuts.Qty) as ProductOut  ,SUM (ProductIns.Qty) -  SUM (ProductOuts.Qty) as Stock from ProductIns join ProductOuts on ProductIns.ProductCode=ProductIns.ProductCode where ProductIns.Date >= '"+date1+"' AND ProductIns.Date <= '"+date2+"' AND ProductOuts.Date >= '"+date1+"' AND ProductOuts.Date <= '"+date2+"' group by ProductIns.ProductCode ";
                string query = string.Empty;

                if (User.IsInRole("admin") || User.IsInRole("sales"))
                {
                    query = string.Format("EXEC [dbo].[sp_GetReportSales] @dateFrom = '{0}', @dateTo = '{1}'", date1, date2);
                }
                else
                {
                    query = string.Format("EXEC [dbo].[sp_GetReportSales] @dateFrom = '{0}', @dateTo = '{1}', @supplierName = '{2}'", date1, date2, supplierName);
                }

                IEnumerable<ReportSales> reportsales = db.Database.SqlQuery<ReportSales>(query);
                ViewBag.reportsales = reportsales.ToList();
            }

            return PartialView();
        }

        [Authorize(Roles = "admin, sales")]
        public ActionResult ReportSupplier()
        {
            var LogUser = User.Identity.Name.ToString();

            return View();
        }

        //[Authorize(Roles = "admin")]
        public ActionResult StockPrediction()
        {
            var supplierName = User.Identity.Name;

            var context = new ATokoDb();

            List<Product> list = new List<Product>();

            Supplier ret = context.Suppliers.Where(o => o.SupplierName == supplierName).FirstOrDefault();

            if (ret != null)
            {
                list = context.Products.Where(o => o.SupplierID == ret.SupplierID).ToList(); 
            }
            else if (User.IsInRole("admin") || User.IsInRole("sales"))
            {
                list = context.Products.ToList();
            }

            ViewBag.arrOrderQty = "[]";
            ViewBag.arrOrderDate = "[]";
            ViewBag.productName = "";
            ViewBag.Result = "";

            List<Product> listproduct = list;

            foreach (var item in listproduct)
            {
                item.ProductName = item.ProductCode + " - " + item.ProductName;
            }

            return View(new ViewModel { listProduct = listproduct });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StockPrediction(ViewModel obj)
        {

            var supplierName = User.Identity.Name;

            var context = new ATokoDb();

            if (!string.IsNullOrEmpty(obj.ProductCode))
            {
                
                //string productCode = "";

                ViewBag.productName = context.Products.Where(o => o.ProductCode == obj.ProductCode).FirstOrDefault().ProductName;

                string querySales = string.Format("EXEC [dbo].[sp_GetSalesByProductCodeAndSupplierName] @productCode = '{0}', @supplierName = '{1}'", obj.ProductCode, supplierName);
                string queryStock = string.Format("EXEC [dbo].[sp_GetStockByProductCodeAndSupplierName] @productCode = '{0}', @supplierName = '{1}'", obj.ProductCode, supplierName);
                string queryOrder = string.Format("EXEC [dbo].[sp_GetProductInByProductCodeAndSupplierName] @productCode = '{0}', @supplierName = '{1}'", obj.ProductCode, supplierName);

                IEnumerable<FuzzyViewModel> listSalesByMonth = db.Database.SqlQuery<FuzzyViewModel>(querySales);
                IEnumerable<FuzzyViewModel> listStocksByMonth = db.Database.SqlQuery<FuzzyViewModel>(queryStock);
                IEnumerable<FuzzyViewModel> listOrderByMonth = db.Database.SqlQuery<FuzzyViewModel>(queryOrder);


                if (listOrderByMonth.Count() > 0 && listSalesByMonth.Count() > 0 && listStocksByMonth.Count() > 0)
                {
                    if (listOrderByMonth.Count() != listSalesByMonth.Count())
                    {
                        int count = listStocksByMonth.Count();

                        List<FuzzyViewModel> newList = new List<FuzzyViewModel>();

                        bool isSales = false;

                        if (listOrderByMonth.Count() != count)
                        {
                            isSales = false;
                        }
                        else if (listSalesByMonth.Count() != count)
                        {
                            isSales = true;
                        }

                        if (isSales)
                        {
                            foreach (var item in listStocksByMonth)
                            {
                                if (listSalesByMonth.Where(o => o.Year == item.Year && o.Month == item.Month).Count() > 0)
                                {
                                    newList.Add(listSalesByMonth.Where(o => o.Year == item.Year && o.Month == item.Month).FirstOrDefault());
                                }
                                else
                                {
                                    newList.Add(new FuzzyViewModel { Month = item.Month, Year = item.Year, ProductCode = item.ProductCode, Qty = 0 });
                                }
                            }

                            listSalesByMonth = newList;
                        }
                        else
                        {
                            foreach (var item in listStocksByMonth)
                            {
                                if (listOrderByMonth.Where(o => o.Year == item.Year && o.Month == item.Month).Count() > 0)
                                {
                                    newList.Add(listOrderByMonth.Where(o => o.Year == item.Year && o.Month == item.Month).FirstOrDefault());
                                }
                                else
                                {
                                    newList.Add(new FuzzyViewModel { Month = item.Month, Year = item.Year, ProductCode = item.ProductCode, Qty = 0 });
                                }
                            }

                            listOrderByMonth = newList;
                        }

                    }

                    int maxSales = listSalesByMonth.Max(o => o.Qty);
                    int minSales = listSalesByMonth.Min(o => o.Qty);
                    var lastSalesObj = listSalesByMonth.OrderByDescending(o => o.Year).ThenByDescending(o => o.Month).FirstOrDefault();
                    int lastSales = lastSalesObj.Qty;

                    int maxStock = listStocksByMonth.Max(o => o.Qty);
                    int minStock = listStocksByMonth.Min(o => o.Qty);
                    var lastStockObj = listStocksByMonth.OrderByDescending(o => o.Year).ThenByDescending(o => o.Month).FirstOrDefault();
                    int lastStock = lastStockObj.Qty;

                    int maxOrder = listOrderByMonth.Max(o => o.Qty);
                    int minOrder = listOrderByMonth.Min(o => o.Qty);
                    var lastOrderObj = listOrderByMonth.OrderByDescending(o => o.Year).ThenByDescending(o => o.Month).FirstOrDefault();
                    int lastOrder = lastOrderObj.Qty;

                    var sales = new Fuzzy();
                    sales.MaxNumber = maxSales;
                    sales.MinNumber = minSales;
                    sales.Input = lastSales;
                    sales.CalculateLinearUp();
                    sales.CalculateLinearDown();

                    var stock = new Fuzzy();
                    stock.MaxNumber = maxStock;
                    stock.MinNumber = minStock;
                    stock.Input = lastStock;
                    stock.CalculateLinearUp();
                    stock.CalculateLinearDown();

                    var order = new Fuzzy();
                    order.MaxNumber = maxOrder;
                    order.MinNumber = minOrder;
                    order.Input = lastOrder;
                    order.CalculateLinearUp();
                    order.CalculateLinearDown();

                    var ruleSet = new Fuzzy();

                    var item1 = new Fuzzy();
                    item1.InputSales = sales.ResultLinearDown;
                    item1.InputStock = stock.ResultLinearUp;
                    ruleSet.Rule1 = item1.ImplicationFunction();

                    var item2 = new Fuzzy();
                    item2.InputSales = sales.ResultLinearDown;
                    item2.InputStock = stock.ResultLinearDown;
                    ruleSet.Rule2 = item2.ImplicationFunction();

                    var item3 = new Fuzzy();
                    item3.InputSales = sales.ResultLinearUp;
                    item3.InputStock = stock.ResultLinearUp;
                    ruleSet.Rule3 = item3.ImplicationFunction();

                    var item4 = new Fuzzy();
                    item4.InputSales = sales.ResultLinearUp;
                    item4.InputStock = stock.ResultLinearDown;
                    ruleSet.Rule4 = item4.ImplicationFunction();

                    ruleSet.RuleComposition();

                    if (ruleSet.DownResult < ruleSet.UpResult)
                    {
                        ruleSet.OrderStatus = 1; // Up
                    }
                    else if (ruleSet.DownResult > ruleSet.UpResult)
                    {
                        ruleSet.OrderStatus = 2; // Down
                    }
                    else
                    {
                        ruleSet.OrderStatus = 1;
                    }
                    //else if(ruleSet.UpResult == ruleSet.DownResult)
                    //{
                    //    ruleSet.OrderStatus = 3;
                    //}

                    var membershipFunction = new Fuzzy();

                    if (ruleSet.OrderStatus == 1) // Up
                    {
                        membershipFunction.OrderMax = (order.Range * ruleSet.UpResult) + order.MinNumber;
                        membershipFunction.OrderMin = (order.Range * ruleSet.DownResult) + order.MinNumber;
                        membershipFunction.OrderStatus = 1;
                    }
                    else if (ruleSet.OrderStatus == 2) // Down
                    {
                        membershipFunction.OrderMax = (order.Range * ruleSet.UpResult) + order.MaxNumber;
                        membershipFunction.OrderMin = (order.Range * ruleSet.DownResult) + order.MaxNumber;
                        membershipFunction.OrderStatus = 2;
                    }
                    //else if (ruleSet.OrderStatus == 3)
                    //{
                    //    membershipFunction.OrderMax = (order.Range * ruleSet.UpResult) + order.MaxNumber;
                    //    membershipFunction.OrderMin = (order.Range * ruleSet.DownResult) + order.MaxNumber;
                    //    membershipFunction.OrderStatus = 3;
                    //}

                    membershipFunction.MaxNumber = order.MaxNumber;
                    membershipFunction.MinNumber = order.MinNumber;

                    membershipFunction.UpResult = ruleSet.UpResult;
                    membershipFunction.DownResult = ruleSet.DownResult;

                    //ViewBag.Result = membershipFunction.Defuzzyfication();

                    var listSeries = new List<ReportFuzzyViewModel>();

                    var date = new DateTime(lastOrderObj.Year, lastOrderObj.Month, 1);
                    var nextMonth = date.AddMonths(1);

                    var listOrder = listOrderByMonth.OrderBy(o => o.Year).ThenBy(o => o.Month);
                    var listSales = listSalesByMonth.OrderBy(o => o.Year).ThenBy(o => o.Month).ToList();

                    foreach (var item in listOrder)
                    {
                        listSeries.Add(new ReportFuzzyViewModel
                        {
                            Date = new DateTime(item.Year, item.Month, 1).ToString("MMM yy"),
                            Quantity = item.Qty
                        });
                    }

                    listSeries.Add(new ReportFuzzyViewModel
                    {
                        Date = nextMonth.ToString("MMM yy"),
                        Quantity = 0
                    });

                    var listResult = new List<ReportFuzzyViewModel>();

                    listResult.Add(new ReportFuzzyViewModel
                    {
                        Date = nextMonth.ToString("MMM yy"),
                        Quantity = decimal.Round(membershipFunction.Defuzzyfication())
                    });


                    var arrSalesQty = "[";
                    var arrOrderQty = "[";
                    var arrOrderDate = "[";
                    var arrOrderResult = "[";

                    decimal lastSeries = 0;
                    int indexLast = 0;

                    for (int i = 0; i < listSeries.Count; i++)
                    {
                        var x = i + 1;
                        if (x >= listSeries.Count)
                        {
                            indexLast = i - 1;
                            lastSeries = listSeries[i - 1].Quantity;
                            arrSalesQty = arrSalesQty + "null";
                            arrOrderQty = arrOrderQty + "null";//string.Format("{0}", listSeries[i].Quantity);
                            arrOrderDate = arrOrderDate + string.Format("'{0}'", listSeries[i].Date);
                        }
                        else
                        {
                            arrSalesQty = arrSalesQty + string.Format("{0},", listSales[i].Qty);
                            arrOrderQty = arrOrderQty + string.Format("{0},", listSeries[i].Quantity);
                            arrOrderDate = arrOrderDate + string.Format("'{0}',", listSeries[i].Date);
                        }
                    }

                    for (int i = 0; i < listSeries.Count; i++)
                    {
                        var x = i + 1;
                        if (x >= listSeries.Count)
                        {
                            arrOrderResult = arrOrderResult + "{y:" + listResult[0].Quantity + ",marker: {enabled: true,states: {hover: {enabled: true}}}}";//string.Format("{0}", listResult[0].Quantity);
                        }
                        else if (i == indexLast)
                        {
                            arrOrderResult = arrOrderResult + "{y:" + lastSeries + ",marker: {enabled: false,states: {hover: {enabled: false}}}},";
                        }
                        else
                        {
                            arrOrderResult = arrOrderResult + "null,";
                        }

                    }


                    //if (arrOrderQty.Length > 2)
                    //{
                    //    arrOrderQty = arrOrderQty.Remove(arrOrderQty.Length - 1); 
                    //}

                    //if (arrOrderDate.Length > 2)
                    //{
                    //    arrOrderDate = arrOrderDate.Remove(arrOrderDate.Length - 1);
                    //}

                    arrSalesQty = arrSalesQty + "]";
                    arrOrderQty = arrOrderQty + "]";
                    arrOrderDate = arrOrderDate + "]";
                    arrOrderResult = arrOrderResult + "]";

                    ViewBag.arrSalesQty = arrSalesQty;
                    ViewBag.arrOrderQty = arrOrderQty;
                    ViewBag.arrOrderDate = arrOrderDate;
                    ViewBag.arrOrderResult = arrOrderResult;

                    ViewBag.Result = "According to the calculations, the number of products (" + ViewBag.productName + ") should be ordered on the next month (" + nextMonth.ToString("MMM yy") + ") is about (" + decimal.Round(membershipFunction.Defuzzyfication()) + ") pieces";

                }
                else
                {
                    ViewBag.ErrMsg = "Insufficient data, cannot calculating stock prediction";
                }

            }
            else
            {
                ViewBag.ErrMsg = "Please select a product";
            }

            List<Product> list = new List<Product>();

            Supplier ret = context.Suppliers.Where(o => o.SupplierName == supplierName).FirstOrDefault();
            if (ret != null)
            {
                list = context.Products.Where(o => o.SupplierID == ret.SupplierID).ToList();
            }
            else if (User.IsInRole("admin") || User.IsInRole("sales"))
            {
                list = context.Products.ToList();
            }

            List<Product> listproduct = list;

            foreach (var item in listproduct)
            {
                item.ProductName = item.ProductCode + " - " + item.ProductName;
            }

            obj.listProduct = listproduct;

            return View(obj);
        }
    }
}