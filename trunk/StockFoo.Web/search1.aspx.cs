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
using System.Text;
using System.Xml.Linq;
using StockFoo.Entity;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System.IO;
using StockFoo.Analyzer;

namespace StockFoo.Web
{
    public partial class search1 : System.Web.UI.Page
    {
        protected string SearchWords = "";
        protected string TotalCount = "";
        protected string SearchTime = "";
        protected string PageInfo="";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["w"] != null && Request["w"].ToString().Length > 0)
                {
                    int PageIndex = 1;
                    if (!int.TryParse(Request.QueryString["pi"], out PageIndex)) PageIndex = 1;
                    SelectWordResult(Server.HtmlDecode(Request["w"].ToString()), PageIndex);
                }
                else
                {
                    Response.Redirect("default.aspx");
                }
            }
        }

        private void SelectWordResult(string searchStr, int pageIndex)
        {
            try
            {
                SearchWords = searchStr;
                DateTime dt1 = DateTime.Now;

                //Lucene.Net.Analysis.Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_CURRENT);
                StockFooAnalyzer analyzer = new StockFooAnalyzer(System.Configuration.ConfigurationManager.AppSettings["AnalyzerPath"].ToString());
                FSDirectory dy = FSDirectory.Open(new DirectoryInfo(Server.MapPath("manage/IndexDirectory")));
                IndexSearcher searcher = new IndexSearcher(dy, false);

                //多个索引文件查询
                //MultiReader reader = new MultiReader(new IndexReader[] { IndexReader.Open(@"c:\index"), IndexReader.Open(@"\\server\index") });
                //IndexSearcher searcher = new IndexSearcher(reader);
                //Hits hits = searcher.Search(query);
                //或 
                //IndexSearcher[] searchers = new IndexSearcher[2];
                //searchers[0] = new IndexSearcher(Server.MapPath("manage/IndexDirectory01"));
                //searchers[1] = new IndexSearcher(Server.MapPath("manage/IndexDirectory02"));
                //MultiSearcher multiSearcher = new MultiSearcher(searchers); 
                //可以使用 ParallelMultiSearcher 进行多线程并行搜索。

                MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_CURRENT, new string[] { "title","body"}, analyzer);
                Query query = parser.Parse(searchStr);
                //单个条件排序
                //Sort sort = new Sort();
                //SortField f = new SortField("publish_time", SortField.STRING, true);//按照publish_time字段排序，true表示降序
                //sort.SetSort(f);
                //多个条件排序
                //Sort sort = new Sort();
                //SortField f1 = new SortField("title", SortField.SCORE, false);
                //SortField f2 = new SortField("publish_time", SortField.STRING, true);
                //sort.SetSort(new SortField[] { f1, f2 });

                Hits hits = searcher.Search(query);
                //TopDocs docs = searcher.Search(query, null, 10000);
                
                //进行多条件搜索
                //Query query1 = new TermQuery(new Term(FieldValue, "name1")); // 词语搜索
                //Query query2 = new WildcardQuery(new Term(FieldName, "name*")); // 通配符
                //Query query3 = new PrefixQuery(new Term(FieldName, "name1")); // 字段搜索 Field:Keyword，自动在结尾添加 *
                //Query query4 = new RangeQuery(new Term(FieldNumber, NumberTools.LongToString(11L)), new Term(FieldNumber, NumberTools.LongToString(13L)), true); // 范围搜索
                //Filter filter = new DateFilter(FieldDate, DateTime.Parse("2005-10-1"), DateTime.Parse("2005-10-30"));
                //Query query5 = new FilteredQuery(query, filter); // 带过滤条件的搜索

                //BooleanQuery query = new BooleanQuery();
                //query.Add(query1, BooleanClause.Occur.MUST);
                //query.Add(query2, BooleanClause.Occur.MUST);

                //IndexSearcher searcher= new IndexSearcher(reader);
                //Hits hits = searcher.Search(query);
                
                int totalCount = 0;
                if (hits != null && hits.Length() > 0)
                {
                    ArrayList arr01 = new ArrayList();
                    ArrayList arr02 = new ArrayList();
                    for (int i = 0; i < hits.Length(); i++)
                    {
                            Document doc = hits.Doc(i);
                            string url = doc.Get("url").ToString();
                            string title = doc.Get("title").ToString();
                            string body = doc.Get("body").ToString();
                            string publishtime = doc.Get("publish_time").ToString();
                            string site = doc.Get("site").ToString();

                            if (searchStr.IndexOf(" ") > -1)
                            {
                                bool isexisit = false;
                                string[] word = searchStr.Split(' ');
                                for (int j = 0; j < word.Length; j++)
                                {
                                    if (word[j].Trim().Equals(""))
                                    {
                                        continue;
                                    }
                                    if (title.IndexOf(word[j].Trim()) > -1)
                                    {
                                        isexisit = true;
                                        break;
                                    }
                                }
                                if (isexisit)
                                {
                                    arr01.Add(doc);
                                }
                                else
                                {
                                    arr02.Add(doc);
                                }
                            }
                            else
                            {
                                if (title.IndexOf(searchStr) > -1)
                                {
                                    arr01.Add(doc);
                                }
                                else
                                {
                                    arr02.Add(doc);
                                }
                            }
                    }
                    arr01.Sort(new DateTimeCompare());
                    arr02.Sort(new DateTimeCompare());
                    ArrayList arrAll = new ArrayList();
                    foreach (Document doo in arr01)
                    {
                        arrAll.Add(doo);
                    }
                    foreach (Document doo in arr02)
                    {
                        arrAll.Add(doo);
                    }
                    
                    StringBuilder sb = new StringBuilder();
                    int begincount = 0;
                    if (pageIndex > 1)
                        begincount = (pageIndex - 1) * 10;
                    int endcount = pageIndex * 10;
                    //if (endcount > hits.Length() || endcount < 10)
                    //    endcount = hits.Length();
                    if (endcount > arrAll.Count || endcount < 10)
                        endcount = arrAll.Count;
                    for (int i = begincount; i < endcount; i++)
                    {
                        //ScoreDoc doc = docs.scoreDocs[i];
                        //string url = searcher.Doc(doc.doc).Get("url").ToString();

                        //Document doc = hits.Doc(i);
                        Document doc = (Document)arrAll[i];
                        string url = doc.Get("url").ToString();
                        string title = doc.Get("title").ToString();
                        string body = doc.Get("body").ToString();
                        body = body.Length > 100 ? body.Substring(0, 100) + "..." : body;
                        if (searchStr.IndexOf(" ") > -1)
                        {
                            title = GetRedWord(title, searchStr);
                            body = GetRedWord(body, searchStr);
                        }
                        else
                        { 
                            title = title.Replace(searchStr, "<font color=red>" + searchStr + "</font>");
                            body = body.Replace(searchStr, "<font color=red>" + searchStr + "</font>");
                        }
                        string publishtime = doc.Get("publish_time").ToString();
                        string site = doc.Get("site").ToString();

                        sb.Append(" <div style=\"margin-top: 20px; margin-bottom: 20px;width:666px;\">");
                        sb.Append(" <div>");
                        sb.Append(" <a target=\"_blank\"  href=\"" + url + "\"><font color=blue>" +title+ "</font></a>");//&nbsp;&nbsp;<font color=#666699>" + doc.score.ToString("0.00")+"</font>
                        sb.Append(" </div>");
                        sb.Append(" <div style=\"font-size: 13px;\">");
                        sb.Append(" <font size=2 >" + body + "</font>");
                        sb.Append(" </div>");
                        sb.Append(" <div>");
                        string datetime = "";
                        try
                        {
                            datetime = Convert.ToDateTime(publishtime).ToString("yyyy-MM-dd HH:mm");
                        }
                        catch
                        {
                            datetime = publishtime;
                        }
                        sb.Append(" <font color=#006600>" + (url.Length > 66 ? url.Substring(0, 66) : url) + "</font>&nbsp;&nbsp;<font size=2 color=#006600>" + datetime + "</font>&nbsp;&nbsp;<font size=2  color=#666699>[" + site + "]</font>");
                        sb.Append("  </div>");
                        sb.Append(" </div>");
                    }
                    labResultList.Text = sb.ToString();

                    //totalCount = hits.Length();
                    totalCount = arrAll.Count;
                    TotalCount = totalCount.ToString();
                    int totalPage = totalCount % 10 == 0 ? totalCount / 10 : totalCount / 10 + 1;
                    if (totalPage > 1)
                    {
                        StringBuilder sbp = new StringBuilder();
                        string parame = "";
                        if (pageIndex > 1)
                        {
                            parame = "search1.aspx?pi=" + (pageIndex - 1).ToString() + "&w=" + System.Web.HttpUtility.UrlEncode(searchStr);
                            sbp.Append("<a  href=\"" + parame + "\">上一页</a> ");
                        }
                        int beginindex = 1;
                        int endindex = totalPage;
                        if (totalPage > 20 && pageIndex > 11 && totalPage - pageIndex > 8)
                        {
                            beginindex = pageIndex - 10;
                        }
                        if (totalPage > 20 && pageIndex > 11 && totalPage - pageIndex < 9)
                        {
                            beginindex = pageIndex - (20 - (totalPage - pageIndex) - 1);
                        }
                        if (endindex > 10)
                        {
                            endindex = pageIndex + 9;
                            if (endindex > totalPage)
                            {
                                endindex = totalPage;
                            }
                        }
                        for (int i = beginindex; i <= endindex; i++)
                        {
                            if ((i) == pageIndex)
                            {
                                sbp.Append(" " + pageIndex);
                            }
                            else
                            {
                                parame = "search1.aspx?pi=" + (i).ToString() + "&w=" + System.Web.HttpUtility.UrlEncode(searchStr);
                                sbp.Append("<a href=\"" + parame + "\">[" + (i).ToString() + "]</a>");
                            }
                        }
                        if (pageIndex < totalPage)
                        {
                            parame = "search1.aspx?pi=" + (pageIndex + 1).ToString() + "&w=" + System.Web.HttpUtility.UrlEncode(searchStr);
                            sbp.Append(" <a   href=\"" + parame + "\">下一页</a>");
                        }
                        PageInfo = sbp.ToString();
                    }
                    DateTime dt2 = DateTime.Now;
                    TimeSpan ts = dt2 - dt1;
                    double timeuse = Convert.ToInt32(ts.TotalMilliseconds) * 0.001;
                    SearchTime = timeuse.ToString("0.001");

                    searcher.Close();
                }
                else
                {
                    SearchWords = searchStr;
                    TotalCount = "";
                    SearchTime = "";
                    PageInfo = "";
                    labResultList.Text = "<br/>抱歉，未找到：“" + SearchWords + "”相关内容。<br/><br/><b>建议您：</b> <br/>1、看看输入的文字是否有误<br/>2、去掉可能不必要的字词，如“的”、“什么”等<br/>3、<a href=\"http://www.haopingba.com/Page1Result.aspx?web=aall&searchword=" + searchStr + "\" target=\"_blank\">去此处查询</a>";
       
                }
            }
            catch (Exception ex)
            {
                SearchWords = searchStr;
                TotalCount ="";
                SearchTime =""; 
                PageInfo="";
                labResultList.Text = "<br/>检索异常：" + ex.Message + "<br/><br/><b>建议您：</b> <br/>1、看看输入的文字是否有误<br/>2、去掉可能不必要的字词，如“的”、“什么”等<br/>3、<a href=\"http://www.haopingba.com/Page1Result.aspx?web=aall&searchword=" + searchStr + "\" target=\"_blank\">去此处查询</a>";
            } 
        }

        private string GetRedWord(string titleorbody, string searchStr)
        {
            
            string[] word = searchStr.Split(' ');
            for (int j = 0; j < word.Length; j++)
            {
                if (word[j].Trim().Equals(""))
                {
                    continue;
                }
                if (titleorbody.IndexOf(word[j].Trim()) > -1)
                {
                    titleorbody=titleorbody.Replace(word[j].Trim(), "<font color=red>" + word[j].Trim() + "</font>");
                }
            }
            return titleorbody;
        }
    }

    public class DateTimeCompare :IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            try
            {
                Document xdoc = (Document)x;
                DateTime xpublishtime=Convert.ToDateTime(xdoc.Get("publish_time"));
                Document ydoc = (Document)y;
                DateTime ypublishtime = Convert.ToDateTime(ydoc.Get("publish_time"));
                return ypublishtime.CompareTo(xpublishtime);
            }
            catch
            {
                throw;
            } 
        }
    }
}
