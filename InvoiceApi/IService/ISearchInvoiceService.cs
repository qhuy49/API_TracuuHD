using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceApi.IService
{
    public interface ISearchInvoiceService
    {
        byte[] PrintInvoiceFromSbm(string id, string folder, string type);
        byte[] PrintInvoiceFromSbm(string id, string mst, string folder, string type);
        //byte[] PrintInvoiceFromSbm(string id, string mst, string folder, string type, bool inchuyendoi);
        byte[] GetInvoiceXml(string soBaoMat, string maSoThue);
        //===============vacom====================
        JObject GetInfoInvoice(JObject model);
        JObject SearchInvoice(JObject data);
        byte[] ExportZipFileXml(string sobaomat, string masothue, string pathReport, ref string fileName, ref string key);

        byte[] PrintInvoiceFromSbmVC(string sobaomat, string masothue, string folder, string type, out string fileNamePrint);
        byte[] PrintInvoiceFromSbmVC(string sobaomat, string masothue, string folder, string type, bool inchuyendoi, out string fileNamePrint);
        byte[] PrintInvoiceFromSbmVC(string sobaomat, string masothue, string folder, string type, bool inchuyendoi, out string xml, out string fileName);

        JObject FromDateToDate(JObject data, string tu_ngay, string den_ngay);
        byte[] ExportXmlInvoieMulty(string sobaomat, string masothue, string pathReport, ref string fileName, ref string key);

        //===========tool bảo trì===========
        JObject Search_Tax(string mst);

        byte[] inHoadonACB(string id, string mst, string folder, string type, bool inchuyendoi);

    }
}