using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml;

namespace InvoiceApi.Services
{
    public class PrintTD
    {
        #region In các thông điệp thuế trả về
        public byte[] PrintThongDiep(string xml, string folder, string type)
        {
            byte[] response = null;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.PreserveWhitespace = true;
                xmlDocument.LoadXml(xml);
                DataSet ds = new DataSet();
                switch (type)
                {
                    case "204":
                        ds = InTD204(xmlDocument, ds);
                        response = Util.ReportUtil.PrintReport(ds, folder, "PDF", 1);
                        break;
                    case "202":
                        ds = InTD202(xmlDocument, ds);
                        response = Util.ReportUtil.PrintReport(ds, folder, "PDF", 1);
                        break;
                    case "102":
                        ds = InTD102(xmlDocument, ds);
                        response = Util.ReportUtil.PrintReport(ds, folder, "PDF", 1);
                        break;
                    case "103":
                        ds = InTD103(xmlDocument, ds);
                        response = Util.ReportUtil.PrintReport(ds, folder, "PDF");
                        break;
                    case "301":
                        ds = InTD301(xmlDocument, ds);
                        response = Util.ReportUtil.PrintReport(ds, folder, "PDF", 1);
                        break;
                }
                if (response == null)
                {
                    throw new Exception("Không tải được mẫu thông điệp [Data is null]");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }
        private DataSet InTD204(XmlDocument xmlDocument, DataSet ds)
        {
            XmlNodeList MSo = xmlDocument.GetElementsByTagName("MSo");
            XmlNodeList NTBao = xmlDocument.GetElementsByTagName("NTBao");
            XmlNodeList TNNT = xmlDocument.GetElementsByTagName("TNNT");
            string MST = xmlDocument.GetElementsByTagName("MST")[0].InnerText.ToString();
            XmlNodeList CCu = xmlDocument.GetElementsByTagName("CCu");
            XmlNodeList MTDTChieu = xmlDocument.GetElementsByTagName("MTDTChieu");
            XmlNodeList SLuong = xmlDocument.GetElementsByTagName("SLuong");
            XmlNodeList KHMSHDon = xmlDocument.GetElementsByTagName("KHMSHDon");
            XmlNodeList SHDon = xmlDocument.GetElementsByTagName("SHDon");
            XmlNodeList KHHDon = xmlDocument.GetElementsByTagName("KHHDon");
            XmlNodeList NLap = xmlDocument.GetElementsByTagName("NLap");
            XmlNodeList X509SubjectName = xmlDocument.GetElementsByTagName("X509SubjectName");
            XmlNodeList SigningTime = xmlDocument.GetElementsByTagName("SigningTime");
            XmlNodeList So = xmlDocument.GetElementsByTagName("So");
            XmlNodeList LTBao = xmlDocument.GetElementsByTagName("LTBao");
            XmlNodeList TGGui = xmlDocument.GetElementsByTagName("TGGui");
            XmlNodeList DDanh = xmlDocument.GetElementsByTagName("DDanh");
            DataTable TTHDon = new DataTable();
            var col = new List<DataColumn>() {
                        new DataColumn(){ColumnName = "mso"},
                        new DataColumn(){ColumnName = "ntbao"},
                        new DataColumn(){ColumnName = "tnnt"},
                        new DataColumn(){ ColumnName = "mst"},
                        new DataColumn(){ColumnName = "ccu"},
                        new DataColumn(){ColumnName = "mtdtchieu"},
                        new DataColumn(){ColumnName = "sluong"},
                        new DataColumn(){ColumnName= "khmshdon"},
                        new DataColumn(){ColumnName = "shdon"},
                        new DataColumn(){ColumnName = "khhdon"},
                        new DataColumn(){ColumnName = "nlap"},
                        new DataColumn(){ColumnName = "so"},
                        new DataColumn(){ColumnName = "ltbao"},
                        new DataColumn(){ColumnName = "tGGui"},
                        new DataColumn(){ColumnName = "dDanh"}
                    };

            TTHDon.Columns.AddRange(col.ToArray());
            DataRow dr = TTHDon.NewRow();
            TTHDon.Rows.Add(dr);
            TTHDon.Rows[0]["mst"] = MST;
            if (KHMSHDon.Count > 0)
                TTHDon.Rows[0]["khmshdon"] = KHMSHDon[0].InnerText;
            if (SHDon.Count > 0)
                TTHDon.Rows[0]["shdon"] = SHDon[0].InnerText;
            if (KHHDon.Count > 0)
                TTHDon.Rows[0]["khhdon"] = KHHDon[0].InnerText;
            if (NLap.Count > 0)
                TTHDon.Rows[0]["nlap"] = NLap[0].InnerText;
            if (MSo.Count > 0)
                TTHDon.Rows[0]["mso"] = MSo[0].InnerText;
            if (NTBao.Count > 0)
                TTHDon.Rows[0]["ntbao"] = NTBao[0].InnerText;
            if (TNNT.Count > 0)
                TTHDon.Rows[0]["tnnt"] = TNNT[0].InnerText;
            if (CCu.Count > 0)
                TTHDon.Rows[0]["ccu"] = CCu[0].InnerText;
            if (SLuong.Count > 0)
                TTHDon.Rows[0]["sluong"] = SLuong[0].InnerText;
            if (So.Count > 0)
                TTHDon.Rows[0]["so"] = So[0].InnerText;
            if (LTBao.Count > 0)
                TTHDon.Rows[0]["ltbao"] = LTBao[0].InnerText;
            if (TGGui.Count > 0)
                TTHDon.Rows[0]["tGGui"] = TGGui[0].InnerText;
            if (TGGui.Count > 0)
                TTHDon.Rows[0]["dDanh"] = DDanh[0].InnerText;
            if (MTDTChieu.Count > 0)
                TTHDon.Rows[0]["mtdtchieu"] = MTDTChieu[0].InnerText;


            ds.Tables.Add(TTHDon);
            string ltbao = xmlDocument.GetElementsByTagName("LTBao")[0].InnerText.ToString();
            DataTable dSLDo = new DataTable();
            if (ltbao == "5")
            {
                XmlNodeList STT = xmlDocument.GetElementsByTagName("STT");
                XmlNodeList KDLieu = xmlDocument.GetElementsByTagName("KDLieu");
                XmlNodeList LDau = xmlDocument.GetElementsByTagName("LDau");
                XmlNodeList SBTHDLieu = xmlDocument.GetElementsByTagName("SBTHDLieu");
                XmlNodeList BSLThu = xmlDocument.GetElementsByTagName("BSLThu");
                XmlNodeList MLoi = xmlDocument.GetElementsByTagName("MLoi");
                XmlNodeList MTLoi = xmlDocument.GetElementsByTagName("MTLoi");
                XmlNodeList HDXLy = xmlDocument.GetElementsByTagName("HDXLy");

                var col1 = new List<DataColumn>() {
                           new DataColumn(){ ColumnName= "sTT"},
                           new DataColumn(){ColumnName = "kDLieu"},
                           new DataColumn(){ ColumnName = "lDau"},
                           new DataColumn(){ColumnName = "sBTHDLieu"},
                           new DataColumn(){ColumnName = "bSLThu" },
                           new DataColumn(){ColumnName = "mLoi"},
                           new DataColumn(){ ColumnName = "mTLoi"},
                           new DataColumn(){ColumnName = "hDXLy"}
                };

                dSLDo.Columns.AddRange(col1.ToArray());
                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("MLoi");
                for (int i = 0; i < nodeList.Count; i++)
                {
                    DataRow dr1 = dSLDo.NewRow();
                    dSLDo.Rows.Add(dr1);
                    if (STT.Count > 0)
                        dSLDo.Rows[i]["sTT"] = STT[i].InnerText;
                    if (KDLieu.Count > 0)
                        dSLDo.Rows[i]["kDLieu"] = KDLieu[i].InnerText;
                    if (LDau.Count > 0)
                        dSLDo.Rows[i]["lDau"] = LDau[i].InnerText;
                    if (SBTHDLieu.Count > 0)
                        dSLDo.Rows[i]["sBTHDLieu"] = SBTHDLieu[i].InnerText;
                    if (BSLThu.Count > 0)
                        dSLDo.Rows[i]["bSLThu"] = BSLThu[i].InnerText;
                    if (MLoi.Count > 0)
                        dSLDo.Rows[i]["mLoi"] = MLoi[i].InnerText;
                    if (MTLoi.Count > 0)
                        dSLDo.Rows[i]["mTLoi"] = MTLoi[i].InnerText;
                    if (HDXLy.Count > 0)
                        dSLDo.Rows[i]["hDXLy"] = HDXLy[i].InnerText;
                }
            }
            else
            {
                XmlNodeList MLoi = xmlDocument.GetElementsByTagName("MLoi");
                XmlNodeList MTLoi = xmlDocument.GetElementsByTagName("MTLoi");
                XmlNodeList HDXLy = xmlDocument.GetElementsByTagName("HDXLy");
                XmlNodeList GChu = xmlDocument.GetElementsByTagName("GChu");
                var col1 = new List<DataColumn>() {
                                new DataColumn(){ColumnName= "mloi"},
                                new DataColumn(){ColumnName = "mtloi"},
                                new DataColumn(){ColumnName = "hdxly"},
                                new DataColumn(){ColumnName = "gchu"}
                                    };
                dSLDo.Columns.AddRange(col1.ToArray());
                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("MLoi");
                for (int i = 0; i < nodeList.Count; i++)
                {
                    DataRow dr1 = dSLDo.NewRow();
                    dSLDo.Rows.Add(dr1);
                    if (MLoi.Count > 0)
                        dSLDo.Rows[i]["mloi"] = MLoi[i].InnerText;
                    if (MTLoi.Count > 0)
                        dSLDo.Rows[i]["mtloi"] = MTLoi[i].InnerText;
                    if (HDXLy.Count > 0)
                        dSLDo.Rows[i]["hdxly"] = HDXLy[i].InnerText;
                    if (GChu.Count > 0)
                        dSLDo.Rows[i]["gchu"] = GChu[i].InnerText;
                }
            }


            

            ds.Tables.Add(dSLDo);
            DataTable dSCKS = new DataTable();
            var col2 = new List<DataColumn>() {
                            new DataColumn(){ColumnName= "x509SubjectName"},
                            new DataColumn(){ColumnName= "signingTime"}
                        };

            dSCKS.Columns.AddRange(col2.ToArray());
            XmlNodeList listCKS = xmlDocument.GetElementsByTagName("X509SubjectName");
            for (int i = 0; i < listCKS.Count; i++)
            {
                DataRow dr1 = dSCKS.NewRow();
                dSCKS.Rows.Add(dr1);

                if (X509SubjectName.Count > 0)
                {
                    dSCKS.Rows[i]["x509SubjectName"] = X509SubjectName[i].InnerText;
                    dSCKS.Rows[i]["signingTime"] = SigningTime[i].InnerText;
                    string chuky = X509SubjectName[i].InnerText;
                    string[] chuky1 = chuky.Split(',');
                    foreach (var item in chuky1)
                    {
                        var a = item.Contains("CN=");
                        if (a == true)
                            dSCKS.Rows[i]["x509SubjectName"] = item.Replace("CN=", "");
                    }
                }
            }
            ds.Tables.Add(dSCKS);
            return ds;
        }
        private DataSet InTD202(XmlDocument xmlDocument, DataSet ds)
        {
            XmlNodeList MTDiep = xmlDocument.GetElementsByTagName("MTDiep");
            XmlNodeList MTDTChieu = xmlDocument.GetElementsByTagName("MTDTChieu");
            XmlNodeList KHMSHDon = xmlDocument.GetElementsByTagName("KHMSHDon");
            XmlNodeList KHHDon = xmlDocument.GetElementsByTagName("KHHDon");
            XmlNodeList SHDon = xmlDocument.GetElementsByTagName("SHDon");
            XmlNodeList NLap = xmlDocument.GetElementsByTagName("NLap");
            XmlNodeList DVTTe = xmlDocument.GetElementsByTagName("DVTTe");
            XmlNodeList HTTToan = xmlDocument.GetElementsByTagName("HTTToan");
            string TenNB = xmlDocument.GetElementsByTagName("Ten")[0].InnerText.ToString();
            string MSTNB = xmlDocument.GetElementsByTagName("MST")[0].InnerText.ToString();
            string DChiNB = xmlDocument.GetElementsByTagName("DChi")[0].InnerText.ToString();
            string SDThoaiNB = "";
            string DCTDTuNB = "";
            string STKNHangNB = "";
            string TNHangNB = "";
            if (xmlDocument.GetElementsByTagName("SDThoai")[0] != null)
                SDThoaiNB = xmlDocument.GetElementsByTagName("SDThoai")[0].InnerText.ToString();
            if (xmlDocument.GetElementsByTagName("DCTDTu")[0] != null)
                DCTDTuNB = xmlDocument.GetElementsByTagName("DCTDTu")[0].InnerText.ToString();
            if (xmlDocument.GetElementsByTagName("STKNHang")[0] != null)
                STKNHangNB = xmlDocument.GetElementsByTagName("STKNHang")[0].InnerText.ToString();
            if (xmlDocument.GetElementsByTagName("TNHang")[0] != null)
                TNHangNB = xmlDocument.GetElementsByTagName("TNHang")[0].InnerText.ToString();
            string TenNM = xmlDocument.GetElementsByTagName("Ten")[1].InnerText.ToString();
            string MSTNM = xmlDocument.GetElementsByTagName("MST")[2].InnerText.ToString();
            string DChiNM = xmlDocument.GetElementsByTagName("DChi")[1].InnerText.ToString();
            string SDThoaiNM = "";
            string DCTDTuNM = "";
            string STKNHangNM = "";
            string TNHangNM = "";
            string HVTNMHang = "";
            if (xmlDocument.GetElementsByTagName("SDThoai")[1] != null)
                SDThoaiNM = xmlDocument.GetElementsByTagName("SDThoai")[1].InnerText.ToString();
            if (xmlDocument.GetElementsByTagName("DCTDTu")[1] != null)
                DCTDTuNM = xmlDocument.GetElementsByTagName("DCTDTu")[1].InnerText.ToString();
            if (xmlDocument.GetElementsByTagName("STKNHang")[1] != null)
                STKNHangNM = xmlDocument.GetElementsByTagName("STKNHang")[1].InnerText.ToString();
            if (xmlDocument.GetElementsByTagName("TNHang")[1] != null)
                TNHangNM = xmlDocument.GetElementsByTagName("TNHang")[1].InnerText.ToString();
            if (xmlDocument.GetElementsByTagName("HVTNMHang")[0] != null)
                HVTNMHang = xmlDocument.GetElementsByTagName("HVTNMHang")[0].InnerText.ToString();
            XmlNodeList TSuat = xmlDocument.GetElementsByTagName("TSuat");
            XmlNodeList ThTien = xmlDocument.GetElementsByTagName("ThTien");
            XmlNodeList TThue = xmlDocument.GetElementsByTagName("TThue");
            XmlNodeList TgTCThue = xmlDocument.GetElementsByTagName("TgTCThue");
            XmlNodeList TgTThue = xmlDocument.GetElementsByTagName("TgTThue");
            XmlNodeList TTCKTMai = xmlDocument.GetElementsByTagName("TTCKTMai");
            XmlNodeList TgTTTBSo = xmlDocument.GetElementsByTagName("TgTTTBSo");
            XmlNodeList TgTTTBChu = xmlDocument.GetElementsByTagName("TgTTTBChu");
            XmlNodeList DLieu = xmlDocument.GetElementsByTagName("DLieu");
            XmlNodeList X509SubjectName = xmlDocument.GetElementsByTagName("X509SubjectName");
            XmlNodeList SigningTime = xmlDocument.GetElementsByTagName("SigningTime");
            XmlNodeList MCCQT = xmlDocument.GetElementsByTagName("MCCQT");
            string TCHDon = "";
            string LHDCLQuan = "";
            string KHMSHDCLQuan = "";
            string KHHDCLQuan = "";
            string SHDCLQuan = "";
            string NLHDCLQuan = "";
            if (xmlDocument.GetElementsByTagName("TCHDon")[0] != null)
                TCHDon = xmlDocument.GetElementsByTagName("TCHDon")[0].InnerText.ToString();
            if (xmlDocument.GetElementsByTagName("LHDCLQuan")[0] != null)
                LHDCLQuan = xmlDocument.GetElementsByTagName("LHDCLQuan")[0].InnerText.ToString();
            if (xmlDocument.GetElementsByTagName("KHMSHDCLQuan")[0] != null)
                KHMSHDCLQuan = xmlDocument.GetElementsByTagName("KHMSHDCLQuan")[0].InnerText.ToString();
            if (xmlDocument.GetElementsByTagName("KHHDCLQuan")[0] != null)
                KHHDCLQuan = xmlDocument.GetElementsByTagName("KHHDCLQuan")[0].InnerText.ToString();
            if (xmlDocument.GetElementsByTagName("SHDCLQuan")[0] != null)
                SHDCLQuan = xmlDocument.GetElementsByTagName("SHDCLQuan")[0].InnerText.ToString();
            if (xmlDocument.GetElementsByTagName("NLHDCLQuan")[0] != null)
                NLHDCLQuan = xmlDocument.GetElementsByTagName("NLHDCLQuan")[0].InnerText.ToString();


            DataTable TTHDon = new DataTable();
            var col = new List<DataColumn>() {
                            new DataColumn(){ColumnName= "mTDiep"},
                            new DataColumn(){ColumnName= "mTDTChieu" },
                            new DataColumn(){ColumnName= "kHMSHDon"},
                            new DataColumn(){ColumnName= "kHHDon"},
                            new DataColumn(){ColumnName= "sHDon"},
                            new DataColumn(){ColumnName= "nLap"},
                            new DataColumn(){ColumnName= "dVTTe"},
                            new DataColumn(){ColumnName= "hTTToan"},
                            new DataColumn(){ColumnName= "tenNB"},
                            new DataColumn(){ColumnName= "mSTNB"},
                            new DataColumn(){ColumnName= "dChiNB"},
                            new DataColumn(){ColumnName= "sDThoaiNB"},
                            new DataColumn(){ColumnName= "dCTDTuNB"},
                            new DataColumn(){ColumnName= "sTKNHangNB"},
                            new DataColumn(){ColumnName= "tNHangNB"},
                            new DataColumn(){ColumnName= "tenNM"},
                            new DataColumn(){ColumnName= "mSTNM"},
                            new DataColumn(){ColumnName= "dChiNM"},
                            new DataColumn(){ColumnName= "sDThoaiNM"},
                            new DataColumn(){ColumnName= "dCTDTuNM"},
                            new DataColumn(){ColumnName= "sTKNHangNM"},
                            new DataColumn(){ColumnName= "tNHangNM"},
                            new DataColumn(){ColumnName= "hVTNMHang"},

                            new DataColumn(){ColumnName= "tSuat"},
                            new DataColumn(){ColumnName= "thTien" },
                            new DataColumn(){ColumnName= "tThue"},
                            new DataColumn(){ColumnName= "tgTCThue"},
                            new DataColumn(){ColumnName= "tgTThue"},
                            new DataColumn(){ColumnName= "tTCKTMai"},
                            new DataColumn(){ColumnName= "tgTTTBSo"},
                            new DataColumn(){ColumnName= "tgTTTBChu"},
                            new DataColumn(){ColumnName= "dLieu"},
                            new DataColumn(){ColumnName= "tgTTTTruongTBChu"},
                            new DataColumn(){ColumnName= "mCCQT"},
                            new DataColumn(){ColumnName= "tCHDon"},
                            new DataColumn(){ColumnName= "lHDCLQuan"},
                            new DataColumn(){ColumnName= "kHMSHDCLQuan"},
                            new DataColumn(){ColumnName= "kHHDCLQuan"},
                            new DataColumn(){ColumnName= "sHDCLQuan"},
                            new DataColumn(){ColumnName= "nLHDCLQuan"},
                        };

            TTHDon.Columns.AddRange(col.ToArray());
            DataRow dr = TTHDon.NewRow();
            TTHDon.Rows.Add(dr);

            if (MTDiep.Count > 0)
                TTHDon.Rows[0]["mTDiep"] = MTDiep[0].InnerText;
            if (MTDTChieu.Count > 0)
                TTHDon.Rows[0]["mTDTChieu"] = MTDTChieu[0].InnerText;
            if (KHMSHDon.Count > 0)
                TTHDon.Rows[0]["kHMSHDon"] = KHMSHDon[0].InnerText;
            if (KHHDon.Count > 0)
                TTHDon.Rows[0]["kHHDon"] = KHHDon[0].InnerText;
            if (SHDon.Count > 0)
                TTHDon.Rows[0]["sHDon"] = SHDon[0].InnerText;
            if (NLap.Count > 0)
                TTHDon.Rows[0]["nLap"] = NLap[0].InnerText;
            if (DVTTe.Count > 0)
                TTHDon.Rows[0]["dVTTe"] = DVTTe[0].InnerText;
            if (HTTToan.Count > 0)
                TTHDon.Rows[0]["hTTToan"] = HTTToan[0].InnerText;

                TTHDon.Rows[0]["tenNB"] = TenNB;
                TTHDon.Rows[0]["mSTNB"] = MSTNB;
                TTHDon.Rows[0]["dChiNB"] = DChiNB;
                TTHDon.Rows[0]["sDThoaiNB"] = SDThoaiNB;
                TTHDon.Rows[0]["dCTDTuNB"] = DCTDTuNB;
                TTHDon.Rows[0]["sTKNHangNB"] = STKNHangNB;
                TTHDon.Rows[0]["tNHangNB"] = TNHangNB;
                TTHDon.Rows[0]["tenNM"] = TenNM;
                TTHDon.Rows[0]["mSTNM"] = MSTNM;
                TTHDon.Rows[0]["dChiNM"] = DChiNM;
                TTHDon.Rows[0]["sDThoaiNM"] = SDThoaiNM;
                TTHDon.Rows[0]["dCTDTuNM"] = DCTDTuNM;
                TTHDon.Rows[0]["sTKNHangNM"] = STKNHangNM;
                TTHDon.Rows[0]["tNHangNM"] = TNHangNM;
                TTHDon.Rows[0]["hVTNMHang"] = HVTNMHang;
            if (TSuat.Count > 0)
                TTHDon.Rows[0]["tSuat"] = TSuat[0].InnerText;
            if (ThTien.Count > 0)
                TTHDon.Rows[0]["thTien"] = ThTien[0].InnerText;
            if (TThue.Count > 0)
                TTHDon.Rows[0]["tThue"] = TThue[0].InnerText;
            if (TgTCThue.Count > 0)
                TTHDon.Rows[0]["tgTCThue"] = TgTCThue[0].InnerText;
            if (TgTThue.Count > 0)
                TTHDon.Rows[0]["tgTThue"] = TgTThue[0].InnerText;
            if (TTCKTMai.Count > 0)
                TTHDon.Rows[0]["tTCKTMai"] = TTCKTMai[0].InnerText;
            if (TgTTTBSo.Count > 0)
                TTHDon.Rows[0]["tgTTTBSo"] = string.Format(CultureInfo.InvariantCulture,
                                 "{0:#,#0.#######}", TgTTTBSo[0].InnerText);
            if (TgTTTBChu.Count > 0)
                TTHDon.Rows[0]["tgTTTBChu"] = TgTTTBChu[0].InnerText;
            if (DLieu.Count > 0)
                TTHDon.Rows[0]["dLieu"] = DLieu[0].InnerText;
            if (MCCQT.Count > 0)
                TTHDon.Rows[0]["mCCQT"] = MCCQT[0].InnerText;
                //TTHDLQuan
                TTHDon.Rows[0]["tCHDon"] = TCHDon;
                TTHDon.Rows[0]["lHDCLQuan"] = LHDCLQuan;
                TTHDon.Rows[0]["kHMSHDCLQuan"] = KHMSHDCLQuan;
                TTHDon.Rows[0]["kHHDCLQuan"] = KHHDCLQuan;
                TTHDon.Rows[0]["sHDCLQuan"] = SHDCLQuan;
                TTHDon.Rows[0]["nLHDCLQuan"] = NLHDCLQuan;

            ds.Tables.Add(TTHDon);
            //chi tiết hóa đơn
            DataTable dSHH = new DataTable();
            XmlNodeList TChat = xmlDocument.GetElementsByTagName("TChat");
            XmlNodeList STT = xmlDocument.GetElementsByTagName("STT");
            XmlNodeList THHDVu = xmlDocument.GetElementsByTagName("THHDVu");
            XmlNodeList DVTinh = xmlDocument.GetElementsByTagName("DVTinh");
            XmlNodeList SLuong = xmlDocument.GetElementsByTagName("SLuong");
            XmlNodeList DGia = xmlDocument.GetElementsByTagName("DGia");
            XmlNodeList TLCKhauCT = xmlDocument.GetElementsByTagName("TLCKhau");
            XmlNodeList STCKhauCT = xmlDocument.GetElementsByTagName("STCKhau");
            XmlNodeList ThTienCT = xmlDocument.GetElementsByTagName("ThTien");
            XmlNodeList TSuatCT = xmlDocument.GetElementsByTagName("TSuat");

            var col1 = new List<DataColumn>() {
                                            new DataColumn(){ColumnName= "tChat"},
                                            new DataColumn(){ColumnName = "sTT"},
                                            new DataColumn(){ColumnName = "tHHDVu"},
                                            new DataColumn(){ColumnName = "dVTinh"},
                                            new DataColumn(){ColumnName = "sLuong"},
                                            new DataColumn(){ColumnName = "dGia"},
                                            new DataColumn(){ColumnName = "tLCKhauCT"},
                                            new DataColumn(){ColumnName = "sTCKhauCT"},
                                            new DataColumn(){ColumnName = "thTienCT"},
                                            new DataColumn(){ColumnName = "tSuatCT"}
                                        };

            dSHH.Columns.AddRange(col1.ToArray());
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("HHDVu");
            for (int i = 0; i < nodeList.Count; i++)
            {
                DataRow dr1 = dSHH.NewRow();
                dSHH.Rows.Add(dr1);
                if (TChat.Count > 0)
                    dSHH.Rows[i]["tChat"] = TChat[i].InnerText;
                if (STT.Count > 0 && STT[i] != null)
                    dSHH.Rows[i]["sTT"] = STT[i].InnerText;
                if (THHDVu.Count > 0)
                    dSHH.Rows[i]["tHHDVu"] = THHDVu[i].InnerText;
                if (DVTinh.Count > 0 && DVTinh[i] != null)
                    dSHH.Rows[i]["dVTinh"] = DVTinh[i].InnerText;
                if (SLuong.Count > 0 && SLuong[i] != null)
                    dSHH.Rows[i]["sLuong"] = SLuong[i].InnerText;
                if (DGia.Count > 0 && DGia[i] != null)
                    dSHH.Rows[i]["dGia"] = DGia[i].InnerText;
                if (TLCKhauCT.Count > 0 && TLCKhauCT[i] != null)
                    dSHH.Rows[i]["tLCKhauCT"] = TLCKhauCT[i].InnerText;
                if (STCKhauCT.Count > 0 && STCKhauCT[i] != null)
                    dSHH.Rows[i]["sTCKhauCT"] = STCKhauCT[i].InnerText;
                if (ThTienCT.Count > 0 && ThTienCT[i] != null)
                    dSHH.Rows[i]["thTienCT"] = ThTienCT[i].InnerText;
                if (TSuatCT.Count > 0 && TSuatCT[i] != null)
                    dSHH.Rows[i]["tSuatCT"] = TSuatCT[i].InnerText;
            }
            ds.Tables.Add(dSHH);

            #region danh sách chữ kỹ số
            DataTable dSCKS = new DataTable();
            var col2 = new List<DataColumn>()
                        {
                            new DataColumn(){ColumnName= "x509SubjectName"},
                            new DataColumn(){ColumnName= "signingTime" }
                        };
            dSCKS.Columns.AddRange(col2.ToArray());
            XmlNodeList listCKS = xmlDocument.GetElementsByTagName("X509SubjectName");
            for (int i = 0; i < listCKS.Count; i++)
            {
                DataRow dr1 = dSCKS.NewRow();
                dSCKS.Rows.Add(dr1);
                if (X509SubjectName.Count > 0)
                {
                    dSCKS.Rows[i]["x509SubjectName"] = X509SubjectName[i].InnerText;
                    dSCKS.Rows[i]["signingTime"] = SigningTime[i].InnerText;
                    string chuky = X509SubjectName[i].InnerText;
                    string[] chuky1 = chuky.Split(',');
                    foreach (var item in chuky1)
                    {
                        var a = item.Contains("CN=");
                        if (a == true)
                            dSCKS.Rows[i]["x509SubjectName"] = item.Replace("CN=", "");
                    }
                }
            }
            ds.Tables.Add(dSCKS);
            #endregion
            return ds;
        }
        private DataSet InTD102(XmlDocument xmlDocument, DataSet ds)
        {
            XmlNodeList So = xmlDocument.GetElementsByTagName("So");
            XmlNodeList NTBao = xmlDocument.GetElementsByTagName("NTBao");
            XmlNodeList MST = xmlDocument.GetElementsByTagName("MST");
            XmlNodeList TNNT = xmlDocument.GetElementsByTagName("TNNT");
            XmlNodeList TGGui = xmlDocument.GetElementsByTagName("TGGui");
            XmlNodeList TTKhai = xmlDocument.GetElementsByTagName("TTKhai");
            XmlNodeList MTDTChieu = xmlDocument.GetElementsByTagName("MTDTChieu");
            XmlNodeList THop = xmlDocument.GetElementsByTagName("THop");
            XmlNodeList MSo = xmlDocument.GetElementsByTagName("MSo");
            XmlNodeList Ten = xmlDocument.GetElementsByTagName("Ten");
            XmlNodeList TGNhan = xmlDocument.GetElementsByTagName("TGNhan");
            XmlNodeList X509SubjectName = xmlDocument.GetElementsByTagName("X509SubjectName");
            XmlNodeList SigningTime = xmlDocument.GetElementsByTagName("SigningTime");
            XmlNodeList DDanh = xmlDocument.GetElementsByTagName("DDanh");

            DataTable TTHDon = new DataTable();
            var col = new List<DataColumn>() {
                      new DataColumn(){ColumnName= "so"},
                      new DataColumn(){ColumnName= "nTBao"},
                      new DataColumn(){ColumnName= "mST"},
                      new DataColumn(){ColumnName= "tNNT"},
                      new DataColumn(){ColumnName= "tGGui"},
                      new DataColumn(){ColumnName= "tTKhai"},
                      new DataColumn(){ColumnName= "mTDTChieu"},
                      new DataColumn(){ColumnName= "tHop"},
                      new DataColumn(){ColumnName= "mSo"},
                      new DataColumn(){ColumnName= "ten"},
                      new DataColumn(){ColumnName= "tGNhan"},
                      new DataColumn(){ColumnName= "x509SubjectName"},
                      new DataColumn(){ColumnName= "signingTime"},
                      new DataColumn(){ColumnName= "dDanh"},
            };

            TTHDon.Columns.AddRange(col.ToArray());
            DataRow dr = TTHDon.NewRow();
            TTHDon.Rows.Add(dr);
            if (So.Count > 0)
                TTHDon.Rows[0]["so"] = So[0].InnerText;
            if (NTBao.Count > 0)
                TTHDon.Rows[0]["nTBao"] = NTBao[0].InnerText;
            if (MST.Count > 0)
                TTHDon.Rows[0]["mST"] = MST[0].InnerText;
            if (TNNT.Count > 0)
                TTHDon.Rows[0]["tNNT"] = TNNT[0].InnerText;
            if (TGGui.Count > 0)
                TTHDon.Rows[0]["tGGui"] = TGGui[0].InnerText;
            if (TTKhai.Count > 0)
                TTHDon.Rows[0]["tTKhai"] = TTKhai[0].InnerText;
            if (MTDTChieu.Count > 0)
                TTHDon.Rows[0]["mTDTChieu"] = MTDTChieu[0].InnerText;
            if (THop.Count > 0)
                TTHDon.Rows[0]["tHop"] = THop[0].InnerText;
            if (MSo.Count > 0)
                TTHDon.Rows[0]["mSo"] = MSo[0].InnerText;
            if (Ten.Count > 0)
                TTHDon.Rows[0]["ten"] = Ten[0].InnerText;
            if (TGNhan.Count > 0)
                TTHDon.Rows[0]["tGNhan"] = TGNhan[0].InnerText;
            if (X509SubjectName.Count > 0)
            {
                string chuky = X509SubjectName[0].InnerText;
                string[] chuky1 = chuky.Split(',');

                foreach (var item in chuky1)
                {
                    var a = item.Contains("CN=");
                    if (a == true)
                    {
                        TTHDon.Rows[0]["x509SubjectName"] = item;
                    }
                }
            }
            if (SigningTime.Count > 0)
                TTHDon.Rows[0]["signingTime"] = SigningTime[0].InnerText;
            if (DDanh.Count > 0)
                TTHDon.Rows[0]["dDanh"] = DDanh[0].InnerText;
            ds.Tables.Add(TTHDon);
            DataTable dSLDo = new DataTable();
            XmlNodeList MLoi = xmlDocument.GetElementsByTagName("MLoi");
            XmlNodeList MTa = xmlDocument.GetElementsByTagName("MTa");

            var col1 = new List<DataColumn>() {
                new DataColumn(){ColumnName= "mloi"},
                new DataColumn(){ColumnName = "mTa"}
                };

            dSLDo.Columns.AddRange(col1.ToArray());
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("MLoi");

            for (int i = 0; i < nodeList.Count; i++)
            {
                DataRow dr1 = dSLDo.NewRow();
                dSLDo.Rows.Add(dr1);
                if (MLoi.Count > 0)
                    dSLDo.Rows[i]["mloi"] = MLoi[i].InnerText;
                if (MTa.Count > 0)
                    dSLDo.Rows[i]["mTa"] = MTa[i].InnerText;
            }
            ds.Tables.Add(dSLDo);
            return ds;
        }
        private DataSet InTD103(XmlDocument xmlDocument, DataSet ds)
        {
            XmlNodeList Ngay = xmlDocument.GetElementsByTagName("Ngay");
            XmlNodeList MSo = xmlDocument.GetElementsByTagName("MSo");
            XmlNodeList MST = xmlDocument.GetElementsByTagName("MST");
            XmlNodeList TNNT = xmlDocument.GetElementsByTagName("TNNT");
            XmlNodeList HTDKy = xmlDocument.GetElementsByTagName("HTDKy");
            XmlNodeList TTXNCQT = xmlDocument.GetElementsByTagName("TTXNCQT");
            XmlNodeList So = xmlDocument.GetElementsByTagName("So");
            XmlNodeList NTBao = xmlDocument.GetElementsByTagName("NTBao");
            XmlNodeList MTDTChieu = xmlDocument.GetElementsByTagName("MTDTChieu");
            XmlNodeList X509SubjectName = xmlDocument.GetElementsByTagName("X509SubjectName");
            XmlNodeList SigningTime = xmlDocument.GetElementsByTagName("SigningTime");
            XmlNodeList TCQTCTren = xmlDocument.GetElementsByTagName("TCQTCTren");
            XmlNodeList TCQT = xmlDocument.GetElementsByTagName("TCQT");
            XmlNodeList DDanh = xmlDocument.GetElementsByTagName("DDanh");

            DataTable TTHDon = new DataTable();
            var col = new List<DataColumn>() {
                      new DataColumn(){ColumnName= "ngay"},
                      new DataColumn(){ColumnName= "mSo"},
                      new DataColumn(){ColumnName= "mST"},
                      new DataColumn(){ColumnName= "tNNT"},
                      new DataColumn(){ColumnName= "hTDKy"},
                      new DataColumn(){ColumnName= "tTXNCQT"},
                      new DataColumn(){ColumnName= "so"},
                      new DataColumn(){ColumnName= "nTBao"},
                      new DataColumn(){ ColumnName= "x509SubjectName"},
                      new DataColumn(){ColumnName= "signingTime"},
                      new DataColumn(){ColumnName= "mTDTChieu"},
                      new DataColumn(){ColumnName= "tCQTCTren"},
                      new DataColumn(){ColumnName= "tCQT"},
                      new DataColumn(){ColumnName= "dDanh"}
            };

            TTHDon.Columns.AddRange(col.ToArray());
            DataRow dr = TTHDon.NewRow();
            TTHDon.Rows.Add(dr);

            if (Ngay.Count > 0)
                TTHDon.Rows[0]["ngay"] = Ngay[0].InnerText;
            if (MSo.Count > 0)
                TTHDon.Rows[0]["mSo"] = MSo[0].InnerText;
            if (MST.Count > 0)
                TTHDon.Rows[0]["mST"] = MST[0].InnerText;
            if (TNNT.Count > 0)
                TTHDon.Rows[0]["tNNT"] = TNNT[0].InnerText;
            if (HTDKy.Count > 0)
                TTHDon.Rows[0]["hTDKy"] = HTDKy[0].InnerText;
            if (TTXNCQT.Count > 0)
                TTHDon.Rows[0]["tTXNCQT"] = TTXNCQT[0].InnerText;
            if (So.Count > 0)
                TTHDon.Rows[0]["so"] = So[0].InnerText;
            if (NTBao.Count > 0)
                TTHDon.Rows[0]["nTBao"] = NTBao[0].InnerText;
            if (X509SubjectName.Count > 0)
            {
                string chuky = X509SubjectName[0].InnerText;
                string[] chuky1 = chuky.Split(',');

                foreach (var item in chuky1)
                {
                    var a = item.Contains("CN=");
                    if (a == true)
                    {
                        TTHDon.Rows[0]["x509SubjectName"] = item;
                    }
                }
            }
            if (SigningTime.Count > 0)
                TTHDon.Rows[0]["signingTime"] = SigningTime[0].InnerText;
            if (MTDTChieu.Count > 0)
                TTHDon.Rows[0]["mTDTChieu"] = MTDTChieu[0].InnerText;
            if (TCQTCTren.Count > 0)
                TTHDon.Rows[0]["tCQTCTren"] = TCQTCTren[0].InnerText;
            if (TCQT.Count > 0)
                TTHDon.Rows[0]["tCQT"] = TCQT[0].InnerText;
            if (DDanh.Count > 0)
                TTHDon.Rows[0]["dDanh"] = DDanh[0].InnerText;
            ds.Tables.Add(TTHDon);

            //chi tiết lý do ko tiếp nhận
            DataTable dSLDo = new DataTable();
            XmlNodeList MLoi = xmlDocument.GetElementsByTagName("MLoi");
            XmlNodeList MTa = xmlDocument.GetElementsByTagName("MTa");
            var col2 = new List<DataColumn>() {
                                            new DataColumn(){ColumnName= "mloi"},
                                            new DataColumn(){ColumnName = "mTa"}
                 };
            dSLDo.Columns.AddRange(col2.ToArray());
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("MLoi");
            for (int i = 0; i < nodeList.Count; i++)
            {
                DataRow dr1 = dSLDo.NewRow();
                dSLDo.Rows.Add(dr1);
                if (MLoi.Count > 0)
                    dSLDo.Rows[i]["mloi"] = MLoi[i].InnerText;
                if (MTa.Count > 0)
                    dSLDo.Rows[i]["mTa"] = MTa[i].InnerText;
            }
            ds.Tables.Add(dSLDo);
            DataTable dSCKS = new DataTable();
            var col1 = new List<DataColumn>() {
                               new DataColumn(){ColumnName= "x509SubjectName"},
                               new DataColumn(){ColumnName= "signingTime"}
            };

            dSCKS.Columns.AddRange(col1.ToArray());
            XmlNodeList nodeListCKS = xmlDocument.GetElementsByTagName("X509SubjectName");
            for (int i = 0; i < nodeListCKS.Count; i++)
            {
                DataRow dr1 = dSCKS.NewRow();
                dSCKS.Rows.Add(dr1);
                if (X509SubjectName.Count > 0)
                {
                    dSCKS.Rows[i]["x509SubjectName"] = X509SubjectName[i].InnerText;
                    dSCKS.Rows[i]["signingTime"] = SigningTime[i].InnerText;
                    string chuky = X509SubjectName[i].InnerText;
                    string[] chuky1 = chuky.Split(',');
                    foreach (var item in chuky1)
                    {
                        var a = item.Contains("CN=");
                        if (a == true)
                        {
                            dSCKS.Rows[i]["x509SubjectName"] = item.Replace("CN=", "");
                        }
                    }
                }
            }
            return ds;
        }
        private DataSet InTD301(XmlDocument xmlDocument, DataSet ds)
        {
            // Tiếp nhận hoặc ko tiếp nhận mẫu 04SS (Sau khi có 204 ktra dữ liệu đúng)
            XmlNodeList So = xmlDocument.GetElementsByTagName("So");
            XmlNodeList NTBao = xmlDocument.GetElementsByTagName("NTBao");
            XmlNodeList MST = xmlDocument.GetElementsByTagName("MST");
            XmlNodeList TNNT = xmlDocument.GetElementsByTagName("TNNT");
            XmlNodeList TGGui = xmlDocument.GetElementsByTagName("TGGui");
            XmlNodeList TTKhai = xmlDocument.GetElementsByTagName("TTKhai");
            XmlNodeList MTDTChieu = xmlDocument.GetElementsByTagName("MTDTChieu");
            XmlNodeList MSo = xmlDocument.GetElementsByTagName("MSo");
            XmlNodeList Ten = xmlDocument.GetElementsByTagName("Ten");
            XmlNodeList TGNhan = xmlDocument.GetElementsByTagName("TGNhan");
            XmlNodeList X509SubjectName = xmlDocument.GetElementsByTagName("X509SubjectName");
            XmlNodeList SigningTime = xmlDocument.GetElementsByTagName("SigningTime");
            XmlNodeList TCQTCTren = xmlDocument.GetElementsByTagName("TCQTCTren");
            XmlNodeList TCQT = xmlDocument.GetElementsByTagName("TCQT");

            DataTable TTHDon = new DataTable();
            var col = new List<DataColumn>() {
                        new DataColumn(){ColumnName= "so"},
                        new DataColumn(){ColumnName= "nTBao"},
                        new DataColumn(){ColumnName= "mST"},
                        new DataColumn(){ColumnName= "tNNT"},
                        new DataColumn(){ColumnName= "tGGui"},
                        new DataColumn(){ColumnName= "tTKhai"},
                        new DataColumn(){ColumnName= "mTDTChieu"},
                        new DataColumn(){ColumnName= "tHop"},
                        new DataColumn(){ColumnName= "mSo"},
                        new DataColumn(){ColumnName= "ten"},
                        new DataColumn(){ColumnName= "tGNhan"},
                        new DataColumn(){ColumnName= "x509SubjectName"},
                        new DataColumn(){ColumnName= "signingTime"},
                        new DataColumn(){ColumnName = "tCQTCTren"},
                        new DataColumn(){ColumnName = "tCQT"}
            };

            TTHDon.Columns.AddRange(col.ToArray());
            DataRow dr = TTHDon.NewRow();
            TTHDon.Rows.Add(dr);
            if (So.Count > 0)
                TTHDon.Rows[0]["so"] = So[0].InnerText;
            if (NTBao.Count > 0)
                TTHDon.Rows[0]["nTBao"] = NTBao[0].InnerText;
            if (MST.Count > 0)
                TTHDon.Rows[0]["mST"] = MST[0].InnerText;
            if (TNNT.Count > 0)
                TTHDon.Rows[0]["tNNT"] = TNNT[0].InnerText;
            if (TGGui.Count > 0)
                TTHDon.Rows[0]["tGGui"] = TGGui[0].InnerText;
            if (TTKhai.Count > 0)
                TTHDon.Rows[0]["tTKhai"] = TTKhai[0].InnerText;
            if (MTDTChieu.Count > 0)
                TTHDon.Rows[0]["mTDTChieu"] = MTDTChieu[0].InnerText;
            if (MSo.Count > 0)
                TTHDon.Rows[0]["mSo"] = MSo[0].InnerText;
            if (Ten.Count > 0)
                TTHDon.Rows[0]["ten"] = Ten[0].InnerText;
            if (TGNhan.Count > 0)
                TTHDon.Rows[0]["tGNhan"] = TGNhan[0].InnerText;
            if (TCQTCTren.Count > 0)
                TTHDon.Rows[0]["tCQTCTren"] = TCQTCTren[0].InnerText;
            if (TCQT.Count > 0)
                TTHDon.Rows[0]["tCQT"] = TCQT[0].InnerText;
            ds.Tables.Add(TTHDon);

            //chi tiết lý do ko tiếp nhận
            DataTable dSLDo = new DataTable();
            XmlNodeList STT = xmlDocument.GetElementsByTagName("STT");
            XmlNodeList MCQTCap = xmlDocument.GetElementsByTagName("MCQTCap");
            XmlNodeList KHMSHDon = xmlDocument.GetElementsByTagName("KHMSHDon");
            XmlNodeList KHHDon = xmlDocument.GetElementsByTagName("KHHDon");
            XmlNodeList SHDon = xmlDocument.GetElementsByTagName("SHDon");
            XmlNodeList NLap = xmlDocument.GetElementsByTagName("NLap");
            XmlNodeList LADHDDT = xmlDocument.GetElementsByTagName("LADHDDT");
            XmlNodeList TCTBao = xmlDocument.GetElementsByTagName("TCTBao");
            XmlNodeList TTTNCCQT = xmlDocument.GetElementsByTagName("TTTNCCQT");
            XmlNodeList MLoi = xmlDocument.GetElementsByTagName("MLoi");
            XmlNodeList MTa = xmlDocument.GetElementsByTagName("MTa");


            var col2 = new List<DataColumn>() {
                        new DataColumn(){ColumnName= "sTT"},
                        new DataColumn(){ColumnName= "mCQTCap"},
                        new DataColumn(){ColumnName= "kHMSHDon"},
                        new DataColumn(){ColumnName= "kHHDon"},
                        new DataColumn(){ColumnName= "sHDon"},
                        new DataColumn(){ColumnName= "nLap"},
                        new DataColumn(){ColumnName= "lADHDDT"},
                        new DataColumn(){ColumnName= "tCTBao"},
                        new DataColumn(){ColumnName= "tTTNCCQT"},
                        new DataColumn(){ColumnName= "mloi"},
                        new DataColumn(){ColumnName = "mTa"}
            };


            dSLDo.Columns.AddRange(col2.ToArray());
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("MLoi");
            for (int i = 0; i < nodeList.Count; i++)
            {
                DataRow dr1 = dSLDo.NewRow();
                dSLDo.Rows.Add(dr1);
                if (STT.Count > 0)
                    dSLDo.Rows[i]["sTT"] = STT[i].InnerText;
                if (MCQTCap.Count > 0)
                    dSLDo.Rows[i]["mCQTCap"] = MCQTCap[i].InnerText;
                if (KHMSHDon.Count > 0)
                    dSLDo.Rows[i]["kHMSHDon"] = KHMSHDon[i].InnerText;
                if (KHHDon.Count > 0)
                    dSLDo.Rows[i]["kHHDon"] = KHHDon[i].InnerText;
                if (SHDon.Count > 0)
                    dSLDo.Rows[i]["sHDon"] = SHDon[i].InnerText;
                if (NLap.Count > 0)
                    dSLDo.Rows[i]["nLap"] = NLap[i].InnerText;
                if (LADHDDT.Count > 0)
                    dSLDo.Rows[i]["lADHDDT"] = LADHDDT[i].InnerText;
                if (TCTBao.Count > 0)
                    dSLDo.Rows[i]["tCTBao"] = TCTBao[i].InnerText;
                if (TTTNCCQT.Count > 0)
                    dSLDo.Rows[i]["tTTNCCQT"] = TTTNCCQT[i].InnerText;
                if (MLoi.Count > 0)
                    dSLDo.Rows[i]["mloi"] = MLoi[i].InnerText;
                if (MTa.Count > 0)
                    dSLDo.Rows[i]["mTa"] = MTa[i].InnerText;
            }
            ds.Tables.Add(dSLDo);

            DataTable dSCKS = new DataTable();
            var col1 = new List<DataColumn>() {
                    new DataColumn(){ColumnName= "x509SubjectName"},
                    new DataColumn(){ColumnName= "signingTime"}
                };
            dSCKS.Columns.AddRange(col1.ToArray());
            XmlNodeList nodeListCKS = xmlDocument.GetElementsByTagName("X509SubjectName");
            for (int i = 0; i < nodeListCKS.Count; i++)
            {
                DataRow dr1 = dSCKS.NewRow();
                dSCKS.Rows.Add(dr1);
                if (X509SubjectName.Count > 0)
                {
                    dSCKS.Rows[i]["x509SubjectName"] = X509SubjectName[i].InnerText;
                    dSCKS.Rows[i]["signingTime"] = SigningTime[i].InnerText;
                    string chuky = X509SubjectName[i].InnerText;
                    string[] chuky1 = chuky.Split(',');

                    foreach (var item in chuky1)
                    {
                        var a = item.Contains("CN=");
                        if (a == true)
                        {
                            dSCKS.Rows[i]["x509SubjectName"] = item.Replace("CN=", "");
                        }
                    }
                }
            }
            ds.Tables.Add(dSCKS);
            return ds;
        }
        #endregion

    }
}