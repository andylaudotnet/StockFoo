<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="StockFoo.Web.search" %>
<%@ Import Namespace="StockFoo.Entity" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>股富财经搜索</title>
    <meta http-equiv="content-type" content="text/html;charset=utf-8">
    <meta name="keywords" content="股票财经搜索,股票,财经,搜索" />
    <link href="css/style.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <div id="div_searchresult">
        <form action="search1.aspx" method="get">
        <a href="default.aspx"><img src="images/logo.png" border="0"/></a><input type="text" class="text_search" id="text_search" name="w" value="<%=SearchWords%>"/><input type="submit"  value="搜 索" class="btn_search" />
        </form>
    </div>
    <%
        if (SearchResults != null)
        {
    %>
    <div id="div_resultinfo">
        <table  border="0" cellpadding ="0" cellspacing ="3" width="100%" bgcolor="#edf8f8">
            <tr>
                <td align="left">
                    <a href ="#"  onclick="window.external.AddFavorite(location.href, document.title);" style="color:Black;">把StockFoo.com收藏起来</a>
                </td>
                <td align="right">共搜索到了<%= TotalCount %>条记录,用时<%=SearchTime%>秒.</td>
            </tr>
      </table>
    </div>
    <div style="width: 666px;">
         <!--<div id="float_div" style="Z-INDEX:1002;position:absolute;left:310px;top:305px;"><img src ="images/jinghua.gif" border=\"0\" /></div>-->
        <div style="margin-top: 10px;"><%=articleclassinfo%></div>
        <%
            if (SearchResults.Count > 0)
            {
                foreach (var art in SearchResults)
                {
        %>
        <div style="margin-top: 10px; margin-bottom: 20px;">
            <div>
                <a href="<%= art.Url %>" target="_blank">
                <%
                    if (SearchWords.Length > 0)
                    {
                  %>   
                    <%= art.Title.Replace(SearchWords, "<span style=\"color:red;\">" + SearchWords + "</span>")%></a>
                 <%
                    }
                    else
                    { 
                  %>  
                    <%= art.Title%></a>
                 <%    
                    }
                  %>
                <span style="color: #999; font-size: 12px;">[<%= art.Site%>]</span></div>
                <div style="font-size: 13px;">
                    <%
                    if (SearchWords.Length > 0)
                    {
                  %>   
                    <%= art.Body.Replace(SearchWords, "<span style=\"color:red;\">" + SearchWords + "</span>")%></a>
                 <%
                    }
                    else
                    { 
                  %>  
                    <%= art.Body%></a>
                 <%    
                    }
                  %>
                </div>
                <div>
                    <span style="color: Green; font-size: 12px;">
                        <%= art.Url%>&nbsp;&nbsp;
                        <%= art.PublishTime.ToString("yyyy年MM月dd日 HH:mm")%>
                        </span></div>
            </div>
        <%
               }
            }
            else
            { 
         %>  
         <br/>抱歉，没有找到“<%=SearchWords%>”相关的内容。<br/><br/><b>建议您：</b> <br/>1、看看输入的文字是否有误<br/>2、去掉可能不必要的字词，如“的”、“什么”等<br/>3、<a href="http://www.haopingba.com/Page1Result.aspx?web=aall&searchword=<%=SearchWords%>" target="_blank">去此处查询</a>
         <%
            }
        %>
        <div id="div_resultpages" style="width: 888px;"><%=PageInfo%></div>
    </div>
    <%
        }
   else
        { 
    %>  
         <br/>抱歉，没有找到“<%=SearchWords%>”相关的内容。<br/><br/><b>建议您：</b> <br/>1、看看输入的文字是否有误<br/>2、去掉可能不必要的字词，如“的”、“什么”等<br/>3、<a href="http://www.haopingba.com/Page1Result.aspx?web=aall&searchword=<%=SearchWords%>" target="_blank">去此处查询</a>
    <%
         }
   %>
    <div id="div_resultpoint">©2010 StockFoo.com 此内容系股富财经搜索网站根据您的指令自动搜索的结果，不代表股富财经搜索网站赞成被搜索网站的内容或立场.</div>
    <div style="display:none;"><script src="http://s11.cnzz.com/stat.php?id=2100046&web_id=2100046&show=pic" language="JavaScript"></script></div>
</body>
</html>
