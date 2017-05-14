using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATokoEntities
{
   public  class Kurs
    {
        public int KursID { get; set; }
        [Required]
        public string Currency  { get; set; }
        [Required]
        public int Rate { get; set; }
        public virtual ICollection<Product> Products { get; set; }


        
    }
}
