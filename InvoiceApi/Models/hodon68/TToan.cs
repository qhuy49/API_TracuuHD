using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InvoiceApi.Models.hoadon68
{
    public class TToan
    {
        public List<LTSuat> THTTLTSuat { get; set; }
        //[XmlElement(ElementName = "TgTHHDVu")]
        //public double TgTHHDVu { get; set; }
        [XmlElement(ElementName = "TgTCThue")]
        public double TgTCThue { get; set; }
        [XmlElement(ElementName = "TgTThue")]
        public double TgTThue { get; set; }
        public List<LPhi> DSLPhi { get; set; }
        [XmlElement(ElementName = "TTCKTMai")]
        public double TTCKTMai { get; set; }
        [XmlElement(ElementName = "TgTTTBSo")]
        public double TgTTTBSo { get; set; }
        [XmlElement(ElementName = "TgTTTBChu")]
        public string TgTTTBChu { get; set; }
      
    }
}
