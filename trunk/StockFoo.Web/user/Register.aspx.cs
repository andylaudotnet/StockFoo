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
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["ResponseUserName"] != null)
            {
                CheckUserNameIsExist(Request["ResponseUserName"].ToString());
            }
            if (Request["ResponseName"] != null)
            {
                CheckNameIsExist(Request["ResponseName"].ToString());
            }
            btnregister1.Attributes.Add("onclick", "return ConfirmElements();");

        }

        protected void btnregister1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Value;
            string password = txtPassword2.Value;
            string userqq = "";
            if (txtQQ.Value != "")
                userqq = txtQQ.Value;
            string remark = txtRemark.Value;
            string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            string sql = @"INSERT INTO sf_User (
UserName,
PassWord,
Remark, 
UserQQ,
RealName,
UserSex,
UserBirthday,
UserMsn,
City ,
Province,
Country, 
UserEmail ,
UserTel ,
UserMobile,
IpAddress ,
Status ,
NetAddress,
Profession ,
channel,
Address ,
Zip ,
Education,
logincount,
UserLevel,
ImageUrl
)
VALUES(
@UserName,
@PassWord,
@Remark, 
@UserQQ,
@RealName,
@UserSex,
@UserBirthday,
@UserMsn,
@City ,
@Province,
@Country, 
@UserEmail ,
@UserTel ,
@UserMobile,
@IpAddress ,
@Status ,
@NetAddress,
@Profession ,
@channel,
@Address ,
@Zip ,
@Education,
@LoginCount,
@UserLevel,
@ImageUrl
)";

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                SqlParameter[] Param = {
                                          new SqlParameter("@UserName",SqlDbType.VarChar,20),
                                          new SqlParameter("@PassWord",SqlDbType.VarChar,20),
                                          new SqlParameter("@Remark",SqlDbType.VarChar,5000),
                                          new SqlParameter("@UserQQ",SqlDbType.VarChar,50),
                                          new SqlParameter("@RealName",SqlDbType.VarChar,20),
                                          new SqlParameter("@UserSex",SqlDbType.VarChar,8),
                                          new SqlParameter("@UserBirthday",SqlDbType.DateTime),
                                          new SqlParameter("@UserMsn",SqlDbType.VarChar,50),
                                          new SqlParameter("@City",SqlDbType.VarChar,50),
                                          new SqlParameter("@Province",SqlDbType.VarChar,50),
                                          new SqlParameter("@Country",SqlDbType.VarChar,50),
                                          new SqlParameter("@UserEmail",SqlDbType.VarChar,50),
                                          new SqlParameter("@UserTel",SqlDbType.VarChar,50),
                                          new SqlParameter("@UserMobile",SqlDbType.VarChar,50),
                                          new SqlParameter("@IpAddress",SqlDbType.VarChar,50),
                                          new SqlParameter("@Status",SqlDbType.Int),
                                          new SqlParameter("@NetAddress",SqlDbType.VarChar,50),
                                          new SqlParameter("@Profession",SqlDbType.VarChar,50),
                                          new SqlParameter("@channel",SqlDbType.VarChar,50),
                                          new SqlParameter("@Address",SqlDbType.VarChar,50),
                                          new SqlParameter("@Zip",SqlDbType.VarChar,50),
                                          new SqlParameter("@Education",SqlDbType.VarChar,50),
                                          new SqlParameter("@LoginCount",SqlDbType.Int),
                                          new SqlParameter("@UserLevel",SqlDbType.Int),
                                          new SqlParameter("@ImageUrl",SqlDbType.VarChar,100),
                                   };
                int i = 0;
                string QQ = txtQQ.Value;
                if (QQ == "")
                    QQ = "00000000";
                string birth = txtUserBirthday.Value;
                if (birth == "")
                    birth = "1900-01-01";
                Param[i++].Value = txtUsername.Value;
                Param[i++].Value = txtPassword2.Value;
                Param[i++].Value = txtRemark.Value;
                Param[i++].Value = QQ;
                Param[i++].Value = txtRealName.Value;
                Param[i++].Value = sltUserSex.Items[sltUserSex.SelectedIndex].Text;
                Param[i++].Value = birth;
                Param[i++].Value = txtUserMsn.Value;
                Param[i++].Value = sltCity.Items[sltCity.SelectedIndex].Text;
                Param[i++].Value = sltProvince.Items[sltProvince.SelectedIndex].Text;
                Param[i++].Value = sltCountry.Items[sltCountry.SelectedIndex].Text;
                Param[i++].Value = txtUserEmail.Value;
                Param[i++].Value = txtUserTel.Value;
                Param[i++].Value = txtUserMobile.Value;
                Param[i++].Value = Request.ServerVariables.Get("Remote_Addr").ToString().Trim();
                Param[i++].Value = 0;
                Param[i++].Value = sltNetAddress.Items[sltNetAddress.SelectedIndex].Text;
                Param[i++].Value = sltProfession.Items[sltProfession.SelectedIndex].Text;
                Param[i++].Value = sltchannel.Items[sltchannel.SelectedIndex].Text;
                Param[i++].Value = txtAddress.Value;
                Param[i++].Value = txtZip.Value;
                Param[i++].Value = sltEducation.Items[sltEducation.SelectedIndex].Text;
                Param[i++].Value = 1;
                Param[i++].Value = 2;
                Param[i++].Value = "img/noupload.jpg";
                cmd.Parameters.Clear();

                if (Param != null)
                {
                    foreach (SqlParameter dbParam in Param)
                    {
                        if (dbParam.Value != null && dbParam.Value != System.DBNull.Value)
                        {
                            dbParam.Value = ConvertParam(dbParam.Value.ToString());
                        }
                        cmd.Parameters.Add(dbParam);
                    }
                }
                int ai = cmd.ExecuteNonQuery();
            }
            Session["username"] = username;
            HttpCookie hc = new HttpCookie("stockfoousername", Server.HtmlEncode(username));
            Response.Cookies.Add(hc);

            Page.RegisterStartupScript("gg", "<script>alert('注册成功！\\n\\n请熟记以下信息：\\n用户名：" + txtUsername.Value + "\\n密  码：" + txtPassword2.Value + "');window.location.href='Default.aspx';</script>");
        }

        public string ConvertParam(string sParam)
        {
            return sParam.Replace("'", "''").Replace("singlechar", "'");
        }

        private void SetUserMessageCount(string requestname)
        {
            string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            string sql = "update sf_User set MessageMaxcount=MessageMaxcount+1 where username='" + requestname + "'";
            int count = 0;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                count = (int)cmd.ExecuteNonQuery();
            }
        }

        private void CheckUserNameIsExist(string username)
        {
            string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            string sql = "select count(ID)  from sf_User where username='" + username + "'";
            int count = 0;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                count = (int)cmd.ExecuteScalar();
            }
            string result = "no";
            if (count > 0)
            {
                result = "exsit";
            }
            Response.Write(result);
            Response.Flush();
            Response.End();
        }

        private void CheckNameIsExist(string name)
        {
            string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            string sql = "select count(ID)  from sf_User where Realname='" + Server.HtmlDecode(name.Trim()) + "'";
            int count = 0;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                count = (int)cmd.ExecuteScalar();
            }
            string result = "no";
            if (count > 0)
            {
                result = "exsit";
            }
            Response.Write(result);
            Response.Flush();
            Response.End();
        }
    }
}
