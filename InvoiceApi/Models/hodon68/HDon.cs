using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InvoiceApi.Models.hoadon68
{
    public class HDon
    {
        public DLHDon DLHDon { get; set; }
        public string MCCQT { get; set; }
        public DSCKS DSCKS { get; set; }
    }
}
