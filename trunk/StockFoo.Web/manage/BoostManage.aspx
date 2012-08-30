<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BoostManage.aspx.cs" Inherits="StockFoo.Web.manage.BoostManage" %>

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
    <table  border ="0" cellpadding =0" cellspacing ="0" width ="666px" align="center">
            <tr>
               <td align="left"><a href="../default.aspx"><img border="0"  src="../images/logo.png"/></a></td>
               <td align="right" valign="bottom"></td>
            </tr>
            </table>
     <table  border="0" cellpadding ="4" cellspacing ="1"  bgcolor="#9999ff" align="center"  width="666px" >
        <tr>
            <td colspan="2" style="background-image:url(../images/menu.jpg) "><b>新建索引</b></td>
        </tr>
        <tr>
                <td bgcolor="#ffffff"><font size=2>title：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txttitle"  runat="server"  Width="533px" 
                        BackColor="#ffffff" ></asp:TextBox><font color="red" size="2">*</font></td>
         </tr>
         <tr>
                <td bgcolor="#ffffff"><font size=2>url：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txturl"  runat="server"  Width="533px" 
                        BackColor="#ffffff" ></asp:TextBox><font color="red" size="2">*</font></td>
         </tr>
         <tr>
                <td bgcolor="#ffffff"><font size=2>site：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtsite"  runat="server"  Width="533px" 
                        BackColor="#ffffff" ></asp:TextBox><font color="red" size="2">*</font></td>
         </tr>
         <tr>
                <td bgcolor="#ffffff"><font size=2>body：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtbody"  runat="server"  Width="535px" 
                        BackColor="#ffffff" Height="101px" TextMode="MultiLine" ></asp:TextBox><font color="red" size="2">*</font></td>
         </tr>
         <tr>
                <td bgcolor="#ffffff"><font size=2>publish_time：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtpublishtime"  runat="server"  Width="150px" BackColor="#ffffff" ></asp:TextBox><font color="red" size="2">*</font></td>
         </tr>
         <tr>
                <td bgcolor="#ffffff"><font size=2>boost：</font></td>
                <td bgcolor="#ffffff"><asp:TextBox ID="txtboost"  runat="server"  Width="150px" BackColor="#ffffff" ></asp:TextBox><font color="red" size="2">*</font></td>
         </tr>
           <tr>
            <td align="center" colspan="2" bgcolor="#ffffff" ><asp:Button ID="btnaddindex" runat="server" onclick="btnaddindex_Click" Text="添加索引" />&nbsp;&nbsp;&nbsp;<a href="CatchIndex.aspx"><font size=2>返回</font></a></td>
        </tr>
    </table>
    </div>
    </form>
    <script language="javascript" src="../js/footer.js" type ="text/javascript"></script>
     <div style="display:none;"><script src="http://s11.cnzz.com/stat.php?id=2100046&web_id=2100046&show=pic" language="JavaScript"></script></div>
</body>
</html>
