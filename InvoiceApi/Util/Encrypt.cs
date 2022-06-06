using InvoiceApi.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace InvoiceApi.Util
{
    public static class conn
    {
        public static TracuuHDDTContext getdb()
        {
            var connectTraCuu = System.Configuration.ConfigurationManager.ConnectionStrings["TracuuHDDTConnectionString"];
            string conn = EncodeMinvocie.Class1.Decrypt(connectTraCuu.ToString(), "NAMPV18081202");
            ExceptionUtility.LogInfo_CreateInvoice("getdb1 " + conn);
            return new TracuuHDDTContext(conn);
        }

        public static void getdb(this TracuuHDDTContext  conn1)
        {
            var connectTraCuu = System.Configuration.ConfigurationManager.ConnectionStrings["TracuuHDDTConnectionString"];
            string conn = EncodeMinvocie.Class1.Decrypt(connectTraCuu.ToString(), "NAMPV18081202");
            ExceptionUtility.LogInfo_CreateInvoice("getdb2 " + conn);
            conn1 = new TracuuHDDTContext(conn);
        }

    }
}