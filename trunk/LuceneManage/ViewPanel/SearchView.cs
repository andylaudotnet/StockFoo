using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System.IO;
using StockFoo.Analyzer;

namespace LuceneManage.ViewPanel {
    public partial class SearchView : UserControl, IUpdateUI {
        public SearchView() {
            InitializeComponent();
            comboBox1.SelectedText = "StockFooAnalyzer";
        }

        #region IUpdateUI 成员

        public void UpdateUI() {
           
        }

        #endregion

        private void btnsearch_Click(object sender, EventArgs e)
        {
            Analyzer analyzer = GetAnalyer(comboBox1.SelectedIndex);
            FSDirectory dy = FSDirectory.Open(new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["IndexDirectory"].ToString()));
            IndexSearcher searcher = new IndexSearcher(dy, false);
            MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_CURRENT, new string[] { "title", "body" }, analyzer);
            Query query = parser.Parse(txtsearch.Text);
            Hits hits = searcher.Search(query);
            labexpress.Text = "共搜索到" + hits.Length().ToString() + "条记录，Query表达式为：" + query.ToString();
            listView1.Clear();
            listView1.Columns.Add("id", 120);
            listView1.Columns.Add("title", 400);
            listView1.Columns.Add("url", 400);
            listView1.Columns.Add("site", 120);
            listView1.Columns.Add("body", 400);
            listView1.Columns.Add("publishtime", 400);
            for (int i = 0; i < hits.Length(); i++)
            {
                Document doc = hits.Doc(i);
                listView1.Items.Add(new ListViewItem(new string[] { (i + 1).ToString(), doc.Get("title"), doc.Get("url").ToString(), doc.Get("site").ToString(), doc.Get("body").ToString(), doc.Get("publish_time").ToString() }));
            }
            searcher.Close();
        }

        private Analyzer GetAnalyer(int type)
        { 
            Analyzer analyzer;
            string path =System.Configuration.ConfigurationSettings.AppSettings["Data"].ToString();
            switch (type)
            {
                case 0:
                    analyzer=new StockFooAnalyzer(path);
                    break;
               case 1:
                    analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_CURRENT);
                    break;
              case 2:
                    analyzer = new SimpleAnalyzer();
                    break;
              case 3:
                    analyzer = new StopAnalyzer(Lucene.Net.Util.Version.LUCENE_CURRENT);
                    break;
             case 4:
                    analyzer = new KeywordAnalyzer();
                    break;
              case 5:
                    analyzer = new WhitespaceAnalyzer();
                    break;
             default:
                    analyzer = new StockFooAnalyzer(path);
                    break;
            }
            return analyzer;
        }
    }
}
