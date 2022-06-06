using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceApi.Attributes
{
    public class TokenSpecifi
    {
        public class TokenHard
        {
            public string token { get; set; }
            public string mst { get; set; }
            public string cong_ty { get; set; }
            public string ghi_chu { get; set; }
            public string sailt { get; set; }
        }

        public static List<TokenHard> lstToken = new List<TokenHard>
        {

        };
    }
}