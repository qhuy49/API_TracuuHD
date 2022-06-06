
using InvoiceApi.IService;
using InvoiceApi.Services;
using InvoiceApi.Util;
using MinvoiceLib.IServices;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace InvoiceApi.Controllers
{
    public class SearchController : BaseApiController
    {
        private readonly ISearchService _searchService; 
        public SearchController(ISearchService searchService)
        {
            this._searchService = searchService;
        }
        [HttpPost]
        [Route("Search/SearchInvoice")]
        [AllowAnonymous]

        public HttpResponseMessage PrintInvoice(JObject model)
        {

            HttpResponseMessage result;
            try
            {
                ExceptionUtility.LogInfo_CreateInvoice("1");
                string type = model["type"].ToString();
                string sobaomat = model["sobaomat"].ToString();
                string masothue = model["masothue"].ToString().Replace("-","");
                bool inchuyendoi = model.ContainsKey("inchuyendoi");
                //TracuuHDDTContext tracuu = new TracuuHDDTContext();
                //tracuu.getdb();

                TracuuHDDTContext tracuu = conn.getdb();
                ExceptionUtility.LogInfo_CreateInvoice("tracuu " + tracuu);



                var checkTraCuu = tracuu.inv_customer_banneds.FirstOrDefault(x =>
                    x.mst.Replace("-", "").Equals(masothue.Replace("-", "")) && x.type.Equals("KHOATRACUU") && x.is_unblock == false);
                if (checkTraCuu != null && !string.IsNullOrEmpty(checkTraCuu.mst))
                {
                    throw new Exception("Quý khách đang bị khóa tra cứu. Vui lòng liên hệ admin để giải quyết");
                }
                if (sobaomat.Length > 16)
                {
                    throw new Exception("Chuỗi số bảo mật vượt quá quy định ");
                }

                var withoutSpecial = new string(sobaomat.Where(c => Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c)).ToArray());
                if (sobaomat != withoutSpecial)
                {
                    throw new Exception("Chuỗi số bảo mật không đúng định dạng ");
                }

                string originalString = ActionContext.Request.RequestUri.OriginalString;
                string path = originalString.StartsWith("/api") ? "~/api/Content/report/" : "~/Content/report/";
                var folder = System.Web.HttpContext.Current.Server.MapPath(path);
                string fileName;
                ExceptionUtility.LogInfo_CreateInvoice("2 ");


                byte[] bytes = _searchService.PrintInvoiceSBM(sobaomat, masothue, folder, type, inchuyendoi, out fileName);

                result = new HttpResponseMessage(HttpStatusCode.OK);

                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                result.Content.Headers.ContentDisposition.FileName = "invoice.pdf";
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentLength = bytes.Length;
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                result.Content = new StringContent(ex.Message);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                result.Content.Headers.ContentLength = ex.Message.Length;
            }
            return result;
        }


        [HttpPost]
        [Route("Search/ExportXmlInvoice")]
        [AllowAnonymous]
        public HttpResponseMessage ExportZipFileXml(JObject model)
        {
            HttpResponseMessage result;
            try
            {
                string masothue = model["masothue"].ToString().Replace("-", "");
                string sobaomat = model["sobaomat"].ToString();
                string originalString = ActionContext.Request.RequestUri.OriginalString;
                string path = originalString.StartsWith("/api") ? "~/api/Content/report/" : "~/Content/report/";
                TracuuHDDTContext tracuu = conn.getdb();
                var checkTraCuu = tracuu.inv_customer_banneds.FirstOrDefault(x =>
                   x.mst.Replace("-", "").Equals(masothue.Replace("-", "")) && x.type.Equals("KHOATRACUU") && x.is_unblock == false);
                if (checkTraCuu != null && !string.IsNullOrEmpty(checkTraCuu.mst))
                {
                    throw new Exception("Quý khách đang bị khóa tra cứu. Vui lòng liên hệ admin để giải quyết");
                }
                if (sobaomat.Length > 16)
                {
                    throw new Exception("Chuỗi số bảo mật vượt quá quy định ");
                }
                var withoutSpecial = new string(sobaomat.Where(c => Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c)).ToArray());
                if (sobaomat != withoutSpecial)
                {
                    throw new Exception("Chuỗi số bảo mật không đúng định dạng ");
                }
                var folder = System.Web.HttpContext.Current.Server.MapPath(path);
                string fileName = "";
                string key = "";

                
                byte[] bytes = _searchService.ExportXmlInvoieMulty(sobaomat, masothue, folder, ref fileName, ref key);

                result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) };
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attach");
                result.Content.Headers.ContentDisposition.FileName = fileName + ".zip";
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                result.Content.Headers.ContentLength = bytes.Length;
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(ex.Message) };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                result.Content.Headers.ContentLength = ex.Message.Length;
            }
            return result;
        }

        [HttpGet]
        [Route("Search/ExportZipFileXmlVTC")]
        [AllowAnonymous]
        public HttpResponseMessage ExportZipFileXmlVTC(string masothue, string sobaomat)
        {
            HttpResponseMessage result;
            try
            {
                string originalString = ActionContext.Request.RequestUri.OriginalString;
                string path = originalString.StartsWith("/api") ? "~/api/Content/report/" : "~/Content/report/";
                TracuuHDDTContext tracuu = conn.getdb();
                var checkTraCuu = tracuu.inv_customer_banneds.FirstOrDefault(x =>
                   x.mst.Replace("-", "").Equals(masothue.Replace("-", "")) && x.type.Equals("KHOATRACUU") && x.is_unblock == false);
                if (checkTraCuu != null && !string.IsNullOrEmpty(checkTraCuu.mst))
                {
                    throw new Exception("Quý khách đang bị khóa tra cứu. Vui lòng liên hệ admin để giải quyết");
                }
                if (sobaomat.Length > 16)
                {
                    throw new Exception("Chuỗi số bảo mật vượt quá quy định ");
                }
                var withoutSpecial = new string(sobaomat.Where(c => Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c)).ToArray());
                if (sobaomat != withoutSpecial)
                {
                    throw new Exception("Chuỗi số bảo mật không đúng định dạng ");
                }
                var folder = System.Web.HttpContext.Current.Server.MapPath(path);
                string fileName = "";
                string key = "";


                byte[] bytes = _searchService.ExportXmlInvoieVTC(sobaomat, masothue, folder, ref fileName, ref key);

                result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) };
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attach");
                result.Content.Headers.ContentDisposition.FileName = fileName + ".zip";
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                result.Content.Headers.ContentLength = bytes.Length;
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(ex.Message) };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                result.Content.Headers.ContentLength = ex.Message.Length;
            }
            return result;
        }


        [HttpPost]
        [Route("Search/SearchTBH")]
        [AllowAnonymous]
        public async Task<JObject> GetThongBaoPH(JObject model)
        {
            JObject json = await _searchService.GetThongBaoPH(model);
            return json;
        }

        [HttpPost]
        [Route("invoice/GetInforInvoice")]
        [AllowAnonymous]
        public JObject GetInforInvoice(JObject model)
        {

            JObject result = this._searchService.GetInforMulti(model);
            return result;
        }

        [HttpPost]
        [Route("Search/SearchThongTinNM")]
        [AllowAnonymous]
        public async Task<JObject> GetThongTinNM(JObject model)
        {
            string masothue = model["masothue"].ToString().Replace("-", "");
            string key = model["key"].ToString();
            JObject json = await _searchService.GetThongTinNM(model);
            return json;
        }


        //[HttpPost]
        //[Route("Search/ExportXmlInvoice78")]
        //[AllowAnonymous]
        //public HttpResponseMessage ExportZipFileXml78(JObject model)
        //{
        //    HttpResponseMessage result;
        //    try
        //    {
        //        string masothue = model["masothue"].ToString().Replace("-", "");
        //        string sobaomat = model["sobaomat"].ToString();
        //        string originalString = ActionContext.Request.RequestUri.OriginalString;
        //        string path = originalString.StartsWith("/api") ? "~/api/Content/report/" : "~/Content/report/";
        //        TracuuHDDTContext tracuu = conn.getdb();
        //        var checkTraCuu = tracuu.inv_customer_banneds.FirstOrDefault(x =>
        //           x.mst.Replace("-", "").Equals(masothue.Replace("-", "")) && x.type.Equals("KHOATRACUU") && x.is_unblock == false);
        //        if (checkTraCuu != null && !string.IsNullOrEmpty(checkTraCuu.mst))
        //        {
        //            throw new Exception("Quý khách đang bị khóa tra cứu. Vui lòng liên hệ admin để giải quyết");
        //        }
        //        if (sobaomat.Length > 16)
        //        {
        //            throw new Exception("Chuỗi số bảo mật vượt quá quy định ");
        //        }
        //        var withoutSpecial = new string(sobaomat.Where(c => Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c)).ToArray());
        //        if (sobaomat != withoutSpecial)
        //        {
        //            throw new Exception("Chuỗi số bảo mật không đúng định dạng ");
        //        }
        //        var folder = System.Web.HttpContext.Current.Server.MapPath(path);
        //        string fileName = "";
        //        string key = "";


        //        byte[] bytes = _searchService.ExportXmlInvoieMulty(sobaomat, masothue, folder, ref fileName, ref key);

        //        result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) };
        //        result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attach");
        //        result.Content.Headers.ContentDisposition.FileName = fileName + ".zip";
        //        result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
        //        result.Content.Headers.ContentLength = bytes.Length;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(ex.Message) };
        //        result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        //        result.Content.Headers.ContentLength = ex.Message.Length;
        //    }
        //    return result;
        //}


        //[HttpPost]
        //[Route("Search/ExportXmlInvoice781")]
        //[AllowAnonymous]
        //public async Task<HttpResponseMessage> ExportZipFileXml781(JObject model)
        //{
        //    try
        //    {
        //        string fileName = "";
        //        string masothue = model["masothue"].ToString().Replace("-", "");
        //        string sobaomat = model["sobaomat"].ToString();
        //        string xml = _searchService.ExportXmlInvoie781(sobaomat, masothue, ref fileName);
        //        byte[] bytes = Encoding.UTF8.GetBytes(xml);
        //        HttpResponseMessage result2 = new HttpResponseMessage(HttpStatusCode.OK);
        //        result2.Content = new ByteArrayContent(bytes);
        //        result2.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //        result2.Content.Headers.ContentDisposition.FileName = "InvoiceXML.xml";
        //        result2.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
        //        result2.Content.Headers.ContentLength = bytes.Length;
        //        return result2;
        //    }
        //    catch (Exception ex2)
        //    {
        //        Exception ex = ex2;
        //        HttpResponseMessage result2 = new HttpResponseMessage(HttpStatusCode.OK);
        //        result2.Content = new StringContent(ex.Message);
        //        result2.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        //        result2.Content.Headers.ContentLength = ex.Message.Length;
        //        return result2;
        //    }
        //}


    }
}
