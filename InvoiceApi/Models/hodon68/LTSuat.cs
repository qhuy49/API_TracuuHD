using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InvoiceApi.Models.hoadon68
{
    public class LTSuat
    {
        [XmlElement(ElementName = "TSuat")]
        public string TSuat { get; set; }
        [XmlElement(ElementName = "ThTien")]
        public double ThTien { get; set; }
        [XmlElement(ElementName = "TThue")]
        public double TThue { get; set; }
    }
}
