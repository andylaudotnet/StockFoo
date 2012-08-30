<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="StockFoo.Web._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>股富财经搜索</title>
    <meta http-equiv="content-type" content="text/html;charset=utf-8">
    <meta name="keywords" content="股票财经搜索,股票,财经,搜索" />
    <meta  name="description" content="旨在提供股票财经搜索功能，为用户收集各种有用和及时股票财经信息，从而为用户赢得价值." />
    <link href="css/style.css" type="text/css" rel="Stylesheet" />
     <script language="javascript" src="js/indexmenu.js" type ="text/javascript"></script>
</head>
<body>
    <div id="div_login" style="text-align:right;margin-right:15px;margin-top:5px;"><%=loginString%></div>
    <div id="div_logo">
        <a href="/"></a>
    </div>
    <div id="div_search">
        <form action="search1.aspx" method="get">
        <input type="text" class="text_search" id="text_search" name="w" /><input type="submit"  value="搜 索" class="btn_search" />
        </form>
    </div>
    <center>
    <div id="tb" style="width:503px; text-align:left;margin-right:25px;"></div>
     <div  style="margin-top:11px;moverflow:hidden;height:20px;width:518px;border:0px solid #999; text-align:left;font-weight: bold;color: #cc0000;">最新资讯：</div>
     <iframe src='html/TopStockInfo.html' width='516px' height="145px" scrolling="no" frameborder="0" marginwidth="0" marginheight="0"></iframe>
    </center>
    <div  style="margin-top:45px;"></div>
    <script language="javascript" src="js/footer.js" type ="text/javascript"></script>
    <div style="display:none;"><script src="http://s11.cnzz.com/stat.php?id=2100046&web_id=2100046&show=pic" language="JavaScript"></script></div>
    <div style="float:left;position:absolute;left:10px; top:10px; width:50px;height:99px;"><a href="http://www.CnInterface.com/"><img border="0" src="images/mobile.png" alt="手机短信在线发送" /></a></div>
</body>
</html>
