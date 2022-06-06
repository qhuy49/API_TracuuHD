using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceApi.Data
{
    public static class CommonConstants
    {
        public static string UserSession = "USER_SESSION";
        public static string KeyMaHoa = "NAMPV18081202";
        public static string select32= "SELECT mau_hd AS mau_hd, inv_invoiceSeries,inv_invoiceNumber, inv_invoiceIssuedDate, inv_TotalAmount, sobaomat,inv_InvoiceAuth_id FROM dbo.inv_InvoiceAuth WHERE inv_invoiceIssuedDate BETWEEN 'tu_ngay' AND 'den_ngay' AND trang_thai<>N'Chờ ký' AND trang_thai_hd NOT IN (13,15) AND ma_dt = @ma_dt ORDER BY inv_invoiceNumber ASC";
        public static string select78 = "declare @mau_hd nvarchar(19)='' SELECT @mau_hd AS mau_hd, khieu AS inv_invoiceSeries,shdon	AS inv_invoiceNumber, tdlap AS  inv_invoiceIssuedDate,tgtttbso as inv_TotalAmount, sbmat AS sobaomat, hoadon68_id AS inv_InvoiceAuth_id FROM dbo.hoadon68 WHERE tdlap BETWEEN 'tu_ngay' AND 'den_ngay' AND tthai<>N'Chờ ký' AND  mnmua = @ma_dt ORDER BY shdon ASC";
    }
}