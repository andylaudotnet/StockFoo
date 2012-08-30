using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace StockFoo.Web.user
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["UrlReferrer"] != null)
                    hiddenPreUrl.Value = Request["UrlReferrer"].ToString();
                else
                    hiddenPreUrl.Value = "~/default.aspx";
                txtusername.Focus();
            }
        }
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string name = txtusername.Value;
            string pwd = txtpassword.Value;
            string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            string sql = "select count(ID)  from sf_User where username=@username and password=@password";
            int count = 0;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@username", name);
                cmd.Parameters.AddWithValue("@password", pwd);
                count = (int)cmd.ExecuteScalar();
            }
            if (count > 0)
            {
                LogUserInfo(name);
                Session["username"] = name;
                if (ckbpwdremember.Checked)
                {
                    HttpCookie hc = new HttpCookie("stockfoousername", name);
                    hc.Expires=DateTime.Now.AddDays(30);
                    Response.Cookies.Add(hc);
                }
                Response.Redirect(hiddenPreUrl.Value);
            }
            else
            {
                Page.RegisterStartupScript("aa", "<script>alert('输入的用户名或密码错误，请核对后重新登录！');</script>");
            }
        }
        private void LogUserInfo(string username)
        {
            string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            string sql = "update sf_User set LoginCount=LoginCount+1,modifytime=getdate(),IpAddress='" + Request.ServerVariables.Get("Remote_Addr").ToString() + "' where username='" + username + "'";
            int value = 0;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                value = cmd.ExecuteNonQuery();
            }
        }
    }
}
