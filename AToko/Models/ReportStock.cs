using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AToko.Models
{
    public class ReportStock
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int ProductIn { get; set; }
        public int ProductOut{ get; set; }
        public int Stock { get; set; }
    }
}