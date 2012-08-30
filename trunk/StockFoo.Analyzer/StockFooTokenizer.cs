using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Analysis.Standard;
using System.IO;
using SharpICTCLAS;

namespace StockFoo.Analyzer
{
    public class StockFooTokenizer : Lucene.Net.Analysis.Tokenizer
    {
       static WordSegment wordSegment;

		private string txt;
		private WordResult[] list;

		private bool isStart = true;
		private int nKind;
		private int current;
		private int max;

        internal StockFooTokenizer(TextReader reader, string dictPath)
		{
			this.input = reader;
			this.txt = this.input.ReadToEnd();

            if (StockFooTokenizer.wordSegment == null)
			{
                StockFooTokenizer.wordSegment = new WordSegment();
                StockFooTokenizer.wordSegment.InitWordSegment(dictPath);
			}
		}

		public override Token Next()
		{
			if (isStart)
			{
                this.list = StockFooTokenizer.wordSegment.Segment(this.txt)[0];
				current = 1;
				max = this.list.Length - 2;
				isStart = false;
			}

			if (current > max)
				return null;

			WordResult wr = this.list[current];
			Token token = new Token(wr.sWord, wr.sLocation - 1, wr.sLocation + wr.sWord.Length - 1);
			current++;
			return token;
		}
    }
}
