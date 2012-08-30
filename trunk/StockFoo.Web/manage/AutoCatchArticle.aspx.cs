using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StockFoo.Catch;


namespace StockFoo.Web.manage
{
    public partial class AutoCatchArticle : System.Web.UI.Page
    {
        protected WebCatch wc;

        protected void Page_Load(object sender, EventArgs e)
        {
                if (Session["CatchArticle"] == null)
                {
                    wc = new WebCatch(Convert.ToInt32(Request["id"]));
                    Session["CatchArticle"] = wc;
                }
                else
                {
                    wc = (WebCatch)Session["CatchArticle"];
                }
                switch (wc.State)
                {
                    case 0:
                        {
                            this.div_load.Visible = false;
                            break;
                        }
                    case 1:
                        {
                            this.lab_state.Text = "正在抓取<font color=blue>" + wc.ParseLogicstr + "</font>数据...<br/>已抓取数据：<font color=blue>" + wc.totalcatchartend + " </font>条<br/>已入库数据：<font color=blue>" + wc.totalinsertdbend + " </font>条<br/>已索引数据：<font color=blue>" + wc.totalindexend.ToString() + " </font>条<br/>已更新数据：<font color=blue>" + wc.totalupdatedbend.ToString() + " </font>条<br/>已运行<font color=blue>" + ((TimeSpan)(DateTime.Now - wc.StartTime)).TotalSeconds.ToString("0.000") + " </font><font color='#666699'>秒";
                            this.btn_startwork.Enabled = false;
                            this.btn_startwork.Text = "数据抓取中...";
                            Page.RegisterStartupScript("", "<script>window.setTimeout('location.href=location.href',2000);</script>");
                            this.lab_jg.Text = "";
                            break;
                        }
                    case 2:
                        {
                            this.lab_jg.Text = "<font color='#666699'>任务执行完成!<br/>完成抓取数据<font color=blue>" + wc.totalcatchartend.ToString() + "</font>条;<br/>完成入库数据<font color=blue>" + wc.totalinsertdbend.ToString() + "</font>条;<br/>完成索引数据<font color=blue>" + wc.totalindexend.ToString() + " </font>条;<br/>完成更新数据<font color=blue>" + wc.totalupdatedbend.ToString() + "</font>条;<br/>用时<font color=blue>" + ((TimeSpan)(wc.FinishTime - wc.StartTime)).TotalSeconds.ToString("0.000") + " </font>秒.</font><br/><a href='CreateTopStockInfo.aspx'>生成首页资讯</a>";
                            this.btn_startwork.Enabled = true;
                            this.btn_startwork.Text = "点击开始抓取";
                            this.div_load.Visible = false;
                            Session["CatchArticle"] = null;
                            break;
                        }
                    case 3:
                        {
                            this.lab_jg.Text = "<font color='#666699'>任务结束!<br/>在<font color=blue>" + ((TimeSpan)(wc.ErrorTime - wc.StartTime)).TotalSeconds.ToString("0.000") + "</font>秒的时候发生错误导致任务失败.</font>'";
                            this.btn_startwork.Enabled = true;
                            this.btn_startwork.Text = "点击开始抓取";
                            this.div_load.Visible = false;
                            Session["CatchArticle"] = null;
                            break;
                        }
                }
        }

        protected void btn_startwork_Click(object sender, EventArgs e)
        {
            if (wc.State != 1)
            {
                this.btn_startwork.Enabled = false;
                this.div_load.Visible = true;
                wc.JustDoing();
                Page.RegisterStartupScript("", "<script>location.href=location.href;</script>");
            }
            else
            {
                Page.RegisterStartupScript("info", "<script>alert('" + ((WebCatch)Session["CatchArticle"]).ParseLogicstr + "数据正在抓取中,请稍候再操作！');</script>"); 
            }
        }
    }
}
