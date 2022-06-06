using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace InvoiceApi.Util
{
    public class ExceptionUtility
    {
        private ExceptionUtility()
        { }
        public static void LogException(Exception exc, string source)
        {
            string logFile = "~/App_Data/" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            logFile = HttpContext.Current.Server.MapPath(logFile);
            if (!System.IO.File.Exists(logFile))
                System.IO.File.Create(logFile).Close();
            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine("********** {0} **********", DateTime.Now);
            if (exc.InnerException != null)
            {
                sw.Write("Inner Exception Type: ");
                sw.WriteLine(exc.InnerException.GetType().ToString());
                sw.Write("Inner Exception: ");
                sw.WriteLine(exc.InnerException.Message);
                sw.Write("Inner Source: ");
                sw.WriteLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    sw.WriteLine("Inner Stack Trace: ");
                    sw.WriteLine(exc.InnerException.StackTrace);
                }
            }
            sw.Write("Exception Type: ");
            sw.WriteLine(exc.GetType().ToString());
            sw.WriteLine("Exception: " + exc.Message);
            sw.WriteLine("Source: " + source);
            sw.WriteLine("Stack Trace: ");
            if (exc.StackTrace != null)
            {
                sw.WriteLine(exc.StackTrace);
                sw.WriteLine();
            }
            sw.Close();
        }
        public static void LogInfo(string msg)
        {
            string logFile = "~/App_Data/" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(logFile)))
                System.IO.File.Create(logFile).Close();
            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine("** {0} **", msg);
            sw.Close();
        }
        public static void NotifySystemOps(Exception exc)
        {
        }
        public static void LogInfo_CreateInvoice(string msg)
        {
            // Include enterprise logic for logging exceptions 
            // Get the absolute path to the log file 
            string strFileName = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_ErrorLog.txt";
            string logFile = "~/App_Data/" + strFileName;
            logFile = HttpContext.Current.Server.MapPath(logFile);
            if (!File.Exists(logFile))
            {
                using (FileStream fs = File.Create(logFile)) { }
            }
            // Open the log file for append and write the log
            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine(msg);
            sw.Close();
        }
    }
}