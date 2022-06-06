using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InvoiceApi.Models.hoadon68
{
    public class TTHDLQuan
    {
        [XmlElement(ElementName = "TCHDon")]
        public string TCHDon { get; set; }
        [XmlElement(ElementName = "LHDCLQuan")]
        public string LHDCLQuan { get; set; }
        [XmlElement(ElementName = "KHMSHDCLQuan")]
        public string KHMSHDCLQuan { get; set; }
        [XmlElement(ElementName = "KHHDCLQuan")]
        public string KHHDCLQuan { get; set; }
        [XmlElement(ElementName = "SHDCLQuan")]
        public string SHDCLQuan { get; set; }
        [XmlElement(ElementName = "NLHDCLQuan")]
        public string NLHDCLQuan { get; set; }
        [XmlElement(ElementName = "GChu")]
        public string GChu { get; set; }
    }
}
