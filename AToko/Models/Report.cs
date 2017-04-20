using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AToko.Models
{
    public class Report
    {
        public int ProductInID { get; set; }
        public int ProductOutID { get; set; }
        public DateTime Date { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public long Price { get; set; }
        public string Notes { get; set; }
        public long Total { get; set; }

        public int Stock { get; set; }
    }

    
}