using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace StockFoo.Web.manage
{
    public partial class CatchIndex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/user/Login.aspx?UrlReferrer=../manage/CatchIndex.aspx");
                }
                else
                {
                    if (GetUserLevel(Session["username"].ToString().Trim()) != 1)
                    {
                        Response.Redirect("~/user/Default.aspx");
                    }
                }
                gvCatchList.DataSource = GetCatchList();
                gvCatchList.DataBind();
            }
        }

        private DataTable GetCatchList()
        {
            string sql = "select [id],[catch_name],[site_name],(select class_name from sf_Class where id=[classid]) as classname,(case site_encode  when 936 then N'gb2312' when 950 then N'big5' else N'UTF-8' end) as site_encode,(case [enabled] when 1 then N'已启用' else N'已停用' end) as enabled from [sf_Catch]";
            DataTable dt = null;
            string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
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

        protected void gvCatchList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes.Add("style", "background-image:url('../images/menu.jpg')");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "e=this.style.backgroundColor; this.style.backgroundColor='#edf8f8'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=e");

            string statusstr = e.Row.Cells[5].Text;
                if (statusstr == "已启用")
                {
                    e.Row.Cells[5].Text = "<font color=green>已启用</font>";
                }
                else
                {
                    e.Row.Cells[5].Text = "<font color=red>已停用</font>";
                    e.Row.Cells[6].Text = "";
                }
            }
        }
    }
}
