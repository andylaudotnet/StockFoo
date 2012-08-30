using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace StockFoo.Web
{
    public partial class _default : System.Web.UI.Page
    {
        protected string loginString = "<a href=\"user/Login.aspx\" style=\"text-decoration:none;color:#666699;\">登录</a>";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string managestr = "";
                    if (Session["username"] != null && Session["username"].ToString().Length > 0)
                    {
                        if (GetUserLevel(Session["username"].ToString().Trim()) == 1)
                        {
                            managestr = "&nbsp;&nbsp;<a href='manage/CatchIndex.aspx'><font color=blue size=2>网站管理</font></a>";
                        }
                        loginString = "<font size=2  color=#666699>欢迎您！<b>" + Session["username"].ToString() + "</b></font>&nbsp;&nbsp;<a href='user/Default.aspx'><font color=blue size=2>用户中心</font></a>" + managestr + "&nbsp;&nbsp;<a href='default.aspx?login=exit'><font size=2 color=red>退出</font></a>";
                    }
                    else
                    {
                        HttpCookie szUserName = Request.Cookies["stockfoousername"];
                        if (szUserName != null && szUserName.Value.Length > 0)
                        {
                            if (GetUserLevel(szUserName.Value.Trim()) == 1)
                            {
                                managestr = "&nbsp;&nbsp;<a href='manage/CatchIndex.aspx'><font color=blue size=2>网站管理</font></a>";
                            }
                            loginString = "<font size=2  color=#666699>欢迎您！<b>" + szUserName.Value + "</b></font>&nbsp;&nbsp;<a href='user/Default.aspx'><font color=blue size=2>用户中心</font></a>" + managestr + "&nbsp;&nbsp;<a href='default.aspx?login=exit'><font size=2 color=red>退出</font></a>";
                            Session["username"] = szUserName.Value;
                        }
                    }
                    if (Request["login"] != null)
                    {
                        Session["username"] = null;
                        Session.Remove("username");
                        HttpCookie hc = Response.Cookies["stockfoousername"];
                        hc = null;
                        Response.Redirect("default.aspx");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }

        private int GetUserLevel(string username)
        {
            int UserLevel = 2;
            string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            string sql = "select  UserLevel from sf_User where username='" + username + "'";
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                    UserLevel = Convert.ToInt32(sdr["UserLevel"]);
            }
            return UserLevel;
        }
    }
}
