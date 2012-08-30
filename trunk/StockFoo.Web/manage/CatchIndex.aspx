<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatchIndex.aspx.cs" Inherits="StockFoo.Web.manage.CatchIndex" %>

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
            <table  border ="0" cellpadding =0" cellspacing ="0" width ="666px" align="center">
            <tr>
               <td align="left"><a href="../default.aspx"><img border="0"  src="../images/logo.png"/></a></td>
               <td align="right" valign="bottom"></td>
            </tr>
            </table>
             <table  border ="0" cellpadding ="4" cellspacing ="0" width ="666px" align="center">
            <tr>
               <td align="left" valign="bottom"><a href="AnalyzerTest.aspx">分词效果预览</a>&nbsp;&nbsp;<a href="AnalyzerIndex.aspx">生成全文索引</a>&nbsp;&nbsp;<a href="BoostManage.aspx">新建索引</a>&nbsp;&nbsp;<a href="CreateTopStockInfo.aspx">生成首页资讯</a></td>
               <td align="right" valign="bottom"><input type="button"  value="新增配置" onclick="window.location.href ='AddCatch.aspx';" /></td>
            </tr>
            </table>
        <table  border ="0" cellpadding ="0" cellspacing ="0" width ="666px" align="center">
            <tr>
                <td>
                    <asp:GridView ID="gvCatchList" runat="server" Width="666px" 
                        AutoGenerateColumns="False" onrowdatabound="gvCatchList_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="序号"   FooterStyle-ForeColor="Black"
                                HeaderStyle-Height="20px" ItemStyle-BorderColor="#CCCCCC" ItemStyle-HorizontalAlign="Center">
<FooterStyle ForeColor="Black"></FooterStyle>

<HeaderStyle  Height="24px" BorderColor="#CCCCCC"></HeaderStyle>
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="id" 
                                DataNavigateUrlFormatString="UpdateCatch.aspx?id={0}" 
                                DataTextField="catch_name" HeaderText="抓取配置名称"   
                                  ItemStyle-BorderColor="#CCCCCC">
<HeaderStyle   BorderColor="#CCCCCC"></HeaderStyle>
                            </asp:HyperLinkField>
                            <asp:BoundField DataField="site_name" HeaderText="站点名称"   
                                  ItemStyle-BorderColor="#CCCCCC">
<HeaderStyle  BorderColor="#CCCCCC"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="site_encode" HeaderText="站点编码"  
                                ItemStyle-HorizontalAlign="Center"    ItemStyle-BorderColor="#CCCCCC">
<HeaderStyle  BorderColor="#CCCCCC"></HeaderStyle>

<ItemStyle HorizontalAlign="Center" BorderColor="#CCCCCC"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="classname" HeaderText="文章类别"  
                                ItemStyle-HorizontalAlign="Center"    ItemStyle-BorderColor="#CCCCCC">
<HeaderStyle  BorderColor="#CCCCCC"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="enabled" HeaderText="状态"  
                                ItemStyle-HorizontalAlign="Center"    ItemStyle-BorderColor="#CCCCCC">
<HeaderStyle  BorderColor="#CCCCCC"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="id" 
                                DataNavigateUrlFormatString="AutoIndex.aspx?id={0}"  Text="抓取" 
                                ItemStyle-HorizontalAlign="Center" HeaderText="数据抓取"  
                                  ItemStyle-BorderColor="#CCCCCC">
<HeaderStyle  BorderColor="#CCCCCC"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:HyperLinkField>
                        </Columns>
                    </asp:GridView>
            </td>
          </tr>
      </table>
      <table  border ="0" cellpadding ="4" cellspacing ="0" width ="666px" align="center">
            <tr>
               <td align="right" valign="top"><a href="AutoIndex.aspx?id=0"><font color=#0353ce>
                   全部抓取</font></a>&nbsp;&nbsp;&nbsp;</td>
            </tr>
            </table>
    </form>
    <script language="javascript" src="../js/footer.js" type ="text/javascript"></script>
</body>
</html>
