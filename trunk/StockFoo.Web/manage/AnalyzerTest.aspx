<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnalyzerTest.aspx.cs" Inherits="StockFoo.Web.manage.AnalyzerTest" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
     <title>股富财经搜索</title>
    <meta http-equiv="content-type" content="text/html;charset=utf-8">
    <meta name="keywords" content="股票财经搜索,股票,财经,搜索" />
     <link href="../css/style.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="div_logo"><a href="/"></a></div>
    <div style="text-align:center;margin-top:33px;">
        <table width="600px" border="0" cellpadding="0" cellspacing="0">
            <tr>
                    <td align="left"><asp:Label ID="labAnalyzer" runat="server" Text="请在下面文本框中输入需要分词的语句："></asp:Label></td>
                    <td align="right"><a href="CatchIndex.aspx">返回</a></td>
            </tr>
        </table>
        <asp:TextBox ID="txtAnalyzer" runat="server" Height="121px"    TextMode="MultiLine" Width="598px"></asp:TextBox><br /><br />
        <asp:Button ID="btnAnalyzer" runat="server"  Text="分  词"  onclick="btnAnalyzer_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnBlank" runat="server"  Text="清  空" onclick="btnBlank_Click"  /><br /><br />
        <table width="600px" border="0" cellpadding="0" cellspacing="0">
            <tr>
            <td align="left"><asp:Label ID="labResult" runat="server"></asp:Label></td>
            </tr>
        </table>
    </div>
         <div  style="display:none"><script  src="http://s11.cnzz.com/stat.php?id=2100046&web_id=2100046&show=pic" language="JavaScript"></script></div>
    </form>
</body>
</html>
