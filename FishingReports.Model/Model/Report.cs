using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FishingReports.Client.Model
{
	public class Report
	{
		#region Report Members

		public string Access
		{
			get;
			set;
		}

		[Required]
		[DisplayName("Access")]
		public Guid AccessId
		{
			get;
			set;
		}

		[DataType(DataType.MultilineText)]
		public string Description
		{
			get;
			set;
		}

		[DisplayName("Flow rate")]
		public int? FlowRate
		{
			get;
			set;
		}

		[DisplayName("Images")]
		public ReportImage[] Images
		{
			get;
			set;
		}

		public string Location
		{
			get;
			set;
		}

		[Required]
		[DisplayName("Location")]
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

		[DisplayName("Report date")]
		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime ReportDate
		{
			get;
			set;
		}

		public int ReportId
		{
			get;
			set;
		}

		[DisplayName("Report type")]
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

		[Required]
		[DisplayName("State")]
		public Guid StateId
		{
			get;
			set;
		}

		[DisplayName("Total fish")]
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

		[DisplayName("Water conditions")]
		public string WaterConditions
		{
			get;
			set;
		}

		[DisplayName("Weather conditions")]
		public string WeatherConditions
		{
			get;
			set;
		}

		#endregion Report Members

	}
}
