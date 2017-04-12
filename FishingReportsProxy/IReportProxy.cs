using System.IO;

using FishingReports.Client.Model;

namespace FishingReportsProxy
{
	public interface IReportProxy
	{
		#region IReportProxy Members

		int AddReport(Report report);

		Report GetNewReport(string username);

		Report GetReport(int reportId);

		ReportSearchResult GetReports();

		ReportSearchResult GetReports(ReportFilter filter);

		int[] GetYears();

		void UpdateReport(Report report);

		ReportImage UploadNewImage(Stream imageStream, string imageFileName, string username);

		#endregion IReportProxy Members

	}
}
