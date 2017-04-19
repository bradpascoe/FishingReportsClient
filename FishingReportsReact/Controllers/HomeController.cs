﻿using System.Web.Mvc;
using System.Web.UI;

using FishingReports.Client.Model;

using FishingReportsProxy;

namespace FishingReportsReact.Controllers
{
	public class HomeController : Controller
	{
		#region HomeController Members

		private IReportProxy mReportProxy;
		public IReportProxy ReportProxy
		{
			get
			{
				return mReportProxy ?? (mReportProxy = ProxyFactory.CreateReportProxy());
			}
		}

		public ActionResult About()
		{
			throw new System.NotImplementedException();
		}

		public ActionResult Contact()
		{
			throw new System.NotImplementedException();
		}

		[OutputCache(Location = OutputCacheLocation.None)]
		public ActionResult Details(int reportId)
		{
			Report report = ReportProxy.GetReport(reportId);

			return Json( report, JsonRequestBehavior.AllowGet );
		}

		[OutputCache(Location = OutputCacheLocation.None)]
		public ActionResult GetReports()
		{
			var reports = ReportProxy.GetReports();

			return Json(reports.Results, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Index()
		{
			return View();
		}

		#endregion HomeController Members

	}
}
