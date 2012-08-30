<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnalyzerIndex.aspx.cs" Inherits="StockFoo.Web.manage.AnalyzerIndex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
     <title>股富财经搜索</title>
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <meta name="keywords" content="股票财经搜索,股票,财经,搜索" />
    <link href="../css/style.css" type="text/css" rel="Stylesheet" />
    <script language="javascript">
        function createHttpRequest() {
            try {
                return new ActiveXObject('Msxml2.XMLHTTP.7.0');
            }
            catch (e) {
                try {
                    return new ActiveXObject('Msxml2.XMLHTTP.6.0');
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

        var xmlhttpBuilde; 
        function clickBuilding() {
            try {
                if (window.confirm("生成全文索引将会删除之前创建的全文索引，然后重新创建最新的全文索引。\n确认继续吗？")) {
                    startclock();
                    document.getElementById("btnBuiding").value = "索引生成中...";
                    document.getElementById("btnBuiding").disabled = "disabled";
                    document.getElementById("divBuildeResult").innerHTML = "<img src=\"../images/loading.gif\" border=\"0\"/>";
                    xmlhttpBuilde = createHttpRequest();
                    var UrlStr = "AnalyzerIndex.aspx?ResponseBuilded=true";
                    xmlhttpBuilde.onreadystatechange = processBuilde;
                    xmlhttpBuilde.open("POST", UrlStr, true);
                    xmlhttpBuilde.setRequestHeader('Connection', 'close');
                    xmlhttpBuilde.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
                    xmlhttpBuilde.send(null);
                }
                else {
                    return;
                }
            }
            catch (e) {
                document.getElementById("btnBuiding").value = "生成全文索引";
                document.getElementById("btnBuiding").disabled = "";
                document.getElementById("divBuildeResult").innerHTML = "";
                pauseclock();
            }
        }

        function processBuilde() {
            try {
                if (xmlhttpBuilde.readyState == 4) {
                    if (xmlhttpBuilde.status == 200) {
                        if (xmlhttpBuilde.responseText == "ok") {
                            document.getElementById("divBuildeResult").innerHTML = "<font color=green><b>生成成功！</b></font>用时：" + document.getElementById("spanresult").innerHTML+"秒。";
                            document.getElementById("btnBuiding").value = "生成全文索引";
                            document.getElementById("btnBuiding").disabled = "";
                            pauseclock();
                        }
                        else {
                            document.getElementById("divBuildeResult").innerHTML = "<font color=red><b>生成失败！</b></font>" + xmlhttpBuilde.responseTex;
                            document.getElementById("btnBuiding").value = "生成全文索引";
                            document.getElementById("btnBuiding").disabled = "";
                            pauseclock();
                        }
                    }
                }
            }
            catch (e) {
                document.getElementById("btnBuiding").value = "生成全文索引";
                document.getElementById("btnBuiding").disabled = "";
                document.getElementById("divBuildeResult").innerHTML = "";
                pauseclock();
            }
        }
        
        var se, m = 0, h = 0, s = 0, ss = 1;
        function second() {
            if ((ss % 100) == 0) { s += 1; ss = 1; }
            //if (s > 0 && (s % 60) == 0) { m += 1; s = 0; }
            document.getElementById("spanresult").innerHTML = "<font color=blue>" + s + "." + ss + "0</font>";
            ss += 1;
        }
        function startclock() { se = setInterval("second()", 1);document.getElementById("spanresult").style.display = "block"; }
        function pauseclock() { clearInterval(se); ss = 1; m = h = s = 0; document.getElementById("spanresult").style.display = "none"; }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="div_logo"><a href="/"></a></div>
        <center><br /><br /><br />
            <input type="button" id="btnBuiding"  value ="生成全文索引" onclick="clickBuilding();"/>&nbsp;&nbsp;<a href="CatchIndex.aspx">返回</a><br /><br />
            <div id="divBuildeResult" style="width:300px;"></div><span id="spanresult"></span>
        <script language="javascript" src="../js/footer.js" type ="text/javascript"></script>
         <div  style="display:none"><script  src="http://s11.cnzz.com/stat.php?id=2100046&web_id=2100046&show=pic" language="JavaScript"></script></div>
    </center>
    </div>
    </form>
</body>
</html>
