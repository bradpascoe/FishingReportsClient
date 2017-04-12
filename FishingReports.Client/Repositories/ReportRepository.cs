using System;
using System.Collections.Generic;

using FishingReports.Client.Model;
using ServiceDto = FishingReports.Client.ReportService;
using ServiceModel = FishingReports.Model;

namespace FishingReports.Client.Repositories
{
	public class ReportRepository : IReportRepository
	{
		#region Constructors

		internal ReportRepository()
		{
		}

		#endregion Constructors

		#region ReportRepository Members

		public Report GetReport(int reportId)
		{
			using (ServiceDto.ReportDataServiceClient client = new ServiceDto.ReportDataServiceClient())
			{
				var report = _TranslateToReport(client.GetReport(reportId));

				if (report.NumberOfImages > 0)
				{
					var images = mImageRepository.GetImages(report.ReportId);

					report.Images = images;
				}
				else
				{
					report.Images = new ReportImage[] { };
				}

				return report;
			}
		}

		public ReportSearchResult GetReports()
		{
			using (ServiceDto.ReportDataServiceClient client = new ServiceDto.ReportDataServiceClient())
			{
				var searchResult = client.GetReports(new ServiceDto.ReportFilter
				{
					DateFrom = DateTime.Today.AddMonths(-2),
					DateTo = DateTime.Today
				});

				return _TranslateToSearchResult(searchResult);
			}
		}

		public ReportSearchResult GetReports(ReportFilter filter)
		{
			using (ServiceDto.ReportDataServiceClient client = new ServiceDto.ReportDataServiceClient())
			{
				var serviceFilter = new ServiceDto.ReportFilter
				{
					Location = filter.Location,
					MonthFrom = filter.Month,
					MonthTo = filter.Month,
					Year = filter.Year
				};

				return _TranslateToSearchResult(client.GetReports(serviceFilter));
			}
		}

		public int[] GetYears()
		{
			using (ServiceDto.ReportDataServiceClient client = new ServiceDto.ReportDataServiceClient())
			{
				return client.GetAllYears();
			}
		}

		public int SaveReport(Report report)
		{
			using (ServiceDto.ReportDataServiceClient client = new ServiceDto.ReportDataServiceClient())
			{
				var entity = new ServiceDto.ReportEntity
				{
					ReportDate = report.ReportDate,
					FlowRate = report.FlowRate,
					TotalFish = report.TotalFish,
					WaterConditions = report.WaterConditions,
					WeatherConditions = report.WeatherConditions,
					Username = report.Username,
					ReportType = _TranslateToReportType(report.ReportType),
					ReportDescription = report.Description,
					AccessID = report.AccessId,
				};

				int reportId = client.SaveReport(entity);

				mImageRepository.SynchronizeImagesForReport(reportId, report.Images);

				return reportId;
			}
		}

		public void UpdateReport(Report report)
		{
			using (ServiceDto.ReportDataServiceClient client = new ServiceDto.ReportDataServiceClient())
			{
				var reportEntity = client.GetReport(report.ReportId);

				reportEntity.FlowRate = report.FlowRate;
				reportEntity.ReportDescription = report.Description;
				reportEntity.TotalFish = report.TotalFish;
				reportEntity.ReportType = _TranslateToReportType(report.ReportType);
				reportEntity.ReportDate = report.ReportDate;
				reportEntity.WeatherConditions = report.WeatherConditions;
				reportEntity.WaterConditions = report.WaterConditions;

				client.UpdateReport(reportEntity);

				mImageRepository.SynchronizeImagesForReport(report.ReportId, report.Images);
			}
		}

		#endregion ReportRepository Members

		#region Fields

		private readonly IImageRepository mImageRepository = RepositoryFactory.CreateImageRepository();

		#endregion Fields

		#region Private Members

