using ATokoEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AToko.Models
{
    public class ViewModel
    {
        public string SupplierName { get; set; }
        public string Currency { get; set; }
        public int Rate { get; set; }
        public string ProductCode { get; set; }
        public List<Product> listProduct { get; set; }

    }

}