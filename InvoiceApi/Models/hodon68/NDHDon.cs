using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceApi.Models.hoadon68
{
    public class NDHDon
    {
        public NBan NBan { get; set; }
        public NMua NMua { get; set; }
        public List<HHDVu> DSHHDVu { get; set; }
        public TToan TToan { get; set; }
    }
}
