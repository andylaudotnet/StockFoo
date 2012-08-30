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
using System .Net;
using HtmlAgilityPack;
using System .Text;
using System.Text.RegularExpressions;

namespace StockFoo.Web.manage
{
    public partial class UpdateCatch : System.Web.UI.Page
    {
        protected string cataloglisturl = "";
        protected string articlefirsturl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/user/Login.aspx?UrlReferrer=../manage/UpdateCatch.aspx?id=" + Request["id"].ToString());
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
                 StockFoo.Entity.Catch ch=GetCatchByID(Convert.ToInt32(Request["id"]));
                  txtCatchName.Text=ch.CatchName;
                  txtSiteName.Text=ch.SiteName;
                  ddlSiteEncode.Items.FindByValue(ch.SiteEncode.ToString()).Selected = true;
                  ddlClassID.Items.FindByValue(ch.ClassId.ToString()).Selected = true;
                  txtCatalogUrl.Text=ch.CatalogUrl;
                  cataloglisturl = txtCatalogUrl.Text;
                  txtCatalogXPath.Text=ch.CatalogXPath;
                  txtCatalogRegex.Text=ch.CatalogRegex;
                  txtArticleXPath.Text=ch.ArticleXPath;
                  txtArticleRegex.Text=ch.ArticleRegex;
                  txtArticleTitleXPath.Text=ch.ArticleTitleXPath;
                  txtArticleTitleRegex.Text=ch.ArticleTitleRegex;
                  txtArticleTimeXPath.Text=ch.ArticleTimeXPath;
                 txtArticleTimeRegex.Text=ch.ArticleTimeRegex ;
                 txtArticleTimeFormat.Text=ch.ArticleTimeFormat ;
                 string enable = ch.Enabled ? "1" : "0";
                 rblEnabled.Items.FindByValue(enable).Selected = true;
                 txtNextTime.Text =Convert.ToDateTime(ch.NextTime).ToString("yyyy-MM-dd HH:mm:ss");
                 txtTimespan.Text = ch.TimeSpan.ToString();

                WebClient wc = new WebClient
                {
                    Encoding = Encoding.GetEncoding(ddlSiteEncode.SelectedItem.Text)
                };
                string html = "";
                try
                {
                    html=wc.DownloadString(txtCatalogUrl.Text);
                    txtCatalogUrl.ForeColor = System.Drawing.Color.Green; 
                }
                catch { 
                    txtCatalogUrl.ForeColor = System.Drawing.Color.Red; 
                }
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                VilidateXpath(doc, txtCatalogXPath);

                try
                {
                    try
                    {
                        txtArticleInfo.Text = doc.DocumentNode.SelectSingleNode(txtCatalogXPath.Text + "[1]/li/a[1]").Attributes["href"].Value;
                        txtCatalogRegex.ForeColor = System.Drawing.Color.Green; 
                        if (txtCatalogUrl.Text.IndexOf("hexun") > -1)
                        {
                            try
                            {
                                txtArticleInfo.Text = doc.DocumentNode.SelectSingleNode(txtCatalogXPath.Text + "[1]/li/a[2]").Attributes["href"].Value;
                                txtCatalogRegex.ForeColor = System.Drawing.Color.Green; 
                            }
                            catch
                            {
                                var regex = new Regex(txtCatalogRegex.Text);
                                Match m;
                                string ulhtml = doc.DocumentNode.SelectSingleNode(txtCatalogXPath.Text + "[1]").InnerHtml;
                                if (!(m = regex.Match(ulhtml)).Success || m.Groups.Count != 2)
                                {
                                    txtArticleInfo.Text = "页面地址匹配错误.";
                                    txtCatalogRegex.ForeColor = System.Drawing.Color.Red;
                                }
                                else
                                {
                                    txtArticleInfo.Text = m.Groups[1].Value;
                                    txtCatalogRegex.ForeColor = System.Drawing.Color.Green; 
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                         txtArticleInfo.Text = ex.Message;
                         txtCatalogRegex.ForeColor = System.Drawing.Color.Red; 
                    }
                    articlefirsturl = txtArticleInfo.Text;

                    html = wc.DownloadString(txtArticleInfo.Text);
                    //System.IO.StreamReader sr = new System.IO.StreamReader("D:\\StockFoo\\qqstock.txt", Encoding.GetEncoding("utf-8"));
                    //lock (sr)
                    //{
                    //    html = sr.ReadToEnd();
                    //    sr.Close();
                    //}
                    txtArticleInfo.ForeColor = System.Drawing.Color.Green;
                }
                catch
                {
                    txtArticleInfo.ForeColor = System.Drawing.Color.Red;
                }
                doc.LoadHtml(html);
                VilidateXpath(doc, txtArticleXPath);
                VilidateXpath(doc, txtArticleTitleXPath);
                VilidateXpath(doc, txtArticleTimeXPath);
            }
        }
        private DataTable GetCnInterfaceType()
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

        private StockFoo.Entity.Catch GetCatchByID(int id)
        {
            string sql = "select * from  [sf_Catch] where id=" + id;
            StockFoo.Entity.Catch ch = null;
            string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                ch = new StockFoo.Entity.Catch();
                ch.Id =(int)sdr["id"];
                ch.CatchName = sdr["catch_name"].ToString();
                ch.SiteName = sdr["site_name"].ToString();
                ch.SiteEncode = (int)sdr["site_encode"];
                ch.ClassId = (int)sdr["classid"];
                ch.CatalogUrl = sdr["catalog_url"].ToString();
                ch.CatalogXPath = sdr["catalog_xpath"].ToString();
                ch.CatalogRegex = sdr["catalog_regex"].ToString();
                ch.ArticleXPath = sdr["article_xpath"].ToString();
                ch.ArticleRegex = sdr["article_regex"].ToString();
                ch.ArticleTitleXPath = sdr["article_title_xpath"].ToString();
                ch.ArticleTitleRegex = sdr["article_title_regex"].ToString();
                ch.ArticleTimeXPath = sdr["article_time_xpath"].ToString();
                ch.ArticleTimeRegex = sdr["article_time_regex"].ToString();
                ch.ArticleTimeFormat = sdr["article_time_format"].ToString();
                ch.Enabled = (bool)sdr["enabled"];
                ch.NextTime = (DateTime)sdr["next_time"];
                ch.TimeSpan = Convert.ToInt32(sdr["timespan"]);
            }
            return ch;
        }

