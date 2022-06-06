using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InvoiceApi.Models.hoadon68
{
    public class LPhi
    {
        [XmlElement(ElementName = "TLPhi")]
        public decimal TLPhi { get; set; }
        [XmlElement(ElementName = "TPhi")]
        public decimal TPhi { get; set; }
    }
}
