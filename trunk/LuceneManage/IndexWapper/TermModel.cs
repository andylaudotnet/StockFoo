﻿/**
 * author : StockFoo
 *      http://www.stockfoo.com
 * description:
 *      
 * history : created by StockFoo 2010-05-06 2:58:50 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Index;

namespace LuceneManage.LuceneAPI {
    /// <summary>
    /// 项模型，额外增加了出现次数属性。
    /// </summary>
    public class TermModel {
        private int count;
        private Term term;
        /// <summary>
        /// 创建项模型。
        /// </summary>
        /// <param name="term">项数据</param>
        /// <param name="count">出现次数</param>
        public TermModel(Term term, int count) {
            this.term = term;
            this.count = count;
        }
        /// <summary>
        /// 项数据
        /// </summary>
        public Term Term {
            get {
                return term;
            }
        }
        /// <summary>
        /// 出现次数
        /// </summary>
        public int Count {
            get {
                return count;
            }
        }
    }
}
