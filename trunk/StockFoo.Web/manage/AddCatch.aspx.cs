using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using StockFoo.Entity;

namespace StockFoo.Web.manage
{
    public partial class AddCatch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/user/Login.aspx?UrlReferrer=../manage/AddCatch.aspx");
                }
                else
                {
                    if (GetUserLevel(Session["username"].ToString().Trim()) != 1)
                    {
                        Response.Redirect("~/user/Default.aspx");
                    }
                }
                DataTable dt = GetCnInterfaceType();
                ddlClassID.Items.Clear();
                ddlClassID.Items.Add(new ListItem("请选择类型", "-1"));
                foreach (DataRow dr in dt.Rows)
                {
                    ListItem li = new ListItem();
                    li.Text = dr["class_name"].ToString();
                    li.Value = dr["id"].ToString();
                    ddlClassID.Items.Add(li);
                }
                txtNextTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        private  DataTable GetCnInterfaceType()
        {
            string sql = "select  id,class_name from  [sf_Class]";
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

        protected void bntAddCatch_Click(object sender, EventArgs e)
        {
            if (txtCatchName.Text.Equals(""))
            {
                txtCatchName.Focus();
                return;
            }
            if (txtSiteName.Text.Equals(""))
            {
                txtSiteName.Focus();
                return;
            }
            if (ddlClassID.SelectedItem.Value == "-1")
            {
                ddlClassID.Focus();
                return;
            }
            try
            {
                DateTime dt = Convert.ToDateTime(txtNextTime.Text);
            }
            catch
            {
                txtNextTime.Focus();
                return;
            }
            try
            {
                int temp = Convert.ToInt32(txtTimespan.Text);
            }
            catch
            {
                txtTimespan.Focus();
                return;
            }
            StockFoo.Entity.Catch ch = new StockFoo.Entity.Catch();
            ch.Guid = Guid.NewGuid();
            ch.CatchName = txtCatchName.Text;
            ch.SiteName = txtSiteName.Text;
            ch.SiteEncode =Convert.ToInt32(ddlSiteEncode.SelectedItem.Value);
            ch.ClassId = Convert.ToInt32(ddlClassID.SelectedItem.Value);
            ch.CatalogUrl = txtCatalogUrl.Text;
            ch.CatalogXPath = txtCatalogXPath.Text;
            ch.CatalogRegex = txtCatalogRegex.Text;
            ch.ArticleXPath = txtArticleXPath.Text;
            ch.ArticleRegex = txtArticleRegex.Text;
            ch.ArticleTitleXPath =txtArticleTitleXPath.Text;
            ch.ArticleTitleRegex = txtArticleTitleRegex.Text;
            ch.ArticleTimeXPath = txtArticleTimeXPath.Text;
            ch.ArticleTimeRegex = txtArticleTimeRegex.Text;
            ch.ArticleTimeFormat = txtArticleTimeFormat.Text;
            if (rblEnabled.SelectedItem.Value == "1")
            {
                ch.Enabled = true;
            }
            else
            {
                ch.Enabled = false;
            }
            ch.NextTime =Convert.ToDateTime(txtNextTime.Text);
            ch.TimeSpan = Convert.ToInt32(txtTimespan.Text);
            int insertcount = InsertCatch(ch);
            if (insertcount > 0)
            {
                Page.RegisterStartupScript("ok", "<script>alert('添加成功！');</script>");
            }
            else
            {
                Page.RegisterStartupScript("error", "<script>alert('添加失败！');</script>");
            }
        }

        private int InsertCatch(StockFoo.Entity.Catch ch)
        {
            int excutecount = 0;
            string sql = @"DECLARE @ch_id INT
SELECT  @ch_id = ID
FROM    [sf_Catch]
WHERE   catch_name = @catch_name
IF @ch_id IS NULL 
    BEGIN
INSERT INTO [sf_Catch]
           ([guid]
           ,[catch_name]
           ,[site_name]
           ,[site_encode]
           ,[classid]
           ,[catalog_url]
           ,[catalog_xpath]
           ,[catalog_regex]
           ,[article_xpath]
           ,[article_regex]
           ,[article_title_xpath]
           ,[article_title_regex]
           ,[article_time_xpath]
           ,[article_time_regex]
           ,[article_time_format]
           ,[enabled]
           ,[next_time]
           ,[timespan])
     VALUES
           (@guid
           ,@catch_name
           ,@site_name
           ,@site_encode
           ,@classid
           ,@catalog_url
           ,@catalog_xpath
           ,@catalog_regex
           ,@article_xpath
           ,@article_regex
           ,@article_title_xpath
           ,@article_title_regex
           ,@article_time_xpath
           ,@article_time_regex
           ,@article_time_format
           ,@enabled
           ,@next_time
           ,@timespan)
      END";
            string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@guid", ch.Guid);
                cmd.Parameters.AddWithValue("@catch_name", ch.CatchName);
                cmd.Parameters.AddWithValue("@site_name", ch.SiteName);
                cmd.Parameters.AddWithValue("@site_encode", ch.SiteEncode);
                cmd.Parameters.AddWithValue("@classid", ch.ClassId);
                cmd.Parameters.AddWithValue("@catalog_url", ch.CatalogUrl);
                cmd.Parameters.AddWithValue("@catalog_xpath", ch.CatalogXPath);
                cmd.Parameters.AddWithValue("@catalog_regex", ch.CatalogRegex);
                cmd.Parameters.AddWithValue("@article_xpath", ch.ArticleXPath);
                cmd.Parameters.AddWithValue("@article_regex", ch.ArticleRegex);
                cmd.Parameters.AddWithValue("@article_title_xpath", ch.ArticleTitleXPath);
                cmd.Parameters.AddWithValue("@article_title_regex", ch.ArticleTitleRegex);
                cmd.Parameters.AddWithValue("@article_time_xpath", ch.ArticleTimeXPath);
                cmd.Parameters.AddWithValue("@article_time_regex", ch.ArticleTimeRegex);
                cmd.Parameters.AddWithValue("@article_time_format", ch.ArticleTimeFormat);
                cmd.Parameters.AddWithValue("@enabled", ch.Enabled);
                cmd.Parameters.AddWithValue("@next_time", ch.NextTime);
                cmd.Parameters.AddWithValue("@timespan", ch.TimeSpan);

                excutecount = cmd.ExecuteNonQuery();
            }
            return excutecount;
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
