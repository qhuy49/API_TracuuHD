using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InvoiceApi.Models.hoadon68
{
    [System.Xml.Serialization.XmlRootAttribute(IsNullable = true)]
    public class TTChung
    {
        [XmlElement(ElementName = "PBan")]
        public string PBan { get; set; }
        [XmlElement(ElementName = "THDon")]
        public string THDon { get; set; }
        [XmlElement(ElementName = "KHMSHDon")]
        public string KHMSHDon { get; set; }
        [XmlElement(ElementName = "KHHDon")]
        public string KHHDon { get; set; }
        [XmlElement(ElementName = "SHDon")]
        public string SHDon { get; set; }
        [XmlElement(ElementName = "MHSo")]
        public string MHSo { get; set; }
        [XmlElement(ElementName = "NLap")]
        public string NLap { get; set; }
        [XmlElement(ElementName = "DVTTe")]
        public string DVTTe { get; set; }
        [XmlElement(ElementName = "HDXKhau")]
        public string HDXKhau { get; set; }
        [XmlElement(ElementName = "HDDCKPTQuan")]
        public string HDDCKPTQuan { get; set; }
        [XmlElement(ElementName = "SBKe")]
        public string SBKe { get; set; }
        [XmlElement(ElementName = "NBKe")]
        public string NBKe { get; set; }
        [XmlElement(ElementName = "HTTToan")]
        public string HTTToan { get; set; }
        [XmlElement(ElementName = "MSTTCGP")]
        public string MSTTCGP { get; set; }
        [XmlElement(ElementName = "TGia")]
        public string TGia { get; set; }
        [XmlElement(ElementName = "THTTTKhac")]
        public string THTTTKhac { get; set; }
        [XmlElement(ElementName = "MSTDVCCHDDTu")]
        public string MSTDVCCHDDTu { get; set; }
        [XmlElement(ElementName = "MSTDVNUNLHDon")]
        public string MSTDVNUNLHDon { get; set; }
        [XmlElement(ElementName = "TDVNUNLHDon")]
        public string TDVNUNLHDon { get; set; }
        public TTHDLQuan TTHDLQuan { get; set; }
        public List<TTin> TTKhac { get; set; }
    }
}
