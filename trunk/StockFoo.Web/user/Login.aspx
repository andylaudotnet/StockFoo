<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="StockFoo.Web.user.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>股富财经搜索</title>
    <meta http-equiv="content-type" content="text/html;charset=utf-8">
    <meta name="keywords" content="股票财经搜索,股票,财经,搜索" />
    <link href="../css/style.css" type="text/css" rel="Stylesheet" />
 <script language="javascript" type ="text/javascript">
        function btnLogin_onclick() {
            if (document.getElementById("txtusername").value == "") {
                alert("请输入用户名！");
                document.getElementById("txtusername").focus();
                return false;
            }
            if (document.getElementById("txtpassword").value == "") {
                alert("请输入密码！");
                document.getElementById("txtpassword").focus();
                return false;
            }
            return true;
        }
    </script>
</head>
<body     bgcolor="#ffffff" >
    <form id="form1" runat="server">
    <div style="margin-top:77px;">
    <table  border ="0" cellpadding ="0" cellspacing ="0" width ="555px" bgcolor="#ffffff" align="center"><tr><td align="left"><a href="../default.aspx"><img border="0"  src="../images/logo.png"/></a></td></tr></table>
         <center>
				        <table border="0" width="555px" border="0"  cellSpacing="1" cellPadding="4" bgcolor="#9999ff">
				            <tr><td colspan=2  style="background-image:url(../images/menu.jpg) "  align="left"><font size=3 color="#666699"><b>用户登录：</b></font><input type="hidden" id="hiddenPreUrl" runat="server" /></td></tr>
				            <tr>
				                <td width="35%" align="right" bgcolor="#ffffff">用户名：&nbsp;&nbsp;&nbsp;</td>
				                <td width="65%" align="left" bgcolor="#ffffff"><input  type="text"  id="txtusername" runat="server" /></td>
				            </tr>
				            <tr >
				                <td align="right" bgcolor="#ffffff">密&nbsp;&nbsp;&nbsp;&nbsp;码：&nbsp;&nbsp;&nbsp;</td>
				                <td  align="left" bgcolor="#ffffff"><input  type="password" ID="txtpassword" runat="server" />&nbsp;<asp:CheckBox ID="ckbpwdremember" runat="server" Text="记住密码(公用电脑勿选)" />
                                </td>
				            </tr>
				            <tr>
				                <td colspan ="2" align ="center" bgcolor="#ffffff"><asp:Button ID="BtnLogin"  runat="server"  OnClientClick="return btnLogin_onclick()"  Text="登&nbsp;录" Width="73px"  onclick="BtnLogin_Click" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href ="Register.aspx"><font size="2" >新用户注册</font></a>
				            </tr>
				        </table>
		</center>
    </div>
    </form>
    <script language="javascript" src="../js/footer.js" type ="text/javascript"></script>
	<div style="display:none;"><script src="http://s11.cnzz.com/stat.php?id=2100046&web_id=2100046&show=pic" language="JavaScript"></script></div>
</body>
</html>