using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATokoEntities
{
   public  class Kurs
    {
        public int KursID { get; set; }
        public string Currency  { get; set; }
        public int Rate { get; set; }
        public virtual ICollection<Product> Products { get; set; }


        
    }
}
