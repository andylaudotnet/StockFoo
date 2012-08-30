using System;

namespace StockFoo.Entity
{
	public class Catch
	{
		#region Properties (15) 

        public string ArticleRegex { get; set; }

		public string ArticleTimeFormat { get; set; }

        public string ArticleTimeRegex { get; set; }

		public string ArticleTimeXPath { get; set; }

        public string ArticleTitleRegex { get; set; }

		public string ArticleTitleXPath { get; set; }

		public string ArticleXPath { get; set; }

        public string CatalogRegex { get; set; }

		public string CatalogUrl { get; set; }

		public string CatalogXPath { get; set; }

		public int ClassId { get; set; }

		public Guid Guid { get; set; }

		public int Id { get; set; }

		public int SiteEncode { get; set; }

		public string SiteName { get; set; }

        public bool Enabled { get; set; }

        public string CatchName { get; set; }

		public DateTime? NextTime { get; set; }

		public int TimeSpan { get; set; }

		#endregion Properties 
	}
}
