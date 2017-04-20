using AToko.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AToko.Controllers
{
    public class ValidationController : Controller
    {
        private ATokoDb db = new ATokoDb();

        [HttpPost]
        public JsonResult checkCode(string code)
        {
            string productCode = db.Products.Where(o => o.ProductCode == code).Select(o => o.ProductCode).FirstOrDefault();

            return Json(productCode, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult checkCodeExist(string code)
        {
            string productCode = db.Products.Where(o => o.ProductCode == code).Select(o => o.ProductCode).FirstOrDefault();

            

            return Json(string.IsNullOrEmpty(productCode), JsonRequestBehavior.AllowGet);
        }
    }
}