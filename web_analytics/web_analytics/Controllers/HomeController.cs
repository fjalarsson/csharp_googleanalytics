using System.Web.Mvc;
using Google.Apis.Analytics.v3.Data;
using web_analytics.Extensions;
using web_analytics.Models.ViewModels;
using web_analytics.Services;

namespace web_analytics.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index(string dimensions, string metrics)
		{
			var model = new PageVisitViewModel();
			if (dimensions.IsNotNullOrEmpty() && metrics.IsNotNullOrEmpty())
			{
				model.Dimensions = dimensions;
				model.Metrics = metrics;
				model.Visits = new GoogleAnalyticsService().GetAnalyticsData(70, 5, dimensions, metrics);
			}
			else if (dimensions.IsNotNullOrEmpty())
			{
				model.Dimensions = dimensions;
				model.Visits = new GoogleAnalyticsService().GetAnalyticsData(70, 5, dimensions);
			}
			else if (metrics.IsNotNullOrEmpty())
			{
				model.Metrics = metrics;
				model.Visits = new GoogleAnalyticsService().GetAnalyticsData(70, 5, null, metrics);
			}
			else
			{
				model.Visits = new GoogleAnalyticsService().GetAnalyticsData(70, 5);
			}
		
			return View(model);
		}
	}
}