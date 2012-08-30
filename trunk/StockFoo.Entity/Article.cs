using System;

namespace StockFoo.Entity
{
	public class Article
	{
		#region Properties (7) 

		public string Body { get; set; }

		public DateTime CreateTime { get; set; }

		public Guid Guid { get; set; }

		public DateTime PublishTime { get; set; }

		public string Site { get; set; }

		public string Title { get; set; }

		public string Url { get; set; }

		#endregion Properties 
	}
}
