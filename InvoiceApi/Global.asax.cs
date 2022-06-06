using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using MinvoiceLib.IServices;
using System.Configuration;
using PkiTokenManager;
using MinvoiceLib.Util;
using System.Globalization;
using System.Web.Http.Dispatcher;
using System.IO;
using MinvoiceLib;
using MinvoiceLib.Services;
using MinvoiceLib.Data;
using MinvoiceLib.Services68;
using MinvoiceLib.IServices68;
using InvoiceApi.Services;
using InvoiceApi.IService;

namespace InvoiceApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        //private IRabbitMQService _rabbitmqService;
        //public class CustomAssemblyResolver : IAssembliesResolver
        //{
        //    public ICollection<Assembly> GetAssemblies()
        //    {
        //        List<Assembly> baseAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        //        //string originalString = AppDomain.CurrentDomain.BaseDirectory + "//Plugin//InvoiceApi.dll";

        //        //foreach (string file in Directory.EnumerateFiles(originalString))
        //        //{
        //        //    baseAssemblies.Add(Assembly.LoadFrom(file));
        //        //}
        //        //var controllersAssembly = Assembly.LoadFrom(originalString);
        //        var controllersAssembly = Assembly.LoadFrom(@"C:InvoiceApi.dll");
        //        baseAssemblies.Add(controllersAssembly);
        //        return baseAssemblies;
        //    }
        //}

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //GlobalConfiguration.Configuration.Services.Replace(typeof(IAssembliesResolver), new CustomAssemblyResolver());

            RegisterAutofacApi();
        }
        public static String GetIP()
        {
            String ip =
                HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
                HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Authorization, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
            //var newCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            //newCulture.NumberFormat.NumberDecimalSeparator = ",";
            //newCulture.NumberFormat.NumberGroupSeparator = ".";

            //System.Threading.Thread.CurrentThread.CurrentCulture = newCulture;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = newCulture;
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {

        }
        private void RegisterAutofacApi()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("nop_cache_static").SingleInstance();
            builder.Register<IWebHelper>(c => new WebHelper(new HttpContextWrapper(HttpContext.Current) as HttpContextBase)).InstancePerLifetimeScope();
            builder.RegisterType<NopDbContext>().As<INopDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<NopDbContext2>().As<INopDbContext2>().InstancePerLifetimeScope();
            builder.RegisterType<SearchService>().As<ISearchService>().InstancePerLifetimeScope();
            builder.RegisterType<SearchMultiInvoiceService>().As<ISearchMultiInvoiceService>().InstancePerLifetimeScope();
            builder.RegisterType<Services.AccountService>().As<IService.IAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<SearchInvoiceService>().As<ISearchInvoiceService>().InstancePerLifetimeScope();
            builder.RegisterType<TestService>().As<ITestService>().InstancePerLifetimeScope();



            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }
    }
}