		private Report _TranslateToReport(ServiceDto.ReportEntity entity)
		{
			return new Report
			{
				Access = entity.Access,
				Location = entity.Location,
				WeatherConditions = entity.WeatherConditions,
				WaterConditions = entity.WaterConditions,
				FlowRate = entity.FlowRate,
				Description = entity.ReportDescription,
				NumberOfImages = entity.NumberOfImages,
				ReportDate = entity.ReportDate,
				ReportId = entity.ReportID,
				ReportType = _TranslateToReportType(entity.ReportType),
				State = entity.State,
				TotalFish = entity.TotalFish,
				Username = entity.Username,
			};
		}

		private ReportListItem _TranslateToReportListItem(ServiceDto.ReportListItem entity)
		{
			return new ReportListItem
			{
				Access = entity.Access,
				Location = entity.Location,
				WeatherConditions = entity.WeatherConditions,
				WaterConditions = entity.WaterConditions,
				FlowRate = entity.FlowRate,
				Description = entity.ReportDescription,
				NumberOfImages = entity.NumberOfImages,
				ReportDate = entity.ReportDate,
				ReportId = entity.ReportId,
				ReportType = _TranslateToReportType(entity.ReportType),
				State = entity.State,
				TotalFish = entity.TotalFish,
				Username = entity.Username,
			};
		}

		private ReportListItem[] _TranslateToReportListItems(ServiceDto.ReportListItem[] reportEntities)
		{
			List<ReportListItem> listItems = new List<ReportListItem>();

			foreach (var entity in reportEntities)
			{
				listItems.Add(_TranslateToReportListItem(entity));
			}

			return listItems.ToArray();
		}

		private ReportType _TranslateToReportType(ServiceDto.ReportType reportType)
		{
			ReportType dtoReportType;

			switch (reportType)
			{
				case ServiceDto.ReportType.Float:
					dtoReportType = ReportType.Float;
					break;

				case ServiceDto.ReportType.SaltFlats:
					dtoReportType = ReportType.SaltFlats;
					break;

				case ServiceDto.ReportType.Lake:
					dtoReportType = ReportType.Lake;
					break;

				case ServiceDto.ReportType.Wade:
					dtoReportType = ReportType.Wade;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(reportType), reportType, null);
			}

			return dtoReportType;
		}

		private ServiceDto.ReportType _TranslateToReportType(ServiceModel.ReportType reportType)
		{
			ServiceDto.ReportType dtoReportType;

			switch (reportType)
			{
				case ServiceModel.ReportType.Float:
					dtoReportType = ServiceDto.ReportType.Float;
					break;

				case ServiceModel.ReportType.SaltFlats:
					dtoReportType = ServiceDto.ReportType.SaltFlats;
					break;

				case ServiceModel.ReportType.Lake:
					dtoReportType = ServiceDto.ReportType.Lake;
					break;

				case ServiceModel.ReportType.Wade:
					dtoReportType = ServiceDto.ReportType.Wade;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(reportType), reportType, null);
			}

			return dtoReportType;
		}

		private ServiceDto.ReportType _TranslateToReportType(ReportType dtoReportType)
		{
			ServiceDto.ReportType reportType;

			switch (dtoReportType)
			{
				case ReportType.Float:
					reportType = ServiceDto.ReportType.Float;
					break;

				case ReportType.Wade:
					reportType = ServiceDto.ReportType.Wade;
					break;

				case ReportType.SaltFlats:
					reportType = ServiceDto.ReportType.SaltFlats;
					break;

				case ReportType.Lake:
					reportType = ServiceDto.ReportType.Lake;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(dtoReportType), dtoReportType, null);
			}

			return reportType;
		}

		private ReportSearchResult _TranslateToSearchResult(ServiceDto.ReportSearchResult searchResult)
		{
			var reportSearchResult = new ReportSearchResult
			{
				AverageNumberOfFish = Math.Round(searchResult.AverageFish, 3),
				NumberOfReports = searchResult.NumberOfReports,
				TotalNumberOfFish = searchResult.TotalFish
			};

			reportSearchResult.Results = _TranslateToReportListItems(searchResult.Results);

			return reportSearchResult;
		}

		#endregion Private Members

	}
}
