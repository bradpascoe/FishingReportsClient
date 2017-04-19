using System.Web.Mvc;
using System.Web.Routing;

namespace FishingReportsReact
{
	public class RouteConfig
	{
		#region RouteConfig Members

		public static void RegisterRoutes( RouteCollection routes )
		{
			routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

			routes.MapRoute(
				name: "Reports",
				url: "reports",
				defaults: new
				{
					controller = "Home",
					action = "GetReports"
				}
			);

			routes.MapRoute(
				name: "Details",
				url: "details/{id}",
				defaults: new
				{
					controller = "Home",
					action = "Details"
				}
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new
				{
					controller = "Home",
					action = "Index",
					id = UrlParameter.Optional
				}
			);
		}

		#endregion RouteConfig Members

	}
}