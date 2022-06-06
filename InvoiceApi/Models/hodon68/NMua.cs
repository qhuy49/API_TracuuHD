using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InvoiceApi.Models.hoadon68
{
    public class NMua
    {
        [XmlElement(ElementName = "Ten")]
        public string ten { get; set; }
        [XmlElement(ElementName = "MST")]
        public string mst { get; set; }
        [XmlElement(ElementName = "DChi")]
        public string dchi { get; set; }
        [XmlElement(ElementName = "MKHang")]
        public string mnmua { get; set; }
        [XmlElement(ElementName = "SDThoai")]
        public string SDThoai { get; set; }
        [XmlElement(ElementName = "DCTDTu")]
        public string email { get; set; }
        [XmlElement(ElementName = "HVTNMHang")]
        public string tnmua { get; set; }
        [XmlElement(ElementName = "STKNHang")]
        public string stknmua { get; set; }
        [XmlElement(ElementName = "TNHang")]
        public string nganhang_ngmua { get; set; }
    }
}
