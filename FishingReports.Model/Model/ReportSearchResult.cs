namespace FishingReports.Client.Model
{
	public class ReportSearchResult
	{
		#region ReportSearchResult Members

		public double AverageNumberOfFish
		{
			get;
			set;
		}

		public int NumberOfReports
		{
			get;
			set;
		}

		public ReportListItem[] Results
		{
			get;
			set;
		}

		public int TotalNumberOfFish
		{
			get;
			set;
		}

		#endregion ReportSearchResult Members

	}
}
