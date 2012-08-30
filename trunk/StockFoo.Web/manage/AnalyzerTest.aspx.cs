using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StockFoo.Analyzer;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace StockFoo.Web.manage
{
    public partial class AnalyzerTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                txtAnalyzer.Focus();
            }
        }

        private string AnalyzerObject()
        {
            string testwords = txtAnalyzer.Text;
            StockFooAnalyzer sa = new StockFooAnalyzer(System.Configuration.ConfigurationManager.AppSettings["AnalyzerPath"].ToString());

            Stopwatch timer = new Stopwatch();
            timer.Start();
            Lucene.Net.Analysis.TokenStream ts = sa.TokenStream(null, new StringReader(testwords));
            timer.Stop();
            Token token = ts.Next();
            StringBuilder sb = new StringBuilder();
            sb.Append("<font color=blue>分词效果如下：</font>(耗时：" + timer.ElapsedMilliseconds + "毫秒)<br/>");
            while (token != null)
            {
                sb.Append(token.TermText() + "&nbsp;&nbsp;&nbsp;" + token.StartOffset() + "&nbsp;&nbsp;&nbsp;" + token.EndOffset() + "<br/>");
                token = ts.Next();
            }
            ts.Close();
            return sb.ToString();
        }

        protected void btnAnalyzer_Click(object sender, EventArgs e)
        {
            if (!txtAnalyzer.Text.Trim().Equals(""))
            {
                labResult.Text =AnalyzerObject();
            }
            else
            {
                labResult.Text = "<font color=red>分词语句不能为空！</font>";
                txtAnalyzer.Focus();
            }
        }

        protected void btnBlank_Click(object sender, EventArgs e)
        {
            txtAnalyzer.Text = "";
            labResult.Text = "";
            txtAnalyzer.Focus();
        }
    }
}
