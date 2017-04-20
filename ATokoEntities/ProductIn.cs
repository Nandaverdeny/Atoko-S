using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATokoEntities
{
    public class ProductIn
    {
        public int ProductInID { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public int Qty { get; set; }
        public int Price { get; set; }
        public string Notes { get; set; }



        public virtual Product Product { get; set; }
    }
}
