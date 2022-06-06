using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InvoiceApi.Models.hoadon68
{
    public class HHDVu
    {
        [XmlElement(ElementName = "TChat")]
        public int tchat { get; set; }
        [XmlElement(ElementName = "STT")]
        public string stt { get; set; }

        [XmlElement(ElementName = "MHHDVu")]
        public string ma { get; set; }

        [XmlElement (ElementName = "THHDVu")]
        public string ten { get; set; }
        [XmlElement(ElementName = "DVTinh")]
        public string mdvtinh { get; set; }

        [XmlElement(ElementName = "SLuong")]
        public double sluong { get; set; }
        [XmlElement(ElementName = "DGia")]
        public double dgia { get; set; }
        [XmlElement(ElementName = "TLCKhau")]
        public double tlckhau { get; set; }

        [XmlElement(ElementName = "STCKhau")]
        public double stckhau { get; set; }
        [XmlElement(ElementName = "ThTien")]
        public double thtien { get; set; }
        //[XmlElement(ElementName = "TGia")]
        //public int TGia { get; set; }
        [XmlElement(ElementName = "TSuat")]
        public string TSuat1 { get; set; }

        public List<TTin> TTKhac { get; set; }
    }
}
