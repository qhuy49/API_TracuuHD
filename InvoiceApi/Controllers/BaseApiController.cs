using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using InvoiceApi.Attributes;
using System.Web.Http.Controllers;

namespace InvoiceApi.Controllers
{
    [RestAuthorizeAttribute]
    public abstract class BaseApiController : ApiController
    {
        public string UserName { get; set; }
        public string MstLog { get; set; }
        public string Ma_dvcs { get; set; }
        
    }
}