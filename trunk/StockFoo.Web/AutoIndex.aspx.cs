using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StockFoo.Web.manage
{
    public partial class AutoIndex : System.Web.UI.Page
    {
        protected string catchid = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["id"] != null)
                {
                    catchid = Request["id"].ToString();
                }
            }
        }
    }
}
