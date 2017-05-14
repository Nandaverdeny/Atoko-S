using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATokoEntities
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public int SupplierID { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public int Price { get; set; }
        public string Notes { get; set; }
        [Required]
        public int KursID { get; set; }




        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<ProductIn> ProductIn { get; set; }
        public virtual ICollection<ProductOut> ProductOut { get; set; }
        public virtual ICollection<Sale> Sale { get; set; }
        public virtual Kurs KursSg { get; set; }
    }
}
