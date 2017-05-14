using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATokoEntities
{
    [Table("AuditLog")]
    public class Log
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public int TransactionID { get; set; }
        public string TableName { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }

    }
}
