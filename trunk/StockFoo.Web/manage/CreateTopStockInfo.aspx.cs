using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Text;

namespace StockFoo.Web.manage
{
    public partial class CreateTopStockInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CreateHTML(2))
            {
                Response.Write("<font color=green><b>生成成功！</b></font>&nbsp;&nbsp;&nbsp;<a href=\"../html/TopStockInfo.html\"><font size=2>查看</font></a>&nbsp;&nbsp;&nbsp;<a href=\"CatchIndex.aspx\"><font size=2>返回</font></a>");
            }
            else
            {
                Response.Write("<font color=red><b>生成失败！</b></font>&nbsp;&nbsp;&nbsp;<a href=\"CatchIndex.aspx\"><font size=2>返回</font></a>");
            }
        }

        private bool CreateHTML(int classid)
        {
            bool result = false;
            try
            {
                
                string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                string sql = " SELECT top 50 title,url FROM sf_Article WHERE id in(select article_id from sf_Map_ArtCls where class_id=" + classid + ") order by publish_time desc";
                StringBuilder TopStockInfo = new StringBuilder();
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        TopStockInfo.Append("<a href=\"" + sdr["url"].ToString() + "\" target=\"_blank\">" + sdr["title"].ToString() + "</a><div style=\"margin-top:5px;\"></div>");
                    }
                }

                StreamReader sr = new StreamReader(System.Configuration.ConfigurationSettings.AppSettings["CreateHtmlPath"].ToString() + "TopStockInfoTemplate.html");
                string temp = sr.ReadToEnd().Replace("[@TopStockInfo]", TopStockInfo.ToString());
                sr.Close();

                if (!System.IO.File.Exists(System.Configuration.ConfigurationSettings.AppSettings["CreateHtmlPath"].ToString() + "TopStockInfo.html"))
                {
                    System.IO.File.Create(System.Configuration.ConfigurationSettings.AppSettings["CreateHtmlPath"].ToString() + "TopStockInfo.html");
                }
                StreamWriter sw = new StreamWriter(System.Configuration.ConfigurationSettings.AppSettings["CreateHtmlPath"].ToString() + "TopStockInfo.html");
                sw.Write(temp);
                sw.Close();
                result=true;
            }
            catch
            {
                result=false;
            }
            return result;
        }
    }
}
