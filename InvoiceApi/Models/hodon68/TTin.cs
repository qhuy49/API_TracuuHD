using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InvoiceApi.Models.hoadon68
{
    public class TTin
    {
        [XmlElement(ElementName = "TTruong")]
        public string TTruong { get; set; }
        [XmlElement(ElementName = "KDLieu")]
        public string KDLieu { get; set; }
        [XmlElement(ElementName = "DLieu")]
        public string DLieu { get; set; }
    }
}
