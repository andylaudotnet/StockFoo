<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoCatchArticle.aspx.cs" Inherits="StockFoo.Web.manage.AutoCatchArticle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title>股富财经搜索</title>
    <meta http-equiv="content-type" content="text/html;charset=utf-8">
    <meta name="keywords" content="股票财经搜索,股票,财经,搜索" />
     <link href="../css/style.css" type="text/css" rel="Stylesheet" />
     <script language="javascript">
         function goback() {
             window.parent.location.href = "CatchIndex.aspx";
         }
         function goindexbuilde() {
             window.parent.location.href = "AnalyzerIndex.aspx";
         }
     </script>
</head>
<body>
    <form id="form1" runat="server">
     <table  border="0" cellpadding ="6" cellspacing ="0"  align="left"  width="456px">
                   <tr height="20px"><td>&nbsp;</td></tr>
                   <tr  height="32">
                            <td align="center">
                                     <div id="div_load" runat="server"> 
                                        <table width="320" height="72" border="0" bordercolor="#cccccc" cellpadding="5" cellspacing="1" 
                                         class="font" style="FILTER: Alpha(opacity=80); WIDTH: 320px; HEIGHT: 72px" align="center"> 
                                         <tr> 
                                          <td align="center"> 
                                            <asp:Label id="lab_state" runat="server"></asp:Label></p> 
                                          </td> 
                                         </tr> 
                                        </table> 
                                   </div> 
                            </td>
                   </tr>
                   <tr>
                            <td align="center">
                             <asp:Button id="btn_startwork" runat="server" Text="点击开始抓取" 
                                                onclick="btn_startwork_Click"  CssClass="ButtonCss"></asp:Button>&nbsp;&nbsp;&nbsp;<a href="#" onclick="goback();"><font size=2>返回</font></a><BR> 
                            </td>
                   </tr>
                   <tr>
                            <td align="center">
                                    <asp:Label id="lab_jg" runat="server"></asp:Label>    
                            </td>
                   </tr>
        </table>
    </form>
    <div style="display:none"><script src="http://s11.cnzz.com/stat.php?id=2100046&web_id=2100046&show=pic" language="JavaScript"></script></div>
</body>
</html>
