using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using MinvoiceLib.Util;

namespace InvoiceApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
