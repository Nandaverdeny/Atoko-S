﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATokoEntities
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        [Required]
        public string SupplierName { get; set; }
        public string Notes { get; set; }


        public virtual ICollection<Product> Product { get; set; }
    }
}
