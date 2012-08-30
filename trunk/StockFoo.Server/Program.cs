namespace StockFoo.Server
{
    using System;
    using System.Collections.Generic;
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
    using StockFoo.Catch;

    internal class Program
    {
        private static void Main()
        {
            for (; ; )
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;

                    var dac = DataAccess.Create(
                        DataAccess.CreateConnection(
                            "server=www.beta-1.cn,1435;database=s446669db0;uid=s446669db0;pwd=B71E8A0397FC;persist security info=true;",
                            DataAccess.DataProvider.SqlServer));

                    const string sql = @"SELECT  *
FROM    sf_catch
WHERE   [enabled] = 1
        AND ( next_time IS NULL
              OR GETDATE() >= next_time
            )
ORDER BY id ASC";
                    var catchList = new List<CatchInfo>();
                    Console.Write("读取抓取定义列表...");
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
                    Console.WriteLine("{0}条.", catchList.Count);

                    foreach (var info in catchList)
                    {

                        Console.Write("检查NextTime...");
                        try
                        {
                            dac.Connection.Open();
                            //检查catch得next_time
                            var checkNextTime = Convert.ToBoolean(dac.ExecuteScalar(DataAccess.ConnManagementMode.Manual, false,
                                                string.Format(
                                                    @"SELECT  CASE WHEN EXISTS ( SELECT   1
                           FROM     sf_catch
                           WHERE    id = {0}
                                    AND ( next_time IS NULL
                                          OR GETDATE() >= next_time
                                        ) ) THEN 'True'
             ELSE 'False'
        END", info.Id)));
                            if (checkNextTime)
                                Console.WriteLine("正常");
                            else
                            {
                                Console.WriteLine("跳过");
                                continue;
                            }


                            Console.Write("更新NextTime...");
                            //修改catch得next_time
                            dac.ExecuteNonQuery(DataAccess.ConnManagementMode.Manual, false,
                                                string.Format(
                                                    @"UPDATE  sf_catch
SET     next_time = DATEADD(SECOND, {0}, GETDATE())
WHERE   id = {1}",
                                                    info.TimeSpan, info.Id));
                            Console.WriteLine("成功.");
                        }
                        catch
                        {
                            Console.WriteLine("失败.");
                        }
                        finally
                        {
                            if (dac.Connection.State != ConnectionState.Closed)
                                dac.Connection.Close();
                        }

                        var arts = new List<Article>();

                        //获取新闻目录
                        var wc = new WebClient { Encoding = Encoding.GetEncoding(info.SiteEncode) };
                        var html = wc.DownloadString(info.CatalogUrl);

                        //HTML Parser
                        var doc = new HtmlDocument();
                        doc.LoadHtml(html);

                        //如果Catalog Regex为空则XPath能准确定位, 那么使用SelectNodes取得URL集合
                        if (info.CatalogRegex.Trim().Equals(""))
                        {
                            var ulnodes = doc.DocumentNode.SelectNodes(info.CatalogXPath);
                            var counter = 0;
                            for (int i = 1; i <= ulnodes.Count; i++)
                            {
                                var nodes = doc.DocumentNode.SelectNodes(info.CatalogXPath + "[" + i.ToString() + "]/li/a");
                                foreach (var node in nodes)
                                {
                                    Console.Write("解析_{2}: {0}/{1}...", ++counter, nodes.Count, info.CatchName);
                                    try
                                    {
                                        arts.Add(GetArticleEntity(wc, node.Attributes["href"].Value, info.ArticleTitleXPath, info.ArticleTitleRegex, info.ArticleTimeXPath, info.ArticleTimeRegex, info.ArticleTimeFormat, info.ArticleXPath, info.ArticleRegex, info.SiteName));
                                    }
                                    catch
                                    {
                                        Console.WriteLine("失败.");
                                        continue;
                                    }

                                    Console.WriteLine("成功.");
                                }
                            }
                        }
                        else
                        {
                            //XPath无法准确定位, 使用Regex加以处理
                          var ulnodes = doc.DocumentNode.SelectNodes(info.CatalogXPath);
                          var counter = 0;
                          for (int i = 1; i <= ulnodes.Count; i++)
                          {
                              var mc = new Regex(info.CatalogRegex).Matches(doc.DocumentNode.SelectSingleNode(info.CatalogXPath + "[" + i.ToString() + "]").InnerHtml);
                              foreach (Match m in mc)
                              {
                                  Console.Write("解析_{2}: {0}/{1}...", ++counter, mc.Count, info.CatchName);
                                  if (!m.Success || m.Groups.Count < 2)
                                  {
                                      Console.WriteLine("失败.");
                                      continue;
                                  }
                                  try
                                  {
                                      arts.Add(GetArticleEntity(wc, m.Groups[1].Value, info.ArticleTitleXPath, info.ArticleTitleRegex, info.ArticleTimeXPath, info.ArticleTimeRegex, info.ArticleTimeFormat, info.ArticleXPath, info.ArticleRegex, info.SiteName));
                                  }
                                  catch
                                  {
                                      Console.WriteLine("失败.");
                                      continue;
                                  }

                                  Console.WriteLine("成功.");

                              }
                          }
                        }



                        //写入数据库
                        try
                        {
                            dac.Connection.Open();
                            var counter = 0;
                            foreach (var art in arts)
                            {
                                Console.Write("入库_{2}: {0}/{1}...", ++counter, arts.Count, info.CatchName);
                                Console.WriteLine("{0}条.", dac.ExecuteNonQuery(DataAccess.ConnManagementMode.Manual, false,
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
                                                    dac.CreateParameter("@display_order", 0)));
                            }
                        }
                        catch (Exception expAll)
                        {
                            Console.WriteLine(expAll.Message);
                            continue;
                        }
                        finally
                        {
                            if (dac.Connection.State != ConnectionState.Closed)
                                dac.Connection.Close();
                        }
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message);
                    continue;
                }

                //休息10s
                Thread.Sleep(10 * 1000);
            }


        }

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
        private static Article GetArticleEntity(WebClient wc, string url, string atXp, string atRgx, string aptXp, string aptRgx, string aptF, string bodyXp, string bodyRgx, string site)
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
    }
}
