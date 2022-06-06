using InvoiceApi.Authorization;
using InvoiceApi.Data;
using InvoiceApi.Data.Domain;
using InvoiceApi.IService;
using InvoiceApi.Services;
using InvoiceApi.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Http;

namespace InvoiceApi.Controllers
{
    public class SearchInvoiceController : ApiController
    {
        private ISearchInvoiceService _tracuuService;
        private readonly ISearchService _searchService;


        public SearchInvoiceController(ISearchInvoiceService tracuuService, ISearchService searchService)
        {
            this._tracuuService = tracuuService;
            this._searchService = searchService;
        }

        [HttpGet]
        [Route("Tracuu/TaxPath")]
        [AllowAnonymous]
        public string TaxPath(string model)
        {
            JObject json = new JObject();
            json.Add("error", "");

            try
            {
                json.Add("ok", true);
            }
            catch (Exception ex)
            {
                json["error"] = ex.Message;
            }

            return json.ToString();
        }


        [HttpGet]
        [Route("Tracuu/TaxPath1")]
        [AllowAnonymous]
        public JObject TaxPath1(JObject model)
        {
            JObject json = new JObject();
            json.Add("error", "");

            try
            {
                json.Add("ok", true);
            }
            catch (Exception ex)
            {
                json["error"] = ex.Message;
            }

            return json;
        }


        [HttpGet]
        [Route("Tracuu/M-Invoice")]
        [AllowAnonymous]
        public HttpResponseMessage PrintInvoice(string mst, string id)
        {

            HttpResponseMessage result = null;
            try
            {
                if (_tracuuService == null)
                {
                    throw new Exception("Không tồn tại mst:");
                }
                string type = "PDF";
                string originalString = this.ActionContext.Request.RequestUri.OriginalString;
                string path = originalString.StartsWith("/api") ? "~/api/Content/report/" : "~/Content/report/";
                //string path = "~/Content/report/";
                var folder = System.Web.HttpContext.Current.Server.MapPath(path);

                //byte[] bytes = _tracuuService.inHoadonACB(id, mst, folder, type, false);
                byte[] bytes = _tracuuService.PrintInvoiceFromSbm(id, mst, folder, type);


                result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);

                if (type == "PDF")
                {
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline");
                    result.Content.Headers.ContentDisposition.FileName = "InvoiceTemplate.pdf";
                    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                }
                else if (type == "Html")
                {
                    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
                }
                result.Content.Headers.ContentLength = bytes.Length;
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent(ex.Message, System.Text.Encoding.UTF8);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
                result.Content.Headers.ContentLength = ex.Message.Length;
            }

            return result;
        }


        [HttpPost]
        [Route("tracuu2/getinvoicexml")]
        [AllowAnonymous]
        public HttpResponseMessage GetInvoiceXml(JObject model)
        {
            HttpResponseMessage result;
            try
            {
                string masothue = model["masothue"].ToString();
                string sobaomat = model["sobaomat"].ToString();
                byte[] bytes = _tracuuService.GetInvoiceXml(sobaomat, masothue);
                result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) };

                if (sobaomat.Length > 16)
                {
                    throw new Exception("Chuỗi số bảo mật vượt quá quy định ");
                }
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attach")
                {
                    FileName = "invoice.xml"
                };

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
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

        //=======================vacom==============================

        [HttpPost]
        [AllowAnonymous]
        [Route("Tracuu2/GetInfoInvoice")]
        public JObject GetInfoInvoice(JObject model)
        {
            JObject json = _tracuuService.GetInfoInvoice(model);
            return json;
        }

        [HttpPost]
        [Route("tracuu2/searchinvoice")]
        [Authorize]
        [BaseAuthentication]

