using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StockFoo.Entity;
using StockFoo.Dac;
using System.Configuration;
using System.Data;
using System.Text;

namespace StockFoo.Web
{
	public partial class search : System.Web.UI.Page
	{
		DataAccess Dac = DataAccess.Create(DataAccess.CreateConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString, DataAccess.DataProvider.SqlServer));
		protected List<Article> SearchResults;
		protected string SearchWords="";
		protected int TotalCount;
		protected int TotalPage;
		protected int PageSize;
		protected int PageIndex;
        protected string SearchTime = "0.000";
        protected string PageInfo;
        protected string articleclassinfo = "";
        protected string jinghuaimg = "";

		protected void Page_Load(object sender, EventArgs e)
		{
            DateTime dt1 = DateTime.Now;
			SearchWords = Request.QueryString["w"];

            if ((string.IsNullOrEmpty(SearchWords) && Request["cla"] == null) || (string.IsNullOrEmpty(SearchWords) && Convert.ToInt32(Request["cla"]) == 0))
			{
				Response.Redirect("default.aspx");
				return;
			}
			if (!int.TryParse(Request.QueryString["ps"], out PageSize)) PageSize = 10;
			if (!int.TryParse(Request.QueryString["pi"], out PageIndex)) PageIndex = 1;

            int artclass = 0;
            if (Request["cla"] != null && Convert.ToInt32(Request["cla"]) > 0)
            {
                artclass = Convert.ToInt32(Request["cla"]);
                SearchWords = "";
                articleclassinfo = GetClassNameByID(artclass);
                if (PageIndex < 3)
                {
                    jinghuaimg = "<img src =\"images/jinghua.gif\" border=\"0\" />";
                }
            }
            SearchResults = Search(artclass, SearchWords, PageSize, PageIndex, out TotalPage, out TotalCount);

            DateTime dt2 = DateTime.Now;
            TimeSpan ts = dt2 - dt1;
            double timeuse = Convert.ToInt32(ts.TotalMilliseconds) * 0.001;
            SearchTime = timeuse.ToString("0.000");
		}

		private List<Article> Search(int articleclass,string searchWords, int pageSize, int pageIndex, out int totalPage, out int totalCount)
		{
			const string sql = @"
DECLARE @temp_search_article TABLE ( id INT, MATCH TINYINT )
--将搜索结果id,match保存表变量
  IF @ArticleClass =0
    BEGIN
        INSERT  @temp_search_article
        ( id ,
          MATCH
        )
        SELECT  a1.id ,
                a1.SUM_match
        FROM    ( SELECT    id ,
                            SUM(MATCH) AS SUM_match ,
                            MAX(publish_time) AS max_publish_time
                  FROM      ( SELECT   id ,
                                        10 AS MATCH ,
                                        publish_time
                              FROM      dbo.sf_Article
                              WHERE     title LIKE '%' + @searchwords + '%'
                              UNION ALL
                              SELECT      id ,
                                        9 AS MATCH ,
                                        publish_time
                              FROM      dbo.sf_Article
                              WHERE     body LIKE '%' + @searchwords + '%'
                            ) a
                  GROUP BY  id
                ) a1
        ORDER BY a1.SUM_match DESC ,
                a1.max_publish_time DESC
   END
  ELSE
   BEGIN
        INSERT  @temp_search_article
        ( id ,
          MATCH
        )
        SELECT  a1.id ,
                a1.SUM_match
        FROM    ( SELECT    id ,
                            SUM(MATCH) AS SUM_match ,
                            MAX(publish_time) AS max_publish_time
                  FROM      ( 
 SELECT id,10 AS MATCH ,publish_time FROM sf_Article WHERE id in(select article_id from sf_Map_ArtCls where class_id=@ArticleClass)
                            ) a
                  GROUP BY  id
                ) a1
        ORDER BY a1.SUM_match DESC ,
                a1.max_publish_time DESC
  END

SELECT  b.MATCH ,
        c.id,c.guid,c.title,(left(c.body,100)+'...') as body,c.site,c.url,c.publish_time,c.create_time
FROM    ( SELECT TOP {0}
                    *
          FROM      @temp_search_article b
          WHERE     id NOT IN ( SELECT TOP {1}
                                        id
                                FROM    @temp_search_article )
        ) b
        INNER JOIN dbo.sf_Article c ON b.id = c.id


-- 总的记录数
DECLARE @totalcount INT ;
-- 总的页数
DECLARE @totalpage INT ;

SELECT  @totalcount = COUNT(0)
FROM    @temp_search_article

SET @totalpage = @totalcount / {0}
IF @totalcount % {0} <> 0 
    SET @totalpage = @totalpage + 1 
		
SELECT  @totalcount AS totalcount ,
        @totalpage AS totalpage
";

			var searchResults = new List<Article>();
			using (IDataReader dr = Dac.ExecuteReader(DataAccess.ConnManagementMode.Auto, string.Format(sql, pageSize,
                pageSize * (pageIndex - 1)), Dac.CreateParameter("@searchwords", searchWords), Dac.CreateParameter("@ArticleClass", articleclass)))
			{

				while (dr.Read())
				{
					searchResults.Add(new Article { Body = dr["body"] as string, CreateTime = (DateTime)dr["create_time"], Guid = (Guid)dr["guid"], PublishTime = (DateTime)dr["publish_time"], Site = dr["site"] as string, Title = dr["title"] as string, Url = dr["url"] as string });
				}
				dr.NextResult();
				dr.Read();
				totalCount = (int)dr["totalcount"];
				totalPage = (int)dr["totalpage"];

                if (totalPage > 1)
                {
                    int artclass = 0;
                    if (Request["cla"] != null && Convert.ToInt32(Request["cla"]) > 0)
                    {
                        artclass = Convert.ToInt32(Request["cla"]);
                    }
                    StringBuilder sb = new StringBuilder();
                    string parame = "";
                    if (pageIndex > 1)
                    {
                        parame = "search.aspx?cla=" + artclass + "&pi=" + (pageIndex - 1).ToString() + "&w=" + System.Web.HttpUtility.UrlEncode(searchWords);
                        sb.Append("<a  href=\"" + parame + "\">上一页</a> ");
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
                            sb.Append(" "+pageIndex);
                        }
                        else
                        {
                            parame = "search.aspx?cla=" + artclass + "&pi=" + (i).ToString() + "&w=" + System.Web.HttpUtility.UrlEncode(searchWords);
                            sb.Append("<a href=\"" + parame + "\">[" + (i).ToString() + "]</a>");
                        }
                    }
                    if (pageIndex < totalPage)
                    {
                        parame = "search.aspx?cla=" + artclass + "&pi=" + (pageIndex + 1).ToString() + "&w=" + System.Web.HttpUtility.UrlEncode(searchWords);
                        sb.Append(" <a   href=\"" + parame + "\">下一页</a>");
                    }
                    PageInfo = sb.ToString();
                }
			}
			return searchResults;
		}

        private string GetClassNameByID(int id)
        {
            string classname = "";
            switch (id)
            { 
                case 1:
                    classname = "个股点金";
                    break;
                case 2:
                    classname = "证券头条";
                    break;
                case 3:
                    classname = "上市公司";
                    break;
                case 4:
                    classname = "牛博论市";
                    break;
                case 5:
                    classname = "大盘分析";
                    break;
                case 6:
                    classname = "股市直播";
                    break;
                case 7:
                    classname = "板块聚焦";
                    break;
                case 8:
                    classname = "数据研报";
                    break;
                case 9:
                    classname = "主力追踪";
                    break;
                default:
                    break;
            }
            return "您正在浏览的是<b>" + classname + "</b>相关信息：";
        }
	}
}
