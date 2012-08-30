using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Store;
using StockFoo.Analyzer;

namespace StockFoo.Web.manage
{
    public partial class AnalyzerIndex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["ResponseBuilded"] != null)
            {
                CreateIndex();
            }
        }

        private void GetSession()
        {
            string ss = "no";
            if (Session["builderesult"] != null)
            {
                ss = Session["builderesult"].ToString().Trim();
            }
            Response.Write(ss);
            Response.Flush();
            Response.End();
        }

        private void CreateIndex()
        {
            string sresult = "";
            try
            {
                //读取数据库数据
                SqlDataReader myred = ExecuteQuery();

                //建立索引字段
                //Lucene.Net.Analysis.Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_CURRENT);
                StockFooAnalyzer analyzer = new StockFooAnalyzer(System.Configuration.ConfigurationManager.AppSettings["AnalyzerPath"].ToString());
                
                FSDirectory dy = FSDirectory.Open(new DirectoryInfo(Server.MapPath("IndexDirectory")));
                IndexWriter writer = new IndexWriter(dy, analyzer, true,IndexWriter.MaxFieldLength.LIMITED);
                while (myred.Read())
                {
                    AddDocument(writer, myred["title"].ToString(), myred["url"].ToString(), myred["site"].ToString(), myred["body"].ToString(), myred["publish_time"].ToString());
                }
                myred.Close();
                myred.Dispose();

                writer.Optimize();
                writer.Close();

                sresult = "ok";
            }
            catch(Exception ex)
            {
                sresult = ex.Message;
            }
            Response.Write(sresult);
            Response.Flush();
            Response.End(); 
        }
        private void AddDocument(IndexWriter writer, string title, string url, string site, string body, string publish_time)
        {
            Document document = new Document();
            Field ftitle = new Field("title", title, Field.Store.YES, Field.Index.ANALYZED);//存储，索引
            document.Add(ftitle);
            Field furl = new Field("url", url, Field.Store.YES, Field.Index.NOT_ANALYZED);//存储，不索引
            document.Add(furl);
            Field fsite = new Field("site", site, Field.Store.YES, Field.Index.NOT_ANALYZED);//存储，不索引
            document.Add(fsite);
            Field fbody = new Field("body", body, Field.Store.YES, Field.Index.ANALYZED);//存储，索引
            document.Add(fbody);
            Field fpublishtime = new Field("publish_time", publish_time, Field.Store.YES, Field.Index.NOT_ANALYZED);//存储，不索引
            document.Add(fpublishtime);
            writer.AddDocument(document);
        }


        private SqlDataReader ExecuteQuery()
        {
            string sql = "select  title,url,site,(left(body,300)+'...') as body,publish_time from sf_article order by publish_time desc";
            string connstr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            SqlConnection con = new SqlConnection(connstr);
            if (con.State == ConnectionState.Closed) con.Open();

            SqlCommand command = new SqlCommand(sql, con);
            SqlDataReader datareader = command.ExecuteReader();
            return datareader;
        }
    }
}
