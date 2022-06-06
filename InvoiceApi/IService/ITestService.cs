using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceApi.IService
{
    public partial interface ITestService
    {
        byte[] inHoadon1();
        byte[] inHoadon2();

        byte[] inHoadon3();
        byte[] inHoadon4();
        byte[] inHoadon5();
        byte[] inHoadonTest();
        byte[] PrintThongDiep();

    }
}