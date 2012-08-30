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
    <iframe src='AutoCatchArticle.aspx?id=<%=catchid%>' width='460px' height="220px" scrolling="no" frameborder="0" marginwidth="0" marginheight="0"></iframe>
    <script language="javascript" src="../js/footer.js" type ="text/javascript"></script>
    <script  src="http://s11.cnzz.com/stat.php?id=2100046&web_id=2100046&show=pic" language="JavaScript"></script>
    </center>
    </div>
    </form>
</body>
</html>
