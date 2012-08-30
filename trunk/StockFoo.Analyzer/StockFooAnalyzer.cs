using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;

namespace StockFoo.Analyzer
{
    public class StockFooAnalyzer : Lucene.Net.Analysis.Analyzer
    {
        private string dictPath;
        //定义要过滤的词
        //public string[] CHINESE_ENGLISH_STOP_WORDS;
        //public string StopPath = System.Configuration.ConfigurationManager.AppSettings["AnalyzerPath"].ToString() + "stopwords.txt";


        public StockFooAnalyzer(string dictPath)
		{
			this.dictPath = dictPath;
            //ArrayList StopWords= new ArrayList();
            //StreamReader reader = new StreamReader(StopPath, System.Text.Encoding.GetEncoding("gb2312"));
            //string noise = reader.ReadLine();
            //int i = 0;
            //while (!string.IsNullOrEmpty(noise))
            //{
            //    StopWords.Add(noise);
            //    noise = reader.ReadLine();
            //    i++;
            //}
            //CHINESE_ENGLISH_STOP_WORDS = new String[i];

            //while (i>0)
            //{
            //    i--;
            //    CHINESE_ENGLISH_STOP_WORDS[i] = (string)StopWords[i];
            //}
            //StopWords.Clear();
		}

		public override Lucene.Net.Analysis.TokenStream TokenStream(string fieldName, System.IO.TextReader reader)
		{
            Lucene.Net.Analysis.TokenStream ts = new StockFooTokenizer(reader, this.dictPath);
            //ts = new StandardFilter(ts);
            //ts = new LowerCaseFilter(ts);
            //ts = new StopFilter(ts, CHINESE_ENGLISH_STOP_WORDS);
			return ts;
		}
    }
}
