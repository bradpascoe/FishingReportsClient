using System;

namespace FishingReports.Client.Model
{
	public class ReportListItem
	{
		#region ReportListItem Members

		public string Access
		{
			get;
			set;
		}

		public Guid AccessId
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int? FlowRate
		{
			get;
			set;
		}

		public string Location
		{
			get;
			set;
		}

		public Guid LocationId
		{
			get;
			set;
		}

		public int NumberOfImages
		{
			get;
			set;
		}

		public DateTime ReportDate
		{
			get;
			set;
		}

		public string ReportDateString => ReportDate.ToShortDateString();

		public int ReportId
		{
			get;
			set;
		}

		public ReportType ReportType
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public Guid StateId
		{
			get;
			set;
		}

		public int? TotalFish
		{
			get;
			set;
		}

		public string Username
		{
			get;
			set;
		}

		public string WaterConditions
		{
			get;
			set;
		}

		public string WeatherConditions
		{
			get;
			set;
		}

		#endregion ReportListItem Members

	}
}
