using InvoiceApi.Data.Domain;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceApi.IService
{
    public partial interface IAccountService
    {
        JObject Login(string mst, string username, string password, string ipAddress, string userAgent);
        string GenerateToken(string username, string password, string ip, string userAgent, long ticks, string masothue);
        #region
        JObject ChangePass(string username, string password);
        bool CheckPassword(string username, string password);
        #endregion
        inv_user GetAccountByUserName(string loginName, string mstLogin);
    }
}