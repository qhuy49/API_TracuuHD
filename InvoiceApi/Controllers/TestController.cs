using InvoiceApi.IService;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace InvoiceApi.Controllers
{
    public class TestController : BaseApiController
    {
        private readonly ITestService _testService;
        public TestController(ITestService testService)
        {
            _testService = testService;
        }
        [HttpGet]
        [Route("Account/doanpv")]
        [AllowAnonymous]
        public JArray doanpv()
        {
            JArray jarr = new JArray { "doanpv", "test excel" };
            return jarr;
        }

        //[HttpPost]
        //[Route("doanpv/excel")]
        //[AllowAnonymous]
        //public HttpRequestMessage Excel()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("ID", typeof(string));
        //    dt.Columns.Add("Name", typeof(string));
        //    //populate your Datatable

        //    SqlParameter param = new SqlParameter("@USP_Insert_Employee_Infi", SqlDbType.Structured)
        //    {
        //        TypeName = "dbo.userdefinedtabletype",
        //        Value = dt
        //    };
        //    sqlComm.Parameters.Add(param);
        //    return null;
        //}


        [HttpGet]
        [Route("test/inhoadon")]
        [AllowAnonymous]
        public HttpResponseMessage PrintfInvoice1()
        {
            HttpResponseMessage result = null;

            try
            {
                string originalString = this.ActionContext.Request.RequestUri.OriginalString;
                string path = originalString.StartsWith("/api") ? "~/api/Content/report/" : "~/Content/report/";

                var folder = System.Web.HttpContext.Current.Server.MapPath(path);

                byte[] bytes = _testService.PrintThongDiep();

                result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                result.Content.Headers.ContentDisposition.FileName = "report.pdf";

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
