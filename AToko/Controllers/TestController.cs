using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AToko.Models;
using AToko.DataContexts;
using ATokoEntities;

namespace AToko.Controllers
{
    public class TestController : ApiController
    {
        [ResponseType(typeof(ViewModel))]
        public IHttpActionResult PostTest(ViewModel model)
        {
            var kurs = new Kurs
            {
                Currency = model.Currency,
                Rate = model.Rate
            };
            var supplier = new Supplier
            {
                SupplierName = model.SupplierName
            };
            using (var context = new ATokoDb())
            {
                context.KursSG.Add(kurs);
                context.Suppliers.Add(supplier);
                context.SaveChanges();
            }

            
           return CreatedAtRoute("DefaultApi", new { id = kurs.KursID }, kurs);
        }

    }
}
