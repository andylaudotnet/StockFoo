using System;
using System.Collections;
using System.Collections.Generic;
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

                Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_CURRENT);
                FSDirectory dy = FSDirectory.Open(new DirectoryInfo(Server.MapPath("manage/IndexDirectory")));
                IndexSearcher searcher = new IndexSearcher(dy, false);
                Hits hits01 = GetHitsByFiled("title", analyzer, searchStr, searcher);
                Hits hits02 = GetHitsByFiled("body", analyzer, searchStr, searcher);
                 IList<Document> IDD=new List<Document>();
                 if (hits01 != null && hits01.Length() > 0)
                 {
                     for (int i = 0; i < hits01.Length(); i++)
                     {
                         Document doc = hits01.Doc(i);
                         IDD.Add(doc);
                     }
                 }
                 if (hits02 != null && hits02.Length() > 0)
                 {
                     for (int i = 0; i < hits02.Length(); i++)
                     {
                         Document doc = hits02.Doc(i);
                         string url = doc.Get("url").ToString().Trim();
                         bool isexisit = false;
                         if (hits01 != null && hits01.Length() > 0)
                         {
                             for (int j = 0; j < hits01.Length(); j++)
                             {
                                 Document doctemp = hits01.Doc(j);
                                 if(url.Equals(doctemp.Get("url").ToString().Trim()))
                                 {
                                     isexisit = true;
                                 }
                             }
                         }
                         if (!isexisit)
                         {
                             IDD.Add(doc);
                         }
                     }
                 }

                #region 其他
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
                //单个条件排序
                //Sort sort = new Sort();
                //SortField f = new SortField("publish_time", SortField.STRING, true);//按照publish_time字段排序，true表示降序
                //sort.SetSort(f);
                //多个条件排序
                //Sort sort = new Sort();
                //SortField f1 = new SortField("publish_time", SortField.DOC, true);
                //SortField f2 = new SortField("id", SortFiedl.INT, false);
                //sort.setSort(new SortField[] { f1, f2 });
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
                #endregion

                int totalCount = 0;
                if (IDD != null && IDD.Count > 0)
                {

                    StringBuilder sb = new StringBuilder();
                    int begincount = 0;
                    if (pageIndex > 1)
                        begincount = (pageIndex - 1) * 10;
                    int endcount = pageIndex * 10;
                    if (endcount > IDD.Count || endcount < 10)
                        endcount = IDD.Count;
                    for (int i = begincount; i < endcount; i++)
                    {
                        Document doc = IDD[i];
                        sb.Append(" <div style=\"margin-top: 20px; margin-bottom: 20px;width:666px;\">");
                        sb.Append(" <div>");
                        sb.Append(" <a target=\"_blank\"  href=\"" + doc.Get("url").ToString() + "\"><font color=blue>" + doc.Get("title").ToString().Replace(searchStr, "<font color=red>" + searchStr + "</font>") + "</font></a>");
                        sb.Append(" </div>");
                        sb.Append(" <div style=\"font-size: 13px;\">");
                        sb.Append(" <font size=2 >" + (doc.Get("body").ToString().Length > 100 ? doc.Get("body").ToString().Substring(0, 100) + "..." : doc.Get("body").ToString()).Replace(searchStr, "<font color=red>" + searchStr + "</font>") + "</font>");
                        sb.Append(" </div>");
                        sb.Append(" <div>");
                        string datetime = "";
                        try
                        {
                            datetime = Convert.ToDateTime(doc.Get("publish_time")).ToString("yyyy-MM-dd HH:mm");
                        }
                        catch
                        {
                            datetime = doc.Get("publish_time").ToString();
                        }
                        sb.Append(" <font color=#006600>" + (doc.Get("url").ToString().Length > 66 ? doc.Get("url").ToString().Substring(0, 66) : doc.Get("url").ToString()) + "</font>&nbsp;&nbsp;<font size=2 color=#006600>" + datetime + "</font>&nbsp;&nbsp;<font size=2  color=#666699>[" + doc.Get("site").ToString() + "]</font>");
                        sb.Append("  </div>");
                        sb.Append(" </div>");
                    }
                    labResultList.Text = sb.ToString();

                    totalCount = IDD.Count;
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

        private Hits GetHitsByFiled(string filedname,Analyzer analyzer, string searchStr, IndexSearcher searcher)
        {
            MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_CURRENT, new string[] { filedname}, analyzer);
            Query query = parser.Parse(searchStr);
            Sort sort = new Sort();
            SortField f = new SortField("publish_time", SortField.STRING, true);//按照publish_time字段排序，true表示降序
            sort.SetSort(f);
            Hits hits = searcher.Search(query, sort);
            return hits;
        }
    }         
}
