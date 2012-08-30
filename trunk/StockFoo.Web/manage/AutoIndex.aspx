<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoIndex.aspx.cs" Inherits="StockFoo.Web.manage.AutoIndex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>股富财经搜索</title>
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <meta name="keywords" content="股票财经搜索,股票,财经,搜索" />
    <link href="../css/style.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="div_logo"><a href="/"></a></div>
    <center>
    <iframe src='AutoCatchArticle.aspx?id=<%=catchid%>' width='460px' height="232px" scrolling="no" frameborder="0" marginwidth="0" marginheight="0"></iframe>
    <table border="0" align="center" cellpadding="0" cellspacing="0" width="666px"><tr><td align="center"><font color="#666699" size="2">提示：抓取过程中出现页面无法访问情况请刷新本页即可。</font></td></tr></table>
    <script language="javascript" src="../js/footer.js" type ="text/javascript"></script>
    <div style="display:none;"><script src="http://s11.cnzz.com/stat.php?id=2100046&web_id=2100046&show=pic" language="JavaScript"></script></div>
    </center>
    </div>
    </form>
</body>
</html>
