/**
 * author : StockFoo
 *      http://www.stockfoo.com
 * description:
 *      
 * history : created by StockFoo 2010-05-06 4:32:01 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace LuceneManage.Helpers {
    /// <summary>
    /// 
    /// </summary>
    public static class ProcessHelper {
        public static void StartDefaultExplorer(string url) {
            Process process = Process.Start("explorer", url);
        }
    }
}
