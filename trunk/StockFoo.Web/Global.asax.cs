using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using StockFoo.Catch;
using System.IO;
using System.Text;

namespace StockFoo.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            System.Timers.Timer timer = new System.Timers.Timer(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["codingTime"].ToString()));
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "抓取日志：");
            try
            {
                WebCatch wc = new WebCatch(0);
                wc.GetArticleByCatch();
                sb.Append("完成抓取数据" + wc.totalcatchartend.ToString() + "条，完成入库数据" + wc.totalinsertdbend.ToString() + "条，完成更新数据" + wc.totalupdatedbend.ToString() + "条。");
            }
            catch (Exception ex)
            {
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "出现抓取异常：" + ex.Message);
            }
            
            if (!System.IO.File.Exists(System.Configuration.ConfigurationSettings.AppSettings["LogPath"].ToString() + "catchLog.txt"))
            {
                System.IO.File.Create(System.Configuration.ConfigurationSettings.AppSettings["LogPath"].ToString() + "catchLog.txt");
            }
            string logfile = System.Configuration.ConfigurationSettings.AppSettings["LogPath"].ToString() + "catchLog.txt";
            using (StreamWriter SW = File.AppendText(logfile))
            {
                SW.WriteLine(sb.ToString());
                SW.Close();
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}