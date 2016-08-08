using System.Collections.Generic;

namespace web_analytics.Models.ViewModels
{
	public class PageVisitViewModel
	{
		public List<PageVisit> Visits { get; set; }
		public string Dimensions { get; set; }
		public string Metrics { get; set; }
	}
}