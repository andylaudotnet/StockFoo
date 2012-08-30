using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Store;
using StockFoo.Analyzer;

namespace StockFoo.Web.manage
{
    public partial class BoostManage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnaddindex_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txttitle.Text))
                {
                    txttitle.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txturl.Text))
                {
                    txturl.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtsite.Text))
                {
                    txtsite.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtbody.Text))
                {
                    txtbody.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtpublishtime.Text))
                {
                    txtpublishtime.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtboost.Text))
                {
                    txtboost.Focus();
                    return;
                }
                try
                {
                    int temp = Convert.ToInt32(txtboost.Text);
                }
                catch
                {
                    txtboost.Focus();
                    return;
                }
                StockFooAnalyzer analyzer = new StockFooAnalyzer(System.Configuration.ConfigurationManager.AppSettings["AnalyzerPath"].ToString());
                FSDirectory dy = FSDirectory.Open(new DirectoryInfo(System.Configuration.ConfigurationManager.AppSettings["IndexDirectory"].ToString()));
                IndexWriter writer = new IndexWriter(dy, analyzer, false, IndexWriter.MaxFieldLength.LIMITED);
                AddDocument(writer, txttitle.Text, txturl.Text, txtsite.Text, txtbody.Text.Length > 200 ? txtbody.Text.Substring(0, 200) : txtbody.Text, txtpublishtime.Text, Convert.ToInt32(txtboost.Text));
                writer.Optimize();
                writer.Close();
                Page.RegisterStartupScript("ok", "<script>alert('新建成功！');</script>");
            }
            catch (Exception ex)
            {
                Page.RegisterStartupScript("error","<script>alert('新建失败！"+ex.Message+"');</script>");
            }
        }

        private void AddDocument(IndexWriter writer, string title, string url, string site, string body, string publish_time,int boost)
        {
            Document document = new Document();
            Field ftitle = new Field("title", title, Field.Store.YES, Field.Index.ANALYZED);
            document.Add(ftitle);//存储，索引
            document.Add(new Field("url", url, Field.Store.YES, Field.Index.NOT_ANALYZED));//存储，不索引
            document.Add(new Field("site", site, Field.Store.YES, Field.Index.NOT_ANALYZED));//存储，不索引
            Field fbody = new Field("body", body, Field.Store.YES, Field.Index.ANALYZED);
            document.Add(fbody);//存储，索引
            document.Add(new Field("publish_time", publish_time, Field.Store.YES, Field.Index.NOT_ANALYZED));//存储，不索引
            document.SetBoost(boost);
            writer.AddDocument(document);
        }
    }
}
