using InvoiceApi.Data.Domain;
using InvoiceApi.IService;
using InvoiceApi.Util;
using MinvoiceLib.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using VACOMLIB;

namespace InvoiceApi.Services
{
    public class AccountService : IAccountService
    {
        private INopDbContext2 _nopDbContext;
        private TracuuHDDTContext _tracuu;

        public AccountService(INopDbContext2 nopDbContext)
        {
            _nopDbContext = nopDbContext;
        }

        public JObject Login(string mst,string username, string password, string ipAddress, string userAgent)
        {
            JObject _res = new JObject();

            //if (password.Length < 6)
            //{
            //    _res.Add("Error", "Mật khẩu phải từ 6 ký tự trở lên");
            //    return _res;
            //}
            //Data.InvoiceDbContext invoiceDb = _nopDbContext.GetInvoiceDb();
            inv_user user = new inv_user() { mst = mst.Replace("-",""), username = username, password = password };
            //TracuuHDDTContext tracuu = new TracuuHDDTContext();
            TracuuHDDTContext tracuu = conn.getdb();
            user = tracuu.inv_user.FirstOrDefault(x => x.mst.Trim().Replace("-", "").Equals(user.mst.Trim().Replace("-", "")));
            if (user == null)
            {
                _res.Add("Error", "Mã số thuế không tồn tại trong hệ thống ! ");
                return _res;
            }
            user = tracuu.inv_user.FirstOrDefault(c => c.mst.Trim().Replace("-", "") == user.mst.Trim().Replace("-", "") && c.username.Trim().Replace("-", "") == username.Trim().Replace("-", ""));
            if (user == null)
            {
                _res.Add("Error", "Tài khoản không tồn tại trong hệ thống ! ");
                return _res;
            }
            var checkTraCuu = tracuu.inv_customer_banneds.FirstOrDefault(x =>
                    x.mst.Replace("-", "").Equals(mst.Replace("-", "")) && x.type.Equals("KHOATRACUU") && x.is_unblock == false);
            if (checkTraCuu != null && !string.IsNullOrEmpty(checkTraCuu.mst))
            {
               _res.Add("ErrorLogin", "Quý khách đang bị khóa tra cứu. Vui lòng liên hệ admin để giải quyết !");
                return _res;
            }
            else
            {
                PassCommand crypt = new PassCommand(user.password);
                if (crypt.CheckPassword(password))
                {
                    string text = GenerateToken(username, user.password, ipAddress, userAgent, DateTime.Now.Ticks, mst);
                    _res.Add("token", text);
                    user.password= null;
                    _res.Add("user", JObject.Parse(JsonConvert.SerializeObject(user)));
                }
                else
                {
                     _res.Add("Error", "Sai tài khoản hoặc mật khẩu ! ");
                    return _res;
                }
            }
            return _res;
        }

        public string GenerateToken(string username, string password, string ip, string userAgent, long ticks, string masothue)
        {
            string s = string.Join(":", username, userAgent, ticks.ToString(), masothue);
            string text = "";
            string text2 = "";
            using (HMAC hMAC = HMAC.Create("HmacSHA256"))
            {
                hMAC.Key = Encoding.UTF8.GetBytes(GetHashedPassword(password));
                hMAC.ComputeHash(Encoding.UTF8.GetBytes(s));
                text = Convert.ToBase64String(hMAC.Hash);
                text2 = string.Join(":", username, ticks.ToString(),masothue);
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", text, text2)));
        }

        private string GetHashedPassword(string password)
        {
            string s = string.Join(":", password, "rz8LuOtFBXphj9WQfvFh");
            HMAC hMAC = HMAC.Create("HmacSHA256");
            hMAC.Key = Encoding.UTF8.GetBytes("rz8LuOtFBXphj9WQfvFh");
            hMAC.ComputeHash(Encoding.UTF8.GetBytes(s));
            return Convert.ToBase64String(hMAC.Hash);
        }
        #region đổi mk
        public JObject ChangePass(string username, string password)
        {
            JObject _res = new JObject();
            Util.ErrorCodeJObject err = new Util.ErrorCodeJObject();
            try
            {
                inv_user user = new inv_user() { username = username };
                TracuuHDDTContext tracuu = conn.getdb();
                user = tracuu.inv_user.FirstOrDefault(c => c.username.Trim().Replace("-", "") == username.Trim().Replace("-", ""));
                PassCommand passCommand = new PassCommand(password);
                user.password = passCommand.CreateHashedPassword(password, null);
                tracuu.Entry(user).State = EntityState.Modified;
                tracuu.SaveChanges();
                _res.Add("ok", true);
            }
            catch (Exception ex)
            {
                _res.Add("error", ex.Message);
            }
            return _res;
        }
        public bool CheckPassword(string username, string password)
        {
            inv_user user = new inv_user() { username = username };
            TracuuHDDTContext tracuu = conn.getdb();
            user = tracuu.inv_user.FirstOrDefault(c => c.username.Trim().Replace("-", "") == username.Trim().Replace("-", ""));
            if (user == null)
            {
                return false;
            }
            Encrypt encrypt = new Encrypt(user.password);
            if (encrypt.CheckPassword(password))
            {
                return true;
            }
            return false;
        }
        #endregion

        public inv_user GetAccountByUserName(string loginName,string mstLogin)
        {

            TracuuHDDTContext tracuu = conn.getdb();
            return tracuu.inv_user.FirstOrDefault(c => c.username.Trim().Replace("-", "") == loginName.Trim().Replace("-", "") && c.mst.Trim().Replace("-","")== mstLogin);
            //user = tracuu.inv_user.FirstOrDefault(c => c.mst.Trim().Replace("-", "") == user.mst.Trim().Replace("-", "") && c.username.Trim().Replace("-", "") == username.Trim().Replace("-", ""));


            //return _nopDbContext.GetInvoiceDb().WbUsers.Where(c => c.username == loginName).FirstOrDefault<wb_user>();
        }
    }
}