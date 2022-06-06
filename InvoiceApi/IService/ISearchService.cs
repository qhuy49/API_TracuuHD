using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InvoiceApi.IService
{
    public partial interface ISearchService
    {
        byte[] PrintInvoiceSBM(string sobaomat, string masothue, string folder, string type, bool inchuyendoi, out string fileNamePrint);
        //byte[] ExportXmlInvoie(string sobaomat, string masothue, string pathReport, ref string fileName, ref string key);
        Task<JObject> GetThongBaoPH(JObject model);
        //Task<JObject> GetThongTinNM(JObject model);
        //JObject GetInfoInvoice(JObject model);

        JObject GetInforMulti(JObject model);
        Task<JObject> GetThongTinNM(JObject model);
        byte[] ExportXmlInvoieVTC(string sobaomat, string masothue, string pathReport, ref string fileName, ref string key);

        //string ExportXmlInvoie78(string sobaomat, string masothue, string pathReport, ref string fileName, ref string key);

        byte[] ExportXmlInvoieMulty(string sobaomat, string masothue, string pathReport, ref string fileName, ref string key);
        //string ExportXmlInvoie781(string sobaomat, string masothue, ref string fileName);
    }
}