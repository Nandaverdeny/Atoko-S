using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATokoEntities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int SupplierID { get; set; }
        public int Price { get; set; }
        public string Notes { get; set; }
        public int KursID { get; set; }




        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<ProductIn> ProductIn { get; set; }
        public virtual ICollection<ProductOut> ProductOut { get; set; }
        public virtual ICollection<Sale> Sale { get; set; }
        public virtual Kurs KursSg { get; set; }
    }
}