        protected void bntUpdateCatch_Click(object sender, EventArgs e)
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
            ch.Id = Convert.ToInt32(Request["id"]);
            ch.CatchName = txtCatchName.Text;
            ch.SiteName = txtSiteName.Text;
            ch.SiteEncode = Convert.ToInt32(ddlSiteEncode.SelectedItem.Value);
            ch.ClassId = Convert.ToInt32(ddlClassID.SelectedItem.Value);
            ch.CatalogUrl = txtCatalogUrl.Text;
            ch.CatalogXPath = txtCatalogXPath.Text;
            ch.CatalogRegex = txtCatalogRegex.Text;
            ch.ArticleXPath = txtArticleXPath.Text;
            ch.ArticleRegex = txtArticleRegex.Text;
            ch.ArticleTitleXPath = txtArticleTitleXPath.Text;
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
            ch.NextTime = Convert.ToDateTime(txtNextTime.Text);
            ch.TimeSpan = Convert.ToInt32(txtTimespan.Text);
            int insertcount = UpdateCatchByID(ch);
            if (insertcount > 0)
            {
                Page.RegisterStartupScript("ok", "<script>alert('更新成功！');window.location.href='UpdateCatch.aspx?id="+Request["id"].ToString()+"';</script>");
            }
            else
            {
                Page.RegisterStartupScript("error", "<script>alert('更新失败！');</script>");
            }
        }

        private int UpdateCatchByID(StockFoo.Entity.Catch ch)
        {
            int excutecount = 0;
            string sql = @"UPDATE [sf_Catch]
      SET 
       [catch_name] = @catch_name
      ,[site_name] = @site_name
      ,[site_encode] = @site_encode
      ,[classid] = @classid
      ,[catalog_url] = @catalog_url
      ,[catalog_xpath] = @catalog_xpath
      ,[catalog_regex] = @catalog_regex
      ,[article_xpath] = @article_xpath
      ,[article_regex] = @article_regex
      ,[article_title_xpath] = @article_title_xpath
      ,[article_title_regex] =@article_title_regex
      ,[article_time_xpath] = @article_time_xpath
      ,[article_time_regex] = @article_time_regex
      ,[article_time_format] = @article_time_format
      ,[enabled] = @enabled
      ,[next_time] = @next_time
      ,[timespan] = @timespan
 WHERE [id]=@id
          ";
            string conStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", ch.Id);
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

        private void VilidateXpath(HtmlDocument doc,TextBox txt)
        {
             try
             {
                 string result = doc.DocumentNode.SelectSingleNode(txt.Text).InnerHtml;
                 txt.ForeColor = System.Drawing.Color.Green;
             }
            catch
             {
                 txt.ForeColor = System.Drawing.Color.Red;
             }
        }
    }
}
