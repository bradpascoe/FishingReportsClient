using System;
using System.Collections.Generic;
using System.IO;

using FishingReports.Client.Model;
using FishingReports.Model;

namespace FishingReports.Client.Repositories
{
	public interface IImageRepository
	{
		#region IImageRepository Members

		ReportImage[] GetImages(int reportId);

		ReportImage[] GetImages(Guid locationId);

		void SynchronizeImagesForReport(int reportId, ReportImage[] currentImages);

		ReportImage UploadNewImage(Stream imageStream, string imageFileName, string username);

		#endregion IImageRepository Members

	}
}
