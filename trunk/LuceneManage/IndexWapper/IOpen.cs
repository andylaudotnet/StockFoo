﻿/**
 * author : StockFoo
 *      http://www.stockfoo.com
 * description:
 *      
 * history : created by StockFoo 2010-05-06 5:52:10 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Index;

namespace LuceneManage.IndexWapper {
    /// <summary>
    /// 索引打开接口
    /// </summary>
    public interface IOpen {
        /// <summary>
        /// 返回IndexReader对象
        /// </summary>
        IndexReader Reader { get; }
        /// <summary>
        /// 返回索引路径
        /// </summary>
        string IndexPath { get; }
        /// <summary>
        /// 返回索引是否被打开
        /// </summary>
        bool IsOpend { get; }
    }
}
