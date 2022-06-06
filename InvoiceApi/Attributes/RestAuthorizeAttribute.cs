using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using InvoiceApi.Controllers;
using MinvoiceLib.Util;
using System.IO;
using InvoiceApi.IService;

namespace InvoiceApi.Attributes
{
    public class RestAuthorizeAttribute : AuthorizeAttribute
    {
        private const string _securityToken = "token";

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (Authorize(filterContext))
            {
                return;
            }

            HandleUnauthorizedRequest(filterContext);
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }

        private bool Authorize(HttpActionContext actionContext)
        {
            try
            {
                HttpRequestMessage requestMessage = actionContext.Request;
                
                string ipAddress = CommonManager.GetClientIpAddress(requestMessage);
                string userAgent = requestMessage.Headers.UserAgent.ToString();
               
                if (SkipAuthorization(actionContext))
                {
                    return true;
                }
                string token = requestMessage.Headers.Authorization.Parameter;
                string mst = requestMessage.RequestUri.Host.Split('.')[0];
                string path = HttpContext.Current.Server.MapPath("~/TaxCode/code.txt");
                if (File.Exists(path))
                {
                    string taxcode = File.ReadAllText(path);
                    TokenSpecifi.TokenHard lstToken = TokenSpecifi.lstToken.Where(p => p.token == token).FirstOrDefault<TokenSpecifi.TokenHard>();
                    if (lstToken != null)
                    {
                        if (taxcode.IndexOf(mst) == -1)
                            return false;
                        else
                        {
                            if (actionContext.ControllerContext.Controller is BaseApiController)
                            {
                                BaseApiController baseController = actionContext.ControllerContext.Controller as BaseApiController;
                                baseController.UserName = "ADMINISTRATOR";
                                baseController.Ma_dvcs = "VP";
                                return true;
                            }
                        }
                    }
                }
               
                return IsTokenValid(token, ipAddress, userAgent, actionContext);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                       || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        public bool IsTokenValid(string token, string ip, string userAgent, HttpActionContext actionContext)
        {
            bool result = false;

            try
            {
                string[] tokenParts = token.Split(';');
                string ma_dvcs = string.Empty;
                //if (tokenParts.Length < 2)
                //{
                //    ma_dvcs = "VP";
                //}
                // Base64 decode the string, obtaining the token:username:timeStamp.
                string key = Encoding.UTF8.GetString(Convert.FromBase64String(tokenParts[0]));

                // Split the parts.
                string[] parts = key.Split(new char[] { ':' });
                if (parts.Length == 4)
                {
                    // Get the hash message, username, and timestamp.
                    string hash = parts[0];
                    string username = parts[1];
                    long ticks = long.Parse(parts[2]);
                    DateTime timeStamp = new DateTime(ticks);
                    string masothue = parts[3];
                    // Ensure the timestamp is valid.
                    bool expired = Math.Abs((DateTime.Now - timeStamp).TotalMinutes) > 60;

                    //!expired
                    if (!expired)
                    {
                        // Get the request lifetime scope so you can resolve services.
                        var requestScope = actionContext.Request.GetDependencyScope();
                        var accountService = requestScope.GetService(typeof(IAccountService)) as IAccountService;
                        var account = accountService.GetAccountByUserName(username, masothue);
                        // Hash the message with the key to generate a token.
                        string computedToken = accountService.GenerateToken(username, account.password,ip, userAgent, ticks,masothue);
                        // Compare the computed token with the one supplied and ensure they match.
                        result = (tokenParts[0] == computedToken);

                        if (result)
                        {
                            if (actionContext.ControllerContext.Controller is BaseApiController)
                            {
                                BaseApiController baseController = actionContext.ControllerContext.Controller as BaseApiController;
                                baseController.UserName = username;
                                baseController.MstLog = masothue;
                                //baseController.Ma_dvcs = tokenParts[1];
                            }
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }
    }
}