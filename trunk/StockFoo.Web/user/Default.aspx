<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="StockFoo.Web.user.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title>股富财经搜索</title>
    <meta http-equiv="content-type" content="text/html;charset=utf-8">
    <meta name="keywords" content="股票财经搜索,股票,财经,搜索" />
    <link href="../css/style.css" type="text/css" rel="Stylesheet" />
    <script language ="javascript">
        function ConfirmElements() {
            if (document.getElementById("txtRealName").value == "") {
                alert("昵称不能为空！");
                document.getElementById("txtRealName").focus();
                return false;
            }
            if (document.getElementById("hiddenNameCheckOk").value == "error") {
                alert("您所输入昵称已被使用，请更改其他可用昵称！");
                document.getElementById("txtRealName").focus();
                return false;
            }
            if (document.getElementById("txtRemark").value == "") {
                txtRemark
                alert("个性签名不能为空！");
                document.getElementById("txtRemark").focus();
                return false;
            }
            return true;
        }

        function createHttpRequest() {
            try {
                return new ActiveXObject('MSXML2.XMLHTTP.4.0');
            }
            catch (e) {
                try {
                    return new ActiveXObject('MSXML2.XMLHTTP.3.0');
                }
                catch (e) {
                    try {
                        return new ActiveXObject('MSXML2.XMLHTTP.5.0');
                    } catch (e) {
                        try {
                            return new ActiveXObject('MSXML2.XMLHTTP');
                        } catch (e) {
                            try {
                                return new ActiveXObject('Microsoft.XMLHTTP');
                            } catch (e) {
                                if (window.XMLHttpRequest) {
                                    return new XMLHttpRequest();
                                }
                                else {
                                    return null;
                                }
                            }
                        }
                    }
                }
            }
        }

        var xmlhttpUserInfoEdit;
        function CheckNameIsExist() {
            try {
                if (document.getElementById("txtRealName").value == "")
                    return;
                xmlhttpUserInfoEdit = createHttpRequest();
                document.getElementById("spannamecheck").innerHTML = "<font size=2 color=blue><b>昵称检查中...</b></font>";
                var UrlStr = "UserInfoEdit.aspx?ResponseName=" + escape(document.getElementById("txtRealName").value) + "&ResponseRealName=" + escape(document.getElementById("hiddenRealName").value);
                xmlhttpUserInfoEdit.onreadystatechange = processReqChange2;
                xmlhttpUserInfoEdit.open("POST", UrlStr, true);
                xmlhttpUserInfoEdit.setRequestHeader('Connection', 'close');
                xmlhttpUserInfoEdit.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
                xmlhttpUserInfoEdit.send(null);
            }
            catch (e) {
                document.getElementById("spannamecheck").innerHTML = "<font size=2 color=red><b>网络忙，请稍后修改此昵称!</b></font>";
                document.getElementById("hiddenNameCheckOk").value = "error";
            }
        }

        function processReqChange2() {
            try {
                if (xmlhttpUserInfoEdit.readyState == 4) {
                    if (xmlhttpUserInfoEdit.status == 200) {
                        if (xmlhttpUserInfoEdit.responseText == "no") {
                            document.getElementById("spannamecheck").innerHTML = "<font size=2 color=green><b>此昵称可用!</b></font>";
                            document.getElementById("hiddenNameCheckOk").value = "ok";
                        }
                        else {
                            document.getElementById("spannamecheck").innerHTML = "<font size=2 color=red><b>此昵称已被使用，请更换其他可用昵称!</b></font>";
                            document.getElementById("hiddenNameCheckOk").value = "error";
                        }
                    }
                }
            }
            catch (e) {
                document.getElementById("spannamecheck").innerHTML = "<font size=2 color=red><b>网络忙，请稍后修改此昵称!</b></font>";
                document.getElementById("hiddenNameCheckOk").value = "error";
            }
        }
    </script>
    <style type="text/css">
        #txtRemark
        {
            height: 98px;
            width: 443px;
        }
    </style>
</head>
<body  bgcolor="#ffffff">
    <form id="form1" runat="server"><input type ="hidden" id ="hiddenRealName"  runat ="server"/><input type ="hidden" id ="hiddenNameCheckOk" />
<table   align="center"  border ="0" cellpadding ="0" cellspacing ="0" width ="770px" bgcolor="#ffffff" >
        <tr>
            <td>
                        <table   align="left"  border ="0" cellpadding ="0" cellspacing ="0" width ="800px" bgcolor="#ffffff"><tr><td><a href="../default.aspx"><img border="0"  src="../images/logo.png"/></a></td></tr></table>

            </td>
        </tr>
             <tr>
            <td>
                 <table align="left"  border ="0" cellpadding ="4" cellspacing ="1" width ="800px" bgcolor="#9999ff">
            <tr>
                <td align="left"  bgcolor="#ffffff" colspan ="2"  style="background-image:url(../images/menu.jpg) " ><font size="3" color="#666699" ><b>个人信息:</b></font></td>
            </tr>
            <tr>
                <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>用户名：</font></td>
                <td bgcolor="#ffffff" width="60%" align="left"  id ="txtUsername" runat ="server"></td>
            </tr>
            <tr>
                <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>用户自定义头像： </font></td>
                <td bgcolor="#ffffff" width="60%" align="left" >
                    <asp:Image  ID="imgUserImg" runat="server" Height="75px" Width="90px"  ImageUrl="img/noupload.jpg"/>&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:FileUpload ID="FileUploadImg" runat="server"     />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnUploadImg" runat="server" Text="上传"   onclick="btnUploadImg_Click"  CssClass="ButtonCss"/>
               </td>
            </tr>
             <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>中文昵称：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left"><input type ="text" id ="txtRealName" runat ="server"  onblur="CheckNameIsExist();"      /><font color=red >*</font><span id="spannamecheck"><font size=2>中文
                                </font></span></td>
             </tr>
             <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>性别：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left">
                                <select id ="sltUserSex" runat ="server" >
                                     <option value="男" selected>男</option>
                                     <option value="女">女</option>
                                </select>
                            </td>
            </tr>
          
           <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>出生日期：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left"><input type ="text" id ="txtUserBirthday" runat ="server"  value =""    /></td>
                        </tr>
                        <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>电话：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left"><input type ="text" id ="txtUserTel" runat ="server"     /></td>
                        </tr>
                        <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>手机：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left"><input type ="text" id ="txtUserMobile" runat ="server"     /></td>
                        </tr>
                         <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>Email：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left"><input type ="text" id ="txtUserEmail" runat ="server"     /></td>
                        </tr>
                         <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>Msn：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left"><input type ="text" id ="txtUserMsn" runat ="server"     /></td>
                        </tr>
                        <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>QQ：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left"><input type ="text" id ="txtQQ" runat ="server"     /></td>
                        </tr>
                         <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>地址：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left"><input type ="text" id ="txtAddress" runat ="server"     /></td>
                        </tr>
                         <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>邮编：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left"><input type ="text" id ="txtZip" runat ="server"     /></td>
                        </tr>
                          <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>受教育程度：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left">
                                <select id ="sltEducation" runat="server">
                                    <option value="">请选择</option>
                                    <option value="高中及以下">高中及以下</option>
                                    <option value="专科(大专)">专科(大专)</option>
                                    <option value="本科">本科</option>
                                    <option value="硕士">硕士</option>
                                    <option value="博士及以上">博士及以上</option>
                                </select>
                            </td>
                        </tr>
                          <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>行业：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left">
                                <select id ="sltProfession" runat="server">
                                 <option value="">请选择</option>
                                    <option value="销售/市场营销">销售/市场营销</option>
                                    <option value="技术研发/售前售后工程师">技术研发/售前售后工程师</option>
                                    <option value="企业管理/行政/人力资源">企业管理/行政/人力资源</option>
                                    <option value="证券/金融/投资">证券/金融/投资</option>
                                    <option value="公关/咨询/媒体">公关/咨询/媒体</option>
                                    <option value="公务员/科/教/文/卫">公务员/科/教/文/卫</option>
                                     <option value="私营企业主/贸易">私营企业主/贸易</option>
                                      <option value="其他">其他</option>
                                </select>
                            </td>
                        </tr>
                          <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>国家：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left">
                                <select id ="sltCountry" runat="server">
                                <option value="">请选择</option>
                                    <option value="中国">中国</option>
                                    <option value="美国">美国</option>
                                    <option value="英国">英国</option>
                                    <option value="法国">法国</option>
                                    <option value="德国">德国</option>
                                    <option value="巴西">巴西</option>
                                    <option value="韩国">韩国</option>
                                    <option value="伊朗">伊朗</option>
                                    <option value="朝鲜">朝鲜</option>
                                     <option value="泰国">泰国</option>
                                     <option value="越南">越南</option>
                                     <option value="印度">印度</option>
                                     <option value="南非">南非</option>
                                      <option value="日本">日本</option>
                                     <option value="墨西哥">墨西哥</option>
                                     <option value="意大利">意大利</option>
                                     <option value="新加坡">新加坡</option>
                                    <option value="俄罗斯">俄罗斯</option>
                                    <option value="加拿大">加拿大</option>
                                     <option value="马来西亚">马来西亚</option>
                                    <option value="澳大利亚">澳大利亚</option>
                                    <option value="巴基斯坦">巴基斯坦</option>
                                    <option value="其他">其他</option>
                                </select>
                            </td>
                        </tr>
                          <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>省份：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left">
                                <select id ="sltProvince" runat="server">
                                 <option value="">请选择</option>
                                   <option value="北京">北京</option>
                                    <option value="上海">上海</option>
                                    <option value="香港">香港</option>
                                    <option value="澳门">澳门</option>
                                    <option value="湖南">湖南</option>
                                    <option value="深圳">深圳</option>
                                    <option value="广东">广东</option>
                                    <option value="天津">天津</option>
                                    <option value="广西">广西</option>
                                    <option value="江西">江西</option>
                                    <option value="山西">山西</option>
                                    <option value="江苏">江苏</option>
                                     <option value="四川">四川</option>
                                       <option value="重庆">重庆</option>
                                      <option value="浙江">浙江</option>
                                      <option value="湖北">湖北</option>
                                      <option value="山东">山东</option>
                                      <option value="辽宁">辽宁</option>
                                      <option value="哈尔滨">哈尔滨</option>
                                      <option value="大连">大连</option>
                                       <option value="内蒙古">内蒙古</option>
                                      <option value="河南">河南</option>
                                      <option value="河北">河北</option>
                                      <option value="西藏">西藏</option>
                                      <option value="新疆">新疆</option>
                                      <option value="宁夏">宁夏</option>
                                       <option value="其他">其他</option>
                                </select>
                            </td>
                        </tr>
                          <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>城市：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left">
                                <select id ="sltCity" runat="server">
                                 <option value="">请选择</option>
                                     <option value="北京">北京</option>
                                    <option value="上海">上海</option>
                                    <option value="长沙">长沙</option>
                                     <option value="香港">香港</option>
                                    <option value="澳门">澳门</option>
                                    <option value="广州">广州</option>
                                    <option value="深圳">深圳</option>
                                    <option value="澳门">澳门</option>
                                    <option value="天津">天津</option>
                                    <option value="大连">大连</option>
                                    <option value="南昌">南昌</option>
                                     <option value="常德">常德</option>
                                      <option value="岳阳">岳阳</option>
                                       <option value="邵阳">邵阳</option>
                                        <option value="耒阳">耒阳</option>
                                         <option value="祁阳">祁阳</option>
                                          <option value="郴州">郴州</option>
                                           <option value="浏阳">浏阳</option>
                                            <option value="株洲">株洲</option>
                                             <option value="衡山">衡山</option>
                                    <option value="衡阳">衡阳</option>
                                    <option value="武汉">武汉</option>
                                    <option value="安徽">安徽</option>
                                    <option value="浙江">浙江</option>
                                    <option value="成都">成都</option>
                                     <option value="昆明">昆明</option>
                                      <option value="扬州">扬州</option>
                                       <option value="其他">其他</option>
                                </select>
                            </td>
                        </tr>
                          <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>上网地点：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left">
                                <select id ="sltNetAddress" runat="server">
                                 <option value="">请选择</option>
                                    <option value="办公室">办公室</option>
                                    <option value="住所">住所</option>
                                    <option value="网吧">网吧</option>
                                    <option value="户外">户外</option>
                                    <option value="其他">其他</option>
                                </select>
                            </td>
                        </tr>
                          <tr>
                            <td bgcolor="#ffffff" width="40%" align="right"><font color=black size=2>知晓本站渠道：</font></td>
                            <td bgcolor="#ffffff" width="60%" align="left">
                                <select id ="sltchannel" runat="server">
                                    <option value="广告">广告</option>
                                    <option value=">朋友介绍" selected="selected">朋友介绍</option>
                                    <option value="3其他网站链接">其他网站链接</option>
                                    <option value="搜索">搜索</option>
                                    <option value="其他">其他</option>
                                </select>
                            </td>
                        </tr>
            <tr>
                <td bgcolor="#ffffff"  width="40%" align="right"><font color="black">个性签名：</font></td>
                <td bgcolor="#ffffff" width="60%" align="left"><textarea id ="txtRemark" runat ="server" ></textarea></td>
            </tr>
            <tr>
                <td bgcolor="#ffffff" colspan="2"  align="center"><asp:Button   ID="btnregister1" runat="server" onclick="btnregister1_Click" Text="保 存"  width="77px" />
                &nbsp;&nbsp;&nbsp;<a href="../default.aspx">返回</a></td>
            </tr>
        </table>
            </td>
        </tr>
    </table>
    </form>
    <script language="javascript" src="../js/footer.js" type ="text/javascript"></script>
<div style="display:none;"><script src="http://s11.cnzz.com/stat.php?id=2100046&web_id=2100046&show=pic" language="JavaScript"></script></div>
</body>
</html>
