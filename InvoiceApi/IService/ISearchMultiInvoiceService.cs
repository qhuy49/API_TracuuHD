using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;

namespace InvoiceApi.IService
{
    public partial interface ISearchMultiInvoiceService
    {
        JObject SearchInvoieCus(JObject model);
        JObject PrintInvoiceCus(JObject model);
        byte[] PrintInvoiceFromSbm(string sobaomat, string masothue, string folder, string type, bool inchuyendoi, out string xml, out string fileNamePrint);
        //byte[] PrintInvoice(string sobaomat, string masothue, string folder, string type, bool inchuyendoi, out string xml, out string fileNamePrint);
    }
}