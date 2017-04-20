using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AToko.Models
{
    public class FuzzyViewModel
    {
        public string ProductCode { get; set; }
        public int Qty { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

    }


    public class ReportFuzzyViewModel
    {
        public string Date { get; set; }
        public decimal Quantity { get; set; }

    }
}