using FishingReports.Client.Model;

namespace FishingReports.Client.Repositories
{
	public interface IReportRepository
	{
		#region IReportRepository Members

		Report GetReport(int reportId);

		ReportSearchResult GetReports();

		ReportSearchResult GetReports(ReportFilter filter);

		int[] GetYears();

		int SaveReport(Report entity);

		void UpdateReport(Report report);

		#endregion IReportRepository Members

	}
}
