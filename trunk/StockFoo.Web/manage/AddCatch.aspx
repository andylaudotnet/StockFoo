<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCatch.aspx.cs" Inherits="StockFoo.Web.manage.AddCatch"   ValidateRequest="false" %>

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
    <div>
        <table  border="0" cellpadding ="0" cellspacing ="0"  align="center" width="666px"><tr><td align="left"><a href="../default.aspx"><img border="0"  src="../images/logo.png"/></a></td></tr></table>
        <table  border="0" cellpadding ="4" cellspacing ="1"  bgcolor="#9999ff" align="center"  width="666px" >
            <tr>
                <td bgcolor="#ffffff" colspan="2" style="background-image:url(../images/menu.jpg) "><b>添加抓取配置：</b></td>
            </tr>
            <tr>
                <td bgcolor="#ffffff"><font size=2>抓取配置名称：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtCatchName"  runat="server"  Width="343px" BackColor="#ffffff" ></asp:TextBox><font color="red" size="2">*</font></td>
            </tr>
             <tr>
                <td bgcolor="#ffffff"><font size=2>站点名称：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtSiteName"  runat="server"  Width="343px" BackColor="#ffffff"    ></asp:TextBox><font color="red" size="2">*</font></td>
            </tr>
             <tr>
                <td bgcolor="#ffffff"><font size=2>站点编码：</font></td>
                <td bgcolor="#ffffff">
                        <asp:DropDownList ID="ddlSiteEncode" runat="server">
                            <asp:ListItem Value="65001">UTF-8</asp:ListItem>
                            <asp:ListItem Value="936">gb2312</asp:ListItem>
                            <asp:ListItem Value="950">big5</asp:ListItem>
                        </asp:DropDownList><font color="red" size="2">*</font></td>
            </tr>
            <tr>
                <td bgcolor="#ffffff" ><font size=2>文章类别：</font></td>
                <td bgcolor="#ffffff">
                         <asp:DropDownList ID="ddlClassID" runat="server">                         
                         </asp:DropDownList><font color="red" size="2">*</font>
                </td>
            </tr>
            <tr>
                <td bgcolor="#ffffff"><font size=2>文章目录地址：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtCatalogUrl"  runat="server"  Width="500px" BackColor="#ffffff"    ></asp:TextBox></td>
            </tr>
            <tr>
                <td bgcolor="#ffffff"><font size=2>文章目录Xpath：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtCatalogXPath"  runat="server"  Width="500px" BackColor="#ffffff"    ></asp:TextBox></td>
            </tr>
            <tr>
                <td bgcolor="#ffffff"><font size=2>文章目录正则：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtCatalogRegex"  runat="server"  Width="500px" BackColor="#ffffff"    ></asp:TextBox></td>
            </tr>
             <tr>
                <td bgcolor="#ffffff"><font size=2>文章标题Xpath：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtArticleTitleXPath"  runat="server"  Width="500px" BackColor="#ffffff"    ></asp:TextBox></td>
            </tr>
             <tr>
                <td bgcolor="#ffffff"><font size=2>文章标题正则：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtArticleTitleRegex"  runat="server"  Width="500px" BackColor="#ffffff"    ></asp:TextBox></td>
            </tr>
             <tr>
                <td bgcolor="#ffffff"><font size=2>文章内容Xpath：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtArticleXPath"  runat="server"  Width="500px" BackColor="#ffffff"    ></asp:TextBox></td>
            </tr>
             <tr>
                <td bgcolor="#ffffff"><font size=2>文章内容正则：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtArticleRegex"  runat="server"  Width="500px" BackColor="#ffffff"    ></asp:TextBox></td>
            </tr>
            <tr>
                <td bgcolor="#ffffff"><font size=2>文章发布时间Xpath：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtArticleTimeXPath"  runat="server"  Width="500px" BackColor="#ffffff"    ></asp:TextBox></td>
            </tr>
            <tr>
                <td bgcolor="#ffffff"><font size=2>文章发布时间正则：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtArticleTimeRegex"  runat="server"  Width="500px" BackColor="#ffffff"    ></asp:TextBox></td>
            </tr>
            <tr>
                <td bgcolor="#ffffff"><font size=2>文章发布时间格式：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtArticleTimeFormat"  runat="server"  Width="500px" BackColor="#ffffff"    ></asp:TextBox></td>
            </tr>
            <tr>
                <td bgcolor="#ffffff"><font size=2>是否启用：</font></td>
                <td bgcolor="#ffffff">
                    <asp:RadioButtonList ID="rblEnabled" runat="server"  RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Selected="True">启用</asp:ListItem>
                        <asp:ListItem Value="0">停用</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td bgcolor="#ffffff"><font size=2>下次抓取时间：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtNextTime"  runat="server"  Width="150px" BackColor="#ffffff"  ></asp:TextBox><font color="red" size="2">*</font>(格式:2010-04-20 15:30:25)</td>
            </tr>
            <tr>
                <td bgcolor="#ffffff"><font size=2>抓取时间间隔：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtTimespan"  runat="server"  Width="150px" BackColor="#ffffff"   Text="0"  ></asp:TextBox><font color="red" size="2">*</font>(单位:秒)</td>
            </tr>
             <tr>
                <td bgcolor="#ffffff" colspan="2" align="center">
                     <asp:Button    ID="bntAddCatch"  runat="server" 
                         Text="添&nbsp;加" onclick="bntAddCatch_Click"  Width="70px"/>&nbsp;&nbsp;&nbsp;<a href="CatchIndex.aspx"><font size=2>返回</font></a>
                 </td>
            </tr>
        </table>
    </div>
    </form>
     <script language="javascript" src="../js/footer.js" type ="text/javascript"></script>
     <div style="display:none;"><script src="http://s11.cnzz.com/stat.php?id=2100046&web_id=2100046&show=pic" language="JavaScript"></script></div>
</body>
</html>
