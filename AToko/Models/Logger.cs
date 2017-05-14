using AToko.DataContexts;
using ATokoEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AToko.Models
{
    public class Logger
    {
        public static string Add = "Insert";
        public static string Edit = "Update";
        public static string Delete = "Delete";

        public static string ProductIn = "ProductIns";
        public static string ProductOut = "ProductOuts";
        public static string Sales = "Sales";
        public static string Product = "Products";
        public static string Supplier = "Suppliers";
        public static string Rate = "Kurs";

        private static Log _log;

        public static string DescriptionSupplier(string _supplierName)
        {
            return string.Format("Supplier Name : {0}", _supplierName);
        }

        public static string DescriptionProduct(string _productCode, string _productName, int _kursId, int _price)
        {
            return string.Format("Product Code : {0}, ProductName : {1}, KursId : {2}, Price : {3}", _productCode, _productName, _kursId.ToString(), _price.ToString());
        }

        public static string DescriptionRate(string _currencyName, int _rate)
        {
            return string.Format("Currency Name : {0}, Rate : {1}", _currencyName, _rate.ToString());
        }

        public static string DescriptionQty(string _productCode, string _productName, int _newqty, int _prevqty)
        {
            return string.Format("Product Code : {0}, ProductName : {1}, Current Qty : {2}, Prev Qty : {3}", _productCode, _productName, _newqty.ToString(), _prevqty.ToString());
        }

        public static string DescriptionQtyPrice(string _productCode, string _productName, int _newqty, int _prevqty, int _newprice, int _prevprice)
        {
            return string.Format("Product Code : {0}, ProductName : {1}, Current Qty : {2}, Prev Qty : {3}, Current Price : {4}, Prev Price : {5}", _productCode, _productName, _newqty.ToString(), _prevqty.ToString(), _newprice.ToString(), _prevprice.ToString());
        }

        public static void AddLog(string _UserName, int _TransactionID, string _TableName, string _Action, string _Description)
        {
            _log = new Log();

            _log.UserName = _UserName;
            _log.TransactionID = _TransactionID;
            _log.TableName = _TableName;
            _log.Action = _Action;
            _log.Description = _Description;
            _log.Date = DateTime.Now;

            _insertLog();
        }

        private static void _insertLog()
        {
            using (var context = new ATokoDb())
            {
                try
                {
                    context.AuditLogs.Add(_log);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }                
            }
        }

    }

    
}