using InvoiceApi.IService;
using MinvoiceLib.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace InvoiceApi.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accService;
        public AccountController(IAccountService accService)
        {
            _accService = accService;
        }
        [HttpGet]
        [Route("Account/doanpv")]
        [AllowAnonymous]
        public JArray doanpv()
        {
            JArray jarr = new JArray { "ahiihiihi", "123321" };
            return jarr;
        }

        [HttpPost]
        [Route("Account/Login")]
        [AllowAnonymous]
        public JObject Login(JObject model)
        {
            string mst = model["mst"].ToString().Replace("-","");
            string username = model["username"].ToString();
            string password = model["password"].ToString();


            string ipAddress = CommonManager.GetClientIpAddress(this.ActionContext.Request);
            string originalString = this.ActionContext.Request.RequestUri.OriginalString;
            string userAgent = this.ActionContext.Request.Headers.UserAgent.ToString();
            JObject json = _accService.Login(mst, username.ToUpper().Trim(), password, ipAddress, userAgent);

            return json;
        }

        #region đổi mk
        [HttpPost]
        [Route("Account/ChangePass")]
        public JObject ChangePass(JObject model)
        {
            JObject json = new JObject();

            string oldpass = model["oldpass"].ToString();
            string newpass = model["newpass"].ToString();
            string confirmpass = model["confirmpass"].ToString();

            if (string.IsNullOrEmpty(oldpass) || string.IsNullOrEmpty(newpass) || string.IsNullOrEmpty(confirmpass))
            {
                json.Add("error", "Bạn chưa nhập đủ thông tin đổi mật khẩu");
                return json;
            }

            if (newpass.Length < 6)
            {
                json.Add("error", "Mật khẩu phải từ 6 ký tự trở lên");
                return json;
            }

            if (newpass != confirmpass)
            {
                json.Add("error", "Mật khẩu mới không giống nhau");
                return json;
            }

            bool oldPassConfirm = _accService.CheckPassword(this.UserName, oldpass);

            if (oldPassConfirm == false)
            {
                json.Add("error", "Mật khẩu cũ không đúng");
                return json;
            }

            json = _accService.ChangePass(this.UserName, newpass);

            return json;
        }
        #endregion

        [HttpGet]
        [Route("Account/GetAccountByUserName")]
        public object GetAccountByUserName()
        {
            return _accService.GetAccountByUserName(UserName,MstLog);
        }
    }
}
