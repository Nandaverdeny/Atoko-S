using AToko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AToko.Controllers
{
    public class HomeController : Controller

    {

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Index()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    if (!User.IsInRole("Admin"))
            //    {
            //        return RedirectToAction("ReportStock", "Report");
            //    }
            //}

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string LastLogin()
        {
            return Date.ShowMockDate == "1" ? Date.getDate().ToString("yyyy-MM-dd") : "";    
        }
    }
}