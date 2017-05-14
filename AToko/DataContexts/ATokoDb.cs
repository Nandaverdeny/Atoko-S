using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ATokoEntities;

namespace AToko.DataContexts
{
    public class ATokoDb : DbContext
    {

        public ATokoDb()
        : base ("DefaultConnection")
    {}

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductIn> ProducsIn { get; set; }
        public DbSet<ProductOut> ProductsOut { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Kurs> KursSG { get; set; }
        
        public DbSet<Log> AuditLogs { get; set; }

        //public System.Data.Entity.DbSet<AToko.Models.AccountSupplier> AccountSuppliers { get; set; }

    }
}