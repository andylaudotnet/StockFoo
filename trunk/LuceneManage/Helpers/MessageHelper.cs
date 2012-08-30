/**
 * author : StockFoo
 *      http://www.stockfoo.com
 * description:
 *      
 * history : created by StockFoo 2010-05-06 5:01:54 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace LuceneManage.Helpers {
    /// <summary>
    /// 
    /// </summary>
    public static class MessageHelper {
        public static void ShowErrorMessage(string msg) {
            MessageBox.Show(msg, "错误提示信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
