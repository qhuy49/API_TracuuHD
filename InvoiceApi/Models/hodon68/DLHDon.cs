using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InvoiceApi.Models.hoadon68
{
    [System.Xml.Serialization.XmlRootAttribute(IsNullable = true)]
    public class DLHDon
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }
        //public string Id
        //{
        //    get
        //    { return "data"; }
        //    set
        //    {
        //        this.Id = value;
        //    }
        //}
        public TTChung TTChung { get; set; }
        public NDHDon NDHDon { get; set; }
        public string DLQRCode { get; set; }
        public List<TTin> TTKhac { get; set; }
    }
}
