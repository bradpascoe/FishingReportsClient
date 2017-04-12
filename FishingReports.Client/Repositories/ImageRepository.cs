using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

using FishingReports.Client.Model;

using ServiceDto = FishingReports.Client.ReportService;
using FishingReports.Model;

namespace FishingReports.Client.Repositories
{
	public class ImageRepository : IImageRepository
	{
		#region Constructors

		internal ImageRepository()
		{
		}

		#endregion Constructors

		#region ImageRepository Members

		public ReportImage[] GetImages(int reportId)
		{
			using (ServiceDto.ReportDataServiceClient client = new ServiceDto.ReportDataServiceClient())
			{
				var imageEntities = _ProcessImages(client.GetImages(reportId));

				return _TranslateToImages(imageEntities);
			}
		}

		public ReportImage[] GetImages(Guid locationId)
		{
			using (ServiceDto.ReportDataServiceClient client = new ServiceDto.ReportDataServiceClient())
			{
				var imageEntities = _ProcessImages(client.GetImagesByLocation(locationId));

				return _TranslateToImages(imageEntities);
			}
		}

		public void SynchronizeImagesForReport(int reportId, ReportImage[] currentImages)
		{
			string imageDirectory = _ImageDirectory;

			var imageEntities = _TranslateToImageEntities(reportId, currentImages).ToArray();

			IEnumerable<ServiceDto.ImageEntity> newImages = imageEntities.Where(image => !image.ReportID.HasValue);

			foreach (ServiceDto.ImageEntity image in newImages)
			{
				string newImagePath = "Report" + reportId + Path.GetFileName(image.ImageName);
				string newThumbPath = "Report" + reportId + Path.GetFileName(image.ThumbNailName);

				System.Drawing.Image imageFile =
								System.Drawing.Image.FromFile(image.ImageName);
				System.Drawing.Image thumbImage =
					System.Drawing.Image.FromFile(image.ThumbNailName);

				imageFile.Save(Path.Combine(imageDirectory, newImagePath));
				thumbImage.Save(Path.Combine(imageDirectory, newThumbPath));

				imageFile.Dispose();
				thumbImage.Dispose();

				File.Delete(image.ImageName);
				File.Delete(image.ThumbNailName);

				image.ImageName = newImagePath;
				image.ThumbNailName = newThumbPath;
			}

			using (ServiceDto.ReportDataServiceClient client = new ServiceDto.ReportDataServiceClient())
			{
				client.SynchronizeImagesForReport(reportId, imageEntities.ToArray());
			}
		}

		public ReportImage UploadNewImage(Stream imageStream, string imageFileName, string username)
		{
			//
			// Grab the image name and the future path of the image and its
			// thumbnail.
			//
			string imageName = Path.GetFileName(imageFileName);
			string thumbNailPath = Path.Combine(_ImageTempDirectory, username + "Thumb" + imageName);
			string imagePath = Path.Combine(_ImageTempDirectory, username + imageName);

			//
			// Create image from the uploaded file
			//
			System.Drawing.Image newImage =
				System.Drawing.Image.FromStream(imageStream);

			//
			// Save the image and the thumbnail on the server.
			//
			newImage.Save(imagePath);
			newImage.GetThumbnailImage(100, 75, null, IntPtr.Zero).Save(thumbNailPath);

			//
			// Create a new Images data row and add it to the collection 
			// stored in the Session variable.
			//
			var imageEntity = new ServiceDto.ImageEntity();
			imageEntity.ThumbNailName = thumbNailPath;
			imageEntity.ImageName = imagePath;
			imageEntity.ImageID = _NewImageId;

			return _TranslateToImage(imageEntity);
		}

		#endregion ImageRepository Members

		#region Fields

		private const string IMAGE_PATH = "ImagePath";
		private const string TEMP_FILE_PATH = "TempImagePath";
		private static int mNewImageId;

		#endregion Fields

		#region Private Members

		private string _ImageDirectory => ConfigurationManager.AppSettings[IMAGE_PATH];

		private string _ImageTempDirectory => ConfigurationManager.AppSettings[TEMP_FILE_PATH];

		private static int _NewImageId
		{
			get
			{
				mNewImageId--;

				return mNewImageId;
			}
		}

		private IList<ServiceDto.ImageEntity> _ProcessImages(IEnumerable<ServiceDto.ImageEntity> images)
		{
			List<ServiceDto.ImageEntity> list = new List<ServiceDto.ImageEntity>();

			string imageDirectory = _ImageDirectory;

			foreach (ServiceDto.ImageEntity image in images)
			{
				image.ThumbNailName = Path.Combine(imageDirectory, image.ThumbNailName);
				image.ImageName = Path.Combine(imageDirectory, image.ImageName);

				list.Add(image);
			}

			return list;
		}

		private ReportImage _TranslateToImage(ServiceDto.ImageEntity imageEntity)
		{
			int urlThumbIndex = imageEntity.ThumbNailName.LastIndexOf("Images", StringComparison.Ordinal);
			int urlImageIndex = imageEntity.ImageName.LastIndexOf("Images", StringComparison.Ordinal);

			return new ReportImage
			{
				ImageName = "../" + imageEntity.ImageName.Substring(urlImageIndex,
					imageEntity.ImageName.Length - urlImageIndex),
				OriginalImageName = imageEntity.ImageName,
				ThumbnailName = "../" + imageEntity.ThumbNailName.Substring(urlThumbIndex,
					imageEntity.ThumbNailName.Length - urlThumbIndex),
				OriginalThumbnailName = imageEntity.ThumbNailName,
				ImageId = imageEntity.ImageID
			};
		}

		private IEnumerable<ServiceDto.ImageEntity> _TranslateToImageEntities(int reportId, ReportImage[] images)
		{
			List<ServiceDto.ImageEntity> imageEntities = new List<ServiceDto.ImageEntity>();

			foreach (var image in images)
			{
				imageEntities.Add(new ServiceDto.ImageEntity
				{
					ImageID = image.ImageId,
					ImageName = image.OriginalImageName,
					ThumbNailName = image.OriginalThumbnailName,
					ReportID = image.ImageId > 0
						? reportId
						: (int?)null
				});
			}

			return imageEntities;
		}

		private ReportImage[] _TranslateToImages(IList<ServiceDto.ImageEntity> imageEntities)
		{
			List<ReportImage> images = new List<ReportImage>();

			foreach (var imageEntity in imageEntities)
			{
				images.Add(_TranslateToImage(imageEntity));
			}

			return images.ToArray();
		}

		#endregion Private Members

	}
}
