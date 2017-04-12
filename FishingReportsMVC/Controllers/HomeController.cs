using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

using FishingReports.Client.Model;

using FishingReportsMVC.Models;

using FishingReportsProxy;

namespace FishingReportsMVC.Controllers
{
	public class HomeController : Controller
	{
		#region HomeController Members

		public ReportImage[] CurrentReportImages
		{
			get
			{
				return Session[SessionVariables.CurrentReportImages] as ReportImage[];
			}
			set
			{
				Session[SessionVariables.CurrentReportImages] = value;
			}
		}

		private ILocationProxy mLocationProxy;
		public ILocationProxy LocationProxy => mLocationProxy ?? (mLocationProxy = ProxyFactory.CreateLocationProxy());

		private IReportProxy mReportProxy;
		public IReportProxy ReportProxy => mReportProxy ?? (mReportProxy = ProxyFactory.CreateReportProxy());

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		[HttpPost]
		public ActionResult AddNewAccess(string locationId, string newAccess)
		{
			var locationIdGuid = Guid.Parse(locationId);
			LocationProxy.AddAccess(locationIdGuid, newAccess);

			var accesses = LocationProxy.GetAccesses(locationIdGuid);

			SelectList selectAccesses = new SelectList(accesses, "AccessId", "AccessName", 0);
			return Json(selectAccesses);
		}

		[HttpPost]
		public ActionResult AddNewLocation(string stateId, string newLocation)
		{
			var stateIdGuid = Guid.Parse(stateId);
			LocationProxy.AddLocation(stateIdGuid, newLocation);

			var locations = LocationProxy.GetLocations(stateIdGuid);

			SelectList selectLocations = new SelectList(locations, "LocationId", "LocationName", 0);
			return Json(selectLocations);
		}

		[HttpPost]
		public ActionResult AddNewState(string newState)
		{
			LocationProxy.AddState(newState);

			var states = LocationProxy.GetStates();

			SelectList selectedStates = new SelectList(states, "StateId", "StateName", 0);
			return Json(selectedStates);
		}

		public ActionResult AddReport()
		{
			ActionResult detailResult = RedirectToAction("Index");

			string username = _GetLoggedInUsername();

			if (username != null)
			{
				ReportAddViewModel viewModel = new ReportAddViewModel
				{
					Report = ReportProxy.GetNewReport(username),
					States = LocationProxy.GetStates().Select(state => new SelectListItem
					{
						Text = state.StateName,
						Value = state.StateId.ToString()
					})
				};

				_PopulateLocations(viewModel);

				_PopulateAccesses(viewModel);

				CurrentReportImages = viewModel.Report.Images;

				detailResult = View(viewModel);
			}

			return detailResult;
		}

		[HttpPost]
		public ActionResult AddReport(ReportAddViewModel viewModel)
		{
			ActionResult addResult = RedirectToAction("Index");

			string username = _GetLoggedInUsername();

			if (username != null)
			{

				viewModel.Report.Username = username;

				viewModel.Report.Images = CurrentReportImages;

				ReportProxy.AddReport(viewModel.Report);
			}

			return addResult;
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult Details(int reportId)
		{
			ActionResult detailResult = RedirectToAction("Index");

			if (_GetLoggedInUsername() != null)
			{
				Report report = ReportProxy.GetReport(reportId);

				ReportDetailViewModel viewModel = new ReportDetailViewModel
				{
					Report = report,
					LoggedInUsername = _GetLoggedInUsername()
				};

				detailResult = View(viewModel);
			}

			return detailResult;
		}

		public ActionResult Edit(int reportid)
		{
			ActionResult editResult = RedirectToAction("Index");

			Report report = ReportProxy.GetReport(reportid);

			if (report.Username == _GetLoggedInUsername())
			{
				ReportEditViewModel viewModel = new ReportEditViewModel
				{
					Report = report
				};

				CurrentReportImages = report.Images;

				editResult = View(viewModel);
			}

			return editResult;
		}

		[HttpPost]
		public ActionResult Edit(ReportEditViewModel editViewModel)
		{
			editViewModel.Report.Images = CurrentReportImages;

			ReportProxy.UpdateReport(editViewModel.Report);

			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult FilterReports(HomeViewModel viewModel)
		{
			if (viewModel.ReportFilter.IsFilterSet)
			{
				Session[SessionVariables.ReportFilter] = viewModel.ReportFilter;
			}
			else
			{
				Session[SessionVariables.ReportFilter] = null;
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult GetAccesses(string locationId)
		{
			var accesses = LocationProxy.GetAccesses(Guid.Parse(locationId));
			SelectList selectLocations = new SelectList(accesses, "AccessId", "AccessName", 0);

			return Json(selectLocations);
		}

		[HttpPost]
		public ActionResult GetLocations(string stateId)
		{
			var locations = LocationProxy.GetLocations(Guid.Parse(stateId));
			SelectList selectLocations = new SelectList(locations, "LocationId", "LocationName", 0);

			return Json(selectLocations);
		}

		public ActionResult Index()
		{
			var username = _GetLoggedInUsername();

			CurrentReportImages = null;

			HomeViewModel viewModel = new HomeViewModel
			{
				IsLoggedIn = username != null,
				Username = username
			};

			if (viewModel.IsLoggedIn)
			{
				_InitializeHomeViewModel(viewModel);
			}

			ActionResult result = View(viewModel);

			return result;
		}

		public ActionResult Login()
		{
			return RedirectToAction("Login", "Account");
		}

		[HttpPost]
		public ActionResult RemoveImage(int imageId)
		{
			List<ReportImage> existingImages = new List<ReportImage>(CurrentReportImages);
			existingImages = existingImages.Where(image => image.ImageId != imageId).ToList();
			CurrentReportImages = existingImages.ToArray();

			return Json(imageId);
		}

		[HttpPost]
		public ActionResult UploadNewFiles()
		{
			ActionResult uploadResult;

			// Checking no of files injected in Request object  
			if (Request.Files.Count > 0)
			{
				try
				{
					List<ReportImage> newImages = new List<ReportImage>();
					//  Get all files from Request object  
					HttpFileCollectionBase files = Request.Files;
					for (int fileIndex = 0; fileIndex < files.Count; fileIndex++)
					{
						HttpPostedFileBase file = files[fileIndex];

						if (file != null)
						{
							var newImage = ReportProxy.UploadNewImage(file.InputStream, file.FileName, _GetLoggedInUsername());
							newImages.Add(newImage);
						}
					}

					List<ReportImage> existingImages = new List<ReportImage>(CurrentReportImages);
					existingImages.AddRange(newImages);
					CurrentReportImages = existingImages.ToArray();

					SelectList imageList = new SelectList(newImages, "ImageId", "ThumbnailName", 0);
					// Returns message that successfully uploaded  
					uploadResult = Json(new
					{
						success = true,
						files = imageList
					});
				}
				catch (Exception ex)
				{
					uploadResult = Json(new
					{
						success = false,
						responseText = ex.Message
					});
				}
			}
			else
			{
				uploadResult = Json(new
				{
					success = false,
					responseText = "No files selected."
				});
			}

			return uploadResult;
		}

		#endregion HomeController Members

		#region Private Members

		private string _GetLoggedInUsername()
		{
			return Session[SessionVariables.Username] as string;
		}

		private List<SelectListItem> _GetMonths()
		{
			return new List<SelectListItem>
			{
				new SelectListItem(),
				new SelectListItem
				{
					Value = "1",
					Text = "January"
				},
				new SelectListItem
				{
					Value = "2",
					Text = "February"
				},
				new SelectListItem
				{
					Value = "3",
					Text = "March"
				},
				new SelectListItem
				{
					Value = "4",
					Text = "April"
				},
				new SelectListItem
				{
					Value = "5",
					Text = "May"
				},
				new SelectListItem
				{
					Value = "6",
					Text = "June"
				},
				new SelectListItem
				{
					Value = "7",
					Text = "July"
				},
				new SelectListItem
				{
					Value = "8",
					Text = "August"
				},
				new SelectListItem
				{
					Value = "9",
					Text = "September"
				},
				new SelectListItem
				{
					Value = "10",
					Text = "October"
				},
				new SelectListItem
				{
					Value = "11",
					Text = "November"
				},
				new SelectListItem
				{
					Value = "12",
					Text = "December"
				},
			};
		}

		private void _InitializeHomeViewModel(HomeViewModel viewModel)
		{
			ReportSearchResult reports;
			ReportFilter filter = Session[SessionVariables.ReportFilter] as ReportFilter;

			if (filter != null)
			{
				reports = ReportProxy.GetReports(filter);
				viewModel.ReportFilter = filter;
				viewModel.IsFiltered = true;
			}
			else
			{
				reports = ReportProxy.GetReports();
				viewModel.ReportFilter = new ReportFilter();
				viewModel.IsFiltered = false;
			}

			viewModel.Years = new List<SelectListItem>
			{
				new SelectListItem()
			};
			viewModel.Years.AddRange(ReportProxy.GetYears().Select(year => new SelectListItem
			{
				Text = year.ToString(),
				Value = year.ToString()
			}));

			viewModel.Locations = new List<SelectListItem>
			{
				new SelectListItem()
			};
			viewModel.Locations.AddRange(LocationProxy.GetLocations().Select(location => new SelectListItem
			{
				Text = location.ToString(),
				Value = location.ToString()
			}));

			viewModel.Months = _GetMonths();

			viewModel.Reports = new List<ReportListItem>(reports.Results);
			viewModel.NumberOfReports = reports.NumberOfReports;
			viewModel.TotalNumberOfFish = reports.TotalNumberOfFish;
			viewModel.AverageNumberOfFish = reports.AverageNumberOfFish;
		}

		private void _PopulateAccesses(ReportAddViewModel viewModel)
		{
			if (viewModel.Locations.Any())
			{
				viewModel.Accesses = LocationProxy.GetAccesses(Guid.Parse(viewModel.Locations.First().Value)).Select(access => new SelectListItem
				{
					Text = access.AccessName,
					Value = access.AccessId.ToString()
				});
			}
			else
			{
				viewModel.Accesses = new List<SelectListItem>();
			}
		}

		private void _PopulateLocations(ReportAddViewModel viewModel)
		{
			if (viewModel.States.Any())
			{
				viewModel.Locations = LocationProxy.GetLocations(Guid.Parse(viewModel.States.First().Value)).Select(location => new SelectListItem
				{
					Text = location.LocationName,
					Value = location.LocationId.ToString()
				});
			}
			else
			{
				viewModel.Locations = new List<SelectListItem>();
			}
		}

		#endregion Private Members

	}
}