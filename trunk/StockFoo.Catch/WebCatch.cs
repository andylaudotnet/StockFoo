
namespace StockFoo.Catch
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using Dac;
    using Entity;
    using HtmlAgilityPack;
    using CatchInfo = Entity.Catch;
    using System.Threading;
    using System.IO;
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Store;
    using StockFoo.Analyzer;

    public class WebCatch
    {
          public int State = 0;//0-没有开始,1-正在运行,2-成功结束,3-失败结束 
          public int totalcatchartend = 0;//抓取Article记录完成数量
          public int totalinsertdbend = 0;//插入数据库Article记录完成数量
          public int totalindexend = 0;//创建Article全文索引完成数量
          public int totalupdatedbend = 0;//更新数据库Article记录完成数量
          public DateTime StartTime;//开始时间
          public DateTime FinishTime;//结束时间
          public DateTime ErrorTime; //出错时间
          public string ParseLogicstr="";//抓取日志
          private int _catchid = 0;

          public WebCatch() { }
          public WebCatch(int id) { this._catchid = id; }

          public void JustDoing()
          {
              lock (this)
              {
                  if (State != 1)
                  {
                      State = 1;
                      StartTime = DateTime.Now;
                      System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(GetArticleByCatch));
                      thread.Start();
                  }
              }
          }

        #region 抓取Article数据方法
        /// <summary>
        ///抓取Article数据方法
        /// </summary>
        public void GetArticleByCatch()
        {
            try
            {
                var dac = DataAccess.Create(DataAccess.CreateConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString, DataAccess.DataProvider.SqlServer));
                var catchList = GetCatchList(dac, _catchid);//获取Catch列表
                foreach (var info in catchList)
                {
                    ParseLogicstr = info.CatchName;
                    try
                    {
                        //获取文章目录
                        var wc = new WebClient { Encoding = Encoding.GetEncoding(info.SiteEncode) };
                        var html = wc.DownloadString(info.CatalogUrl);
                        //HTML Parser
                        var doc = new HtmlDocument();
                        doc.LoadHtml(html);

                        //如果Catalog Regex为空则XPath能准确定位, 那么使用SelectNodes取得URL集合
                        try
                        {
                            dac.Connection.Open();
                            if (info.CatalogRegex.Trim().Equals(""))
                            {
                                var ulnodes = doc.DocumentNode.SelectNodes(info.CatalogXPath);
                                for (int i = 1; i <= ulnodes.Count; i++)
                                {
                                    var nodes = doc.DocumentNode.SelectNodes(info.CatalogXPath + "[" + i.ToString() + "]/li/a");
                                    foreach (var node in nodes)
                                    {
                                        try
                                        {
                                            Article art = GetArticleEntity(wc, node.Attributes["href"].Value, info.ArticleTitleXPath, info.ArticleTitleRegex, info.ArticleTimeXPath, info.ArticleTimeRegex, info.ArticleTimeFormat, info.ArticleXPath, info.ArticleRegex, info.SiteName);
                                            if (art != null)
                                            {
                                                totalcatchartend++;
                                                int insertcount = InsertIntoArticle(dac, art, info);//插入数据库
                                                if (insertcount == 1)
                                                {
                                                    totalupdatedbend++;
                                                }
                                                if (insertcount == 2)
                                                {
                                                    totalinsertdbend++;
                                                    totalindexend += CreateArticleIndex(art);
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }
                                    Thread.Sleep(100);
                                }
                            }
                            else//XPath无法准确定位, 使用Regex加以处理
                            {
                                var ulnodes = doc.DocumentNode.SelectNodes(info.CatalogXPath);
                                for (int i = 1; i <= ulnodes.Count; i++)
                                {
                                    var mc = new Regex(info.CatalogRegex).Matches(doc.DocumentNode.SelectSingleNode(info.CatalogXPath + "[" + i.ToString() + "]").InnerHtml);
                                    foreach (Match m in mc)
                                    {
                                        if (!m.Success || m.Groups.Count < 2)
                                        {
                                            continue;
                                        }
                                        try
                                        {
                                            Article art = GetArticleEntity(wc, m.Groups[1].Value, info.ArticleTitleXPath, info.ArticleTitleRegex, info.ArticleTimeXPath, info.ArticleTimeRegex, info.ArticleTimeFormat, info.ArticleXPath, info.ArticleRegex, info.SiteName);
                                            if (art != null)
                                            {
                                                totalcatchartend++;
                                                int insertcount = InsertIntoArticle(dac, art, info);//插入数据库
                                                if (insertcount == 1)
                                                {
                                                    totalupdatedbend++;
                                                }
                                                if (insertcount == 2)
                                                {
                                                    totalinsertdbend++;
                                                    totalindexend += CreateArticleIndex(art);
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }
                                    Thread.Sleep(100);
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                        finally
                        {
                            if (dac.Connection.State != ConnectionState.Closed)
                                dac.Connection.Close();
                        }
                        Thread.Sleep(1000);
                    }
                    catch
                    {
                        continue;
                    }
                }

                State = 2;
            }
            catch
            {
                ErrorTime = DateTime.Now;
                State = 3;
            }
            finally
            {
                FinishTime = DateTime.Now;
            }
        }
        #endregion

        #region  获取Catch列表
        /// <summary>
        /// 获取Catch列表
        /// </summary>
        /// <returns></returns>
         private  IList<CatchInfo> GetCatchList(DataAccess dac,int id)
        {
                   string sql ="SELECT  * FROM  sf_catch WHERE   [enabled] = 1 and id="+id;
                   if (id == 0)
                   {
                       sql = "SELECT  * FROM  sf_catch WHERE   [enabled] = 1";
                   }
                    var catchList = new List<CatchInfo>();
                    using (IDataReader dr = dac.ExecuteReader(DataAccess.ConnManagementMode.Auto, sql))
                    {
                        while (dr.Read())
                        {
                            catchList.Add(new CatchInfo
                            {
                                ArticleTimeFormat = dr["article_time_format"] as string,
                                ArticleTimeRegex = dr["article_time_regex"] as string,
                                ArticleTimeXPath = dr["article_time_xpath"] as string,
                                ArticleTitleXPath = dr["article_title_xpath"] as string,
                                ArticleTitleRegex = dr["article_title_regex"] as string,
                                ArticleXPath = dr["article_xpath"] as string,
                                ArticleRegex = dr["article_regex"] as string,
                                CatalogUrl = dr["catalog_url"] as string,
                                CatalogXPath = dr["catalog_xpath"] as string,
                                CatalogRegex = dr["catalog_regex"] as string,
                                ClassId = (int)dr["classid"],
                                Guid = new Guid(dr["guid"].ToString()),
                                Id = (int)dr["id"],
                                SiteEncode = (int)dr["site_encode"],
                                SiteName = dr["site_name"] as string,
                                Enabled = (bool)dr["enabled"],
                                CatchName = dr["catch_name"] as string,
                                NextTime = dr["next_time"] as DateTime?,
                                TimeSpan = (int)dr["timespan"]
                            });
                        }
                    }
            return catchList;
        }
       #endregion

        #region 通过URL获取Article对象.
        /// <summary>
        /// 通过URL获取Article对象.
        /// </summary>
        /// <param name="wc">WebClient 对象.</param>
        /// <param name="url">文章URL.</param>
        /// <param name="atXp">文章标题 XPath.</param>
        /// <param name="atRgx">文章标题 Regex.</param>
        /// <param name="aptXp">文章发布时间 XPath.</param>
        /// <param name="aptRgx">文章发布时间 Regex.</param>
        /// <param name="aptF">文章发布时间 Format.</param>
        /// <param name="bodyXp">文章内容 XPath.</param>
        /// <param name="bodyRgx">文章内容 Regex.</param>
        /// <param name="site">所属站点名称.</param>
        /// <returns>Article对象.</returns>
         private  Article GetArticleEntity(WebClient wc, string url, string atXp, string atRgx, string aptXp, string aptRgx, string aptF, string bodyXp, string bodyRgx, string site)
        {
            try
            {
                var html = wc.DownloadString(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                //取文章标题
                var title = doc.DocumentNode.SelectSingleNode(atXp).InnerHtml;
                if (!atRgx.Equals(""))
                    title = new Regex(atRgx).Match(title).Groups[1].Value;

                //取文章发布时间
                var ptStr = doc.DocumentNode.SelectSingleNode(aptXp).InnerHtml;
                if (!aptRgx.Equals(""))
                    ptStr = new Regex(aptRgx).Match(ptStr).Groups[1].Value;
                //避免与系统format定义冲突
                ptStr = ptStr.Replace("s", "S");
                var pt = DateTime.ParseExact(ptStr, aptF, CultureInfo.CurrentCulture);

                //取文章内容
                var body = doc.DocumentNode.SelectSingleNode(bodyXp).InnerHtml;
                if (!bodyRgx.Equals(""))
                    title = new Regex(bodyRgx).Match(body).Groups[1].Value;
                //去除html标记
                body = new HtmlToText().ConvertHtml(body).Trim();
                //body=body.Length > 200 ? body.Substring(0, 200) : body;
                return new Article
                {
                    Body = body,
                    CreateTime = DateTime.Now,
                    Guid = Guid.NewGuid(),
                    PublishTime = pt,
                    Title = title,
                    Url = url,
                    Site = site
                };
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region  往表sf_Article插入数据
         private  int InsertIntoArticle(DataAccess dac, Article art, Catch info)
        {
           //插入数据库
           var counter = 0;
            try
            {
                     counter = dac.ExecuteNonQuery(DataAccess.ConnManagementMode.Manual, false,
                                        @"DECLARE @article_id INT
SELECT  @article_id = id
FROM    dbo.sf_Article
WHERE   url = @url
        OR title = @title
IF @article_id IS NULL 
    BEGIN
        INSERT  INTO [sf_Article]
                ( [guid] ,
                  [title] ,
                  [body] ,
                  [site] ,
                  [url] ,
                  [publish_time] ,
                  [create_time]
                )
        VALUES  ( @guid ,
                  @title ,
                  @body ,
                  @site ,
                  @url ,
                  @publish_time ,
                  @create_time
                )
        SET @article_id = SCOPE_IDENTITY()
    END
ELSE
 BEGIN
        UPDATE  [sf_Article]
                  SET [guid]=@guid ,                                       
                  [title]=@title ,
                  [body] =@body ,
                  [site] =@site ,
                  [url]=@url ,
                  [publish_time] =@publish_time ,
                  [create_time]=@create_time
        WHERE id=@article_id
END
IF NOT EXISTS ( SELECT  1
                FROM    dbo.sf_Map_ArtCls
                WHERE   article_id = @article_id
                        AND class_id = @class_id ) 
    BEGIN
        INSERT  INTO [sf_Map_ArtCls]
                ( [guid] ,
                  [article_id] ,
                  [class_id] ,
                  [display_order]
                
                )
        VALUES  ( @guid2 ,
                  @article_id ,
                  @class_id ,
                  @display_order
                
                )
    END
                 ",
                                        dac.CreateParameter("@guid", art.Guid), dac.CreateParameter("@title", art.Title),
                                        dac.CreateParameter("@body", art.Body), dac.CreateParameter("@site", art.Site),
                                        dac.CreateParameter("@url", art.Url), dac.CreateParameter("@publish_time", art.PublishTime),
                                        dac.CreateParameter("@create_time", art.CreateTime),
                                        dac.CreateParameter("@guid2", Guid.NewGuid()), dac.CreateParameter("@class_id", info.ClassId),
                                        dac.CreateParameter("@display_order", 0));
            }
            catch
            {
                counter = 0;
            }
            if (counter < 0)
                counter = 0;
            return counter;
        }
        #endregion

         #region 将Article数据对象建立全文索引
         private int CreateArticleIndex(Article art)
        {
            int createcount=0;
            try
            {
                StockFooAnalyzer analyzer = new StockFooAnalyzer(System.Configuration.ConfigurationManager.AppSettings["AnalyzerPath"].ToString());

                FSDirectory dy = FSDirectory.Open(new DirectoryInfo(System.Configuration.ConfigurationManager.AppSettings["IndexDirectory"].ToString()));
                IndexWriter writer = new IndexWriter(dy, analyzer, false, IndexWriter.MaxFieldLength.LIMITED);
                AddDocument(writer, art.Title, art.Url, art.Site, art.Body.Length>300?art.Body.Substring(0,300):art.Body, art.PublishTime.ToString());
                writer.Optimize();
                writer.Close();
                createcount = 1;
            }
            catch
            {
                createcount = 0;
            }
            return createcount;
        }
       #endregion

         private void AddDocument(IndexWriter writer, string title, string url, string site, string body, string publish_time)
         {
             Document document = new Document();
             Field ftitle = new Field("title", title, Field.Store.YES, Field.Index.ANALYZED);
             document.Add(ftitle);//存储，索引
             document.Add(new Field("url", url, Field.Store.YES, Field.Index.NOT_ANALYZED));//存储，不索引
             document.Add(new Field("site", site, Field.Store.YES, Field.Index.NOT_ANALYZED));//存储，不索引
             Field fbody = new Field("body", body, Field.Store.YES, Field.Index.ANALYZED);
             document.Add(fbody);//存储，索引
             document.Add(new Field("publish_time", publish_time, Field.Store.YES, Field.Index.NOT_ANALYZED));//存储，不索引
             writer.AddDocument(document);
         }
    }
}
