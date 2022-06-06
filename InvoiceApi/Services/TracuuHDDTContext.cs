using InvoiceApi.Data.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InvoiceApi.Services
{
    public class TracuuHDDTContext : DbContext
    {

        public TracuuHDDTContext() { }

        public TracuuHDDTContext(string conn)
            : base(conn)
        {
           
        }

        //public TracuuHDDTContext()
        //    :base("TracuuHDDTConnectionString")
        //{
        //    var connectTraCuu = System.Configuration.ConfigurationManager.ConnectionStrings["TracuuHDDTConnectionString"];
        //    string conn = EncodeMinvocie.Class1.Decrypt(connectTraCuu.ToString(), "NAMPV18081202");
        //    this.Configuration.ProxyCreationEnabled = false;
        //}

        public DbSet<inv_admin> Inv_admin { get; set; }
        //public DbSet<inv_updateInfo> inv_updateInfos { get; set; }
        public DbSet<inv_user> inv_user { get; set; }
        public DbSet<inv_customer_banned> inv_customer_banneds { get; set; }

    }
    

}