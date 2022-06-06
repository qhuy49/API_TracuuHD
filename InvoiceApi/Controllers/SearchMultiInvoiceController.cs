using InvoiceApi.IService;
using InvoiceApi.Services;
using InvoiceApi.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace InvoiceApi.Controllers
{
    public class SearchMultiInvoiceController : BaseApiController
    {
        private readonly ISearchMultiInvoiceService _searchMultiService;
        public SearchMultiInvoiceController(ISearchMultiInvoiceService searchMultiService)
        {
            this._searchMultiService = searchMultiService;
        }

        [HttpPost]
        [Route("invoice/SearchMultiInvoice")]
        public JObject SearchInvoice(JObject model)
        {
            JObject result = this._searchMultiService.SearchInvoieCus(model);
            //if (result.ContainsKey("code"))
            //{
            //    if (result["code"].ToString() == "99")
            //    {
            //        HttpResponseMessage message= new HttpResponseMessage();
            //        message = Request.CreateResponse(HttpStatusCode.BadRequest, "Token không đúng");
            //    }
            //}
            return result;
        }

        [HttpPost]
        [Route("Search/PrintInvoiceCusPDF")]
        [AllowAnonymous]
        public HttpResponseMessage PrintInvoiceCusPDF(JObject model)
        {
            HttpResponseMessage result;
            try
            {
                string type = model["type"].ToString();
                string sobaomat = model["sobaomat"].ToString();
                string masothue = model["masothue"].ToString().Replace("-","");
                bool inchuyendoi = model.ContainsKey("inchuyendoi");
                TracuuHDDTContext tracuu = conn.getdb();
                string originalString = ActionContext.Request.RequestUri.OriginalString;
                string path = originalString.StartsWith("/api") ? "~/api/Content/report/" : "~/Content/report/";
                var folder = System.Web.HttpContext.Current.Server.MapPath(path);
                string fileName;
                string xml;
                if (sobaomat.Length > 16)
                {
                    throw new Exception("Chuỗi số bảo mật vượt quá quy định ");
                }
                byte[] bytes = _searchMultiService.PrintInvoiceFromSbm(sobaomat, masothue, folder, type, inchuyendoi, out xml, out fileName);

                result = new HttpResponseMessage(HttpStatusCode.OK);

                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                result.Content.Headers.ContentDisposition.FileName = "invoice.pdf";
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentLength = bytes.Length;
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent(ex.Message);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                result.Content.Headers.ContentLength = ex.Message.Length;
            }
            return result;
        }
    }
}
