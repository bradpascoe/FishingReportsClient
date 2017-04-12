using System;
using System.IO;
using System.Linq;

using FishingReports.Client.Model;
using FishingReports.Client.Repositories;

namespace FishingReportsProxy
{
	internal class ReportProxy : IReportProxy
	{
		#region ReportProxy Members

		public int AddReport(Report report)
		{
			var newReportId = mReportRepository.SaveReport(report);

			return newReportId;
		}

		public Report GetNewReport(string username)
		{
			return new Report
			{
				ReportDate = DateTime.Today,
				Username = username,
				Images = new ReportImage[] { }
			};
		}

		public Report GetReport(int reportId)
		{
			return mReportRepository.GetReport(reportId);
		}

		public ReportSearchResult GetReports()
		{
			return mReportRepository.GetReports();
		}

		public ReportSearchResult GetReports(ReportFilter filter)
		{
			return mReportRepository.GetReports(filter);
		}

		public int[] GetYears()
		{
			return mReportRepository.GetYears().ToArray();
		}

		public void UpdateReport(Report report)
		{
			mReportRepository.UpdateReport(report);
		}

		public ReportImage UploadNewImage(Stream imageStream, string imageFileName, string username)
		{
			var image = mImageRepository.UploadNewImage(imageStream, imageFileName, username);

			return image;
		}

		#endregion ReportProxy Members

		#region Fields

		private readonly IImageRepository mImageRepository = RepositoryFactory.CreateImageRepository();
		private readonly IReportRepository mReportRepository = RepositoryFactory.CreateReportRepository();

		#endregion Fields

	}
}
