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
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["ResponseName"] != null)
            {
                CheckNameIsExist(Request["ResponseName"].ToString(), Request["ResponseRealName"].ToString());
            }
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                    Response.Redirect("Login.aspx?UrlReferrer=Default.aspx");
                else
                {
                    LoadUserInfo();
                    btnregister1.Attributes.Add("onclick", "return ConfirmElements();");
                }
            }
        }

        private void LoadUserInfo()
        {
            string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            string username = "";
            if (Session["username"] != null)
                username = Session["username"].ToString();
            string sql = "select * from sf_User where username='" + username + "'";
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                txtUsername.InnerText = ds.Tables[0].Rows[0]["username"].ToString();
                imgUserImg.ImageUrl = ds.Tables[0].Rows[0]["ImageUrl"].ToString();
                if (ds.Tables[0].Rows[0]["ImageUrl"].ToString().Trim() == "")
                    imgUserImg.ImageUrl = "userimg/noupload.jpg";
                txtRealName.Value = ds.Tables[0].Rows[0]["realname"].ToString();
                hiddenRealName.Value = ds.Tables[0].Rows[0]["realname"].ToString();
                sltUserSex.Value = ds.Tables[0].Rows[0]["usersex"].ToString();
                sltchannel.Value = ds.Tables[0].Rows[0]["channel"].ToString();
                txtUserBirthday.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["userbirthday"]).ToString("yyyy-MM-dd");
                if (Convert.ToDateTime(ds.Tables[0].Rows[0]["userbirthday"]).ToString("yyyy-MM-dd") == "1900-01-01")
                    txtUserBirthday.Value = "";
                txtUserTel.Value = ds.Tables[0].Rows[0]["usertel"].ToString();
                txtUserMobile.Value = ds.Tables[0].Rows[0]["UserMobile"].ToString();
                txtUserEmail.Value = ds.Tables[0].Rows[0]["UserEmail"].ToString();
                txtUserMsn.Value = ds.Tables[0].Rows[0]["UserMsn"].ToString();
                txtQQ.Value = ds.Tables[0].Rows[0]["Userqq"].ToString();
                txtAddress.Value = ds.Tables[0].Rows[0]["Address"].ToString();
                txtZip.Value = ds.Tables[0].Rows[0]["Zip"].ToString();
                sltEducation.Value = ds.Tables[0].Rows[0]["Education"].ToString();
                sltProfession.Value = ds.Tables[0].Rows[0]["Profession"].ToString();
                sltCountry.Value = ds.Tables[0].Rows[0]["Country"].ToString();
                sltProvince.Value = ds.Tables[0].Rows[0]["Province"].ToString();
                sltCity.Value = ds.Tables[0].Rows[0]["City"].ToString();
                sltNetAddress.Value = ds.Tables[0].Rows[0]["NetAddress"].ToString();
                txtRemark.Value = ds.Tables[0].Rows[0]["Remark"].ToString();
            }
        }
        protected void btnregister1_Click(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("UserLogin.aspx?UrlReferrer=UserInfoEdit.aspx");
                return;
            }
            string username = txtUsername.InnerText;
            string password = "";
            string userqq = "";
            if (txtQQ.Value != "")
                userqq = txtQQ.Value;
            string remark = txtRemark.Value;
            string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            string sql = @"update sf_User
set 
Remark=@Remark, 
UserQQ=@UserQQ,
RealName=@RealName,
UserSex=@UserSex,
UserBirthday=@UserBirthday,
UserMsn=@UserMsn,
City=@City ,
Province=@Province,
Country=@Country,
UserEmail =@UserEmail ,
UserTel=@UserTel ,
UserMobile=@UserMobile,
IpAddress=@IpAddress ,
NetAddress=@NetAddress,
Profession =@Profession ,
channel=@channel,
Address=@Address ,
Zip =@Zip ,
Education=@Education,
ImageUrl=@ImageUrl

where UserName=@UserName";

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                SqlParameter[] Param = {
                                          new SqlParameter("@UserName",SqlDbType.VarChar,20),
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
                                          new SqlParameter("@NetAddress",SqlDbType.VarChar,50),
                                          new SqlParameter("@Profession",SqlDbType.VarChar,50),
                                          new SqlParameter("@channel",SqlDbType.VarChar,50),
                                          new SqlParameter("@Address",SqlDbType.VarChar,50),
                                          new SqlParameter("@Zip",SqlDbType.VarChar,50),
                                          new SqlParameter("@Education",SqlDbType.VarChar,50),
                                          new SqlParameter("@ImageUrl",SqlDbType.VarChar,100)
                                   };
                int i = 0;
                string QQ = txtQQ.Value;
                if (QQ == "")
                    QQ = "00000000";
                Param[i++].Value = txtUsername.InnerText;
                Param[i++].Value = txtRemark.Value;
                Param[i++].Value = QQ;
                Param[i++].Value = txtRealName.Value;
                Param[i++].Value = sltUserSex.Items[sltUserSex.SelectedIndex].Text;
                if (txtUserBirthday.Value.Trim() == "")
                    txtUserBirthday.Value = "1900-01-01";
                Param[i++].Value = txtUserBirthday.Value;
                Param[i++].Value = txtUserMsn.Value;
                Param[i++].Value = sltCity.Items[sltCity.SelectedIndex].Text;
                Param[i++].Value = sltProvince.Items[sltProvince.SelectedIndex].Text;
                Param[i++].Value = sltCountry.Items[sltCountry.SelectedIndex].Text;
                Param[i++].Value = txtUserEmail.Value;
                Param[i++].Value = txtUserTel.Value;
                Param[i++].Value = txtUserMobile.Value;
                Param[i++].Value = Request.ServerVariables.Get("Remote_Addr").ToString();
                Param[i++].Value = sltNetAddress.Items[sltNetAddress.SelectedIndex].Text;
                Param[i++].Value = sltProfession.Items[sltProfession.SelectedIndex].Text;
                Param[i++].Value = sltchannel.Items[sltchannel.SelectedIndex].Text;
                Param[i++].Value = txtAddress.Value;
                Param[i++].Value = txtZip.Value;
                Param[i++].Value = sltEducation.Items[sltEducation.SelectedIndex].Text;
                Param[i++].Value = imgUserImg.ImageUrl;
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
                if (ai > 0)
                {
                    Page.RegisterStartupScript("ok", "<script>alert('更新成功！');window.location.href='Default.aspx';</script>");
                }
                else
                {
                    Page.RegisterStartupScript("error", "<script>alert('更新失败！');</script>");
                }
            }
        }
        public string ConvertParam(string sParam)
        {
            return sParam.Replace("'", "''").Replace("singlechar", "'");
        }


        private void CheckNameIsExist(string name, string realname)
        {
            if (Server.HtmlDecode(name.Trim()).Trim() == Server.HtmlDecode(realname.Trim()).Trim())
            {
                Response.Write("no");
                Response.Flush();
                Response.End();
            }
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
        protected void btnUploadImg_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("UserLogin.aspx?UrlReferrer=UserInfoEdit.aspx");
                    return;
                }
                if (System.IO.Path.GetExtension(FileUploadImg.FileName) == ".jpg" || System.IO.Path.GetExtension(FileUploadImg.FileName) == ".JPG" || System.IO.Path.GetExtension(FileUploadImg.FileName) == ".gif" || System.IO.Path.GetExtension(FileUploadImg.FileName) == ".GIF")
                {
                    if (FileUploadImg.PostedFile.ContentLength > 102400)
                    {
                        Page.RegisterStartupScript("sss", "<script>alert('上传的图片不能超过100kb！');</script>");
                        return;
                    }
                    FileUploadImg.SaveAs(HttpContext.Current.Server.MapPath("~/user/img/") + Session["username"].ToString() + ".jpg");
                    imgUserImg.ImageUrl = "img/" + Session["username"].ToString() + ".jpg";
                    string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                    string sql = "update sf_User set ImageUrl='" + imgUserImg.ImageUrl + "' where username='" + Session["username"].ToString() + "'";
                    int value = 0;
                    using (SqlConnection conn = new SqlConnection(conStr))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        value = cmd.ExecuteNonQuery();
                    }
                    if (value > 0)
                        Page.RegisterStartupScript("sss", "<script>alert('上传成功！');</script>");
                }
                else
                {
                    Page.RegisterStartupScript("iii", "<script>alert('图片格式不正确，请上传.jpg或.JPG格式图片！');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.RegisterStartupScript("oo", "<script>alert('上传失败！原因：" + ex.Message + "');</script>");
            }
        }
    }
}
