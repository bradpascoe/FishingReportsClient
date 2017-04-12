using System;

namespace FishingReports.Client.Model
{
	[Serializable]
	public class ReportFilter
	{
		#region ReportFilter Members

		public string Location
		{
			get;
			set;
		}

		public int? Month
		{
			get;
			set;
		}

		public int? Year
		{
			get;
			set;
		}

		public bool IsFilterSet => !string.IsNullOrEmpty( Location ) ||
		                           Year.HasValue ||
		                           Month.HasValue;

		#endregion ReportFilter Members

	}
}