        public JObject SearchInvoice(JObject model)
        {
            string userName;
            string mst;
            string maDt;
            var claimsIdentity = RequestContext.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var listClaim = claimsIdentity.Claims.ToList();
                userName = listClaim.FirstOrDefault(x => x.Type == "username")?.Value;
                mst = listClaim.FirstOrDefault(x => x.Type == "mst")?.Value;
                maDt = listClaim.FirstOrDefault(x => x.Type == "ma_dt")?.Value;
                model.Add("user_name", userName);
                model.Add("mst", mst);
                model.Add("ma_dt", maDt);
            }
            return _tracuuService.SearchInvoice(model);
        }


        [HttpPost]
        [Route("Tracuu2/ExportZipFileXML")]
        [AllowAnonymous]
        public HttpResponseMessage ExportZipFileXml(JObject model)
        {
            HttpResponseMessage result;
            try
            {
                string masothue = model["masothue"].ToString();
                string sobaomat = model["sobaomat"].ToString();
                string originalString = ActionContext.Request.RequestUri.OriginalString;
                string path = originalString.StartsWith("/api") ? "~/api/Content/report/" : "~/Content/report/";
                TracuuHDDTContext tracuu = conn.getdb();
                //var tracuu = "";
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
                var folder = System.Web.HttpContext.Current.Server.MapPath(path);
                string fileName = "";
                string key = "";
                byte[] bytes = _searchService.ExportXmlInvoieMulty(sobaomat, masothue, folder, ref fileName, ref key);
                //byte[] bytes = _tracuuService.ExportZipFileXml(sobaomat, masothue, folder, ref fileName, ref key);
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
        [Route("Tracuu2/ExportZipFile")]
        [Authorize]
        [BaseAuthentication]
        public HttpResponseMessage ExportZipFile(JObject model)
        {
            HttpResponseMessage result;
            try
            {
                string masothue = model["masothue"].ToString();
                string sobaomat = model["sobaomat"].ToString();
                string originalString = ActionContext.Request.RequestUri.OriginalString;
                string path = originalString.StartsWith("/api") ? "~/api/Content/report/" : "~/Content/report/";
                var folder = System.Web.HttpContext.Current.Server.MapPath(path);
                string fileName = "";
                string key = "";
                if (sobaomat.Length > 16)
                {
                    throw new Exception("Chuỗi số bảo mật vượt quá quy định ");
                }
                byte[] bytes = _tracuuService.ExportZipFileXml(sobaomat, masothue, folder, ref fileName, ref key);
                result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) };
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attach");
                result.Content.Headers.ContentDisposition.FileName = fileName + ".zip";
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
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
        [Route("Tracuu2/PrintInvoice")]
        [AllowAnonymous]
        public HttpResponseMessage PrintInvoice(JObject model)
        {
            HttpResponseMessage result;
            try
            {
                string type = model["type"].ToString();
                string sobaomat = model["sobaomat"].ToString();
                string masothue = model["masothue"].ToString();
                bool inchuyendoi = model.ContainsKey("inchuyendoi");
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
                string originalString = ActionContext.Request.RequestUri.OriginalString;
                string path = originalString.StartsWith("/api") ? "~/api/Content/report/" : "~/Content/report/";
                var folder = System.Web.HttpContext.Current.Server.MapPath(path);
                string fileName;
                byte[] bytes = _searchService.PrintInvoiceSBM(sobaomat, masothue, folder, type, inchuyendoi, out fileName);
                result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) };
                if (type == "PDF")
                {
                    result.Content.Headers.ContentDisposition =
                        new ContentDispositionHeaderValue("inline") { FileName = fileName };
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                }
                else if (type == "Html")
                {
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                }
                result.Content.Headers.ContentLength = bytes.Length;
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message, System.Text.Encoding.UTF8)
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                result.Content.Headers.ContentLength = ex.Message.Length;
            }
            return result;
        }

        [HttpPost]
        [Route("Tracuu2/PrintInvoicePdf")]
        [AllowAnonymous]
        public JObject PrintInvoicePdf(JObject model)
        {
            JObject result = new JObject();
            try
            {
                string type = model["type"].ToString();
                string sobaomat = model["sobaomat"].ToString();
                string masothue = model["masothue"].ToString();

                var invoiceInfo = _tracuuService.GetInfoInvoice(model);
                if (invoiceInfo.ContainsKey("error"))
                {
                    return invoiceInfo;
                }
                if (sobaomat.Length > 16)
                {
                    throw new Exception("Chuỗi số bảo mật vượt quá quy định ");
                }
                TracuuHDDTContext tracuu = conn.getdb();
                var checkTraCuu = tracuu.inv_customer_banneds.FirstOrDefault(x =>
                    x.mst.Replace("-", "").Equals(masothue) && x.type.Equals("KHOATRACUU") && x.is_unblock == false);
                if (checkTraCuu != null && !string.IsNullOrEmpty(checkTraCuu.mst))
                {
                    result.Add("error", $"Quý khách đang bị khóa tra cứu. Vui lòng liên hệ admin để giải quyết");
                    return result;
                }

                string originalString = ActionContext.Request.RequestUri.OriginalString;
                string path = originalString.StartsWith("/api") ? "~/api/Content/report/" : "~/Content/report/";
                var folder = System.Web.HttpContext.Current.Server.MapPath(path);
                string xml;
                string fileName;
                byte[] bytes = _tracuuService.PrintInvoiceFromSbmVC(sobaomat, masothue, folder, type, false, out xml, out fileName);

                string a = Convert.ToBase64String(bytes);


                result.Add("data", invoiceInfo);
                result.Add("ok", a);
                result.Add("ecd", xml);
                result.Add("fileName", fileName.Replace("-", ""));
            }
            catch (Exception ex)
            {
                result.Add("error", ex.Message);
            }

            return result;
        }

        [HttpPost]
        [Route("Customer/GetInvoiceFromdateTodate")]
        [Authorize]
        public JObject GetInvoiceFromdateTodate(JObject model)
        {
            string userName;
            string mst;
            string maDt;
            var claimsIdentity = RequestContext.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var listClaim = claimsIdentity.Claims.ToList();
                userName = listClaim.FirstOrDefault(x => x.Type == "username")?.Value;
                mst = listClaim.FirstOrDefault(x => x.Type == "mst")?.Value;
                maDt = listClaim.FirstOrDefault(x => x.Type == "ma_dt")?.Value;
                model.Add("user_name", userName);
                model.Add("Mst", mst);
                model.Add("ma_dt", maDt);
            }

            string tu_ngay = model["tu_ngay"].ToString();
            string den_ngay = model["den_ngay"].ToString();
            JObject result = this._tracuuService.FromDateToDate(model, tu_ngay, den_ngay);
            return result;
        }

        #region api searchTax dùng cho tool bảo trì
        [HttpGet]
        [Route("tracuu2/searchTax")]
        [AllowAnonymous]
        public JObject SearchTax(String model)
        {
            return _tracuuService.Search_Tax(model);
        }
        #endregion

        //tra cứu 78 ACB

        [HttpGet]
        [Route("Tracuu/PrintACB")]
        [AllowAnonymous]
        public HttpResponseMessage PrintInvoiceACB(string mst, string id)
        {

            HttpResponseMessage result = null;
            try
            {
                if (_tracuuService == null)
                {
                    throw new Exception("Không tồn tại mst:");
                }
                string type = "PDF";
                string originalString = this.ActionContext.Request.RequestUri.OriginalString;
                string path = originalString.StartsWith("/api") ? "~/api/Content/report/" : "~/Content/report/";
                //string path = "~/Content/report/";
                var folder = System.Web.HttpContext.Current.Server.MapPath(path);

                byte[] bytes = _tracuuService.PrintInvoiceFromSbm(id, mst, folder, type);

                result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);

                if (type == "PDF")
                {
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline");
                    result.Content.Headers.ContentDisposition.FileName = "InvoiceTemplate.pdf";
                    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                }
                else if (type == "Html")
                {
                    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
                }
                result.Content.Headers.ContentLength = bytes.Length;
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent(ex.Message, System.Text.Encoding.UTF8);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
                result.Content.Headers.ContentLength = ex.Message.Length;
            }

            return result;
        }
    }


}
