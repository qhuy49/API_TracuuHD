using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InvoiceApi.Models.hoadon68
{
    public class DSCKS
    {
        [XmlElement(ElementName = "NBan")]
        public string NBan { get; set; }
        [XmlElement(ElementName = "NMua")]
        public string NMua { get; set; }
        [XmlElement(ElementName = "CCKSKhac")]
        public string CCKSKhac { get; set; }
    }
}
