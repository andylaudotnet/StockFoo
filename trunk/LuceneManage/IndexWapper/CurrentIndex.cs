/**
 * author : StockFoo
 *      http://www.stockfoo.com
 * description:
 *      
 * history : created by StockFoo 2010-05-06 4:57:31 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace LuceneManage.IndexWapper {
    /// <summary>
    /// 当前索引
    /// </summary>
    public class CurrentIndex {
        private static object index_locked = new object();
        private static IndexOpen index;
        /// <summary>
        /// 指定当前索引
        /// </summary>
        /// <param name="index"></param>
        public static void SetCurrentOpendIndex(IndexOpen index) {
            lock (index_locked) {
                CurrentIndex.index = index;
            }
        }
        /// <summary>
        /// 返回当前索引
        /// </summary>
        /// <returns></returns>
        public static IndexOpen GetCurrentOpendIndex() {
            lock (index_locked) {
                return index;
            }
        }
        /// <summary>
        /// 当前索引是否被打开
        /// </summary>
        /// <returns></returns>
        public static bool IsIndexBeOpend() {
            lock (index_locked) {
                return index != null && index.IsOpend;
            }
        }
    }
}
