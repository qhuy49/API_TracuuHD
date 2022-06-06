using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InvoiceApi.Models.hoadon68
{
    public class NBan
    {
        [XmlElement(ElementName = "Ten")]
        public string ten_dvcs { get; set; }
        [XmlElement(ElementName = "MST")]
        // hóa đơn 06
        public string ms_thue { get; set; }
        [XmlElement(ElementName = "LDDNBo")]
        public string lddnbo { get; set; }
        [XmlElement(ElementName = "DChi")]
        public string dia_chi { get; set; }
        [XmlElement(ElementName = "SDThoai")]
        public string dien_thoai { get; set; }
        [XmlElement(ElementName = "DCTDTu")]
        public string email_nguoiban { get; set; }
        [XmlElement(ElementName = "STKNHang")]
        //public string tai_khoan { get; set; }
        public string stknban { get; set; }
        [XmlElement(ElementName = "TNHang")]
        public string nganhang_ngban { get; set; }
        [XmlElement(ElementName = "Fax")]
        public string fax { get; set; }
        [XmlElement(ElementName = "Website")]
        public string Website { get; set; }
        
        [XmlElement(ElementName = "HDSo")]
        public string sohopdong { get; set; }
        [XmlElement(ElementName = "HVTNXHang")]
        public string tennguoinhanhang { get; set; }
        [XmlElement(ElementName = "TNVChuyen")]
        public string tnvchuyen { get; set; }
        [XmlElement(ElementName = "PTVChuyen")]
        public string ptvchuyen { get; set; }
    }
}
