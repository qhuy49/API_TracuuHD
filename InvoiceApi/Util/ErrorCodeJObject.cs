using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceApi.Util
{
    public class ErrorCodeJObject
    {
        public string code { get; set; }

        public string message { get; set; }

        public JObject data { get; set; }
    }
}