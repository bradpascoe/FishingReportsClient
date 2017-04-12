using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using FishingReports.Client.Model;

using FishingReportsProxy;

namespace FishingReportsMVC.Models
{
	public class ReportEditViewModel
	{
		#region ReportEditViewModel Members

		[FileSize(10240)]
		[FileTypes("jpg,jpeg,png")]
		public HttpPostedFileBase NewImageFile { get; set; }

		public Report Report
		{
			get;
			set;
		}

		#endregion ReportEditViewModel Members

	}

	public class FileTypesAttribute : ValidationAttribute
	{
		#region Constructors

		public FileTypesAttribute(string types)
		{
			_types = types.Split(',').ToList();
		}

		#endregion Constructors

		#region FileTypesAttribute Members

		public override string FormatErrorMessage(string name)
		{
			return string.Format("Invalid file type. Only the following types {0} are supported.", String.Join(", ", _types));
		}

		public override bool IsValid(object value)
		{
			if (value == null) return true;

			var fileExt = System.IO.Path.GetExtension((value as HttpPostedFileBase).FileName).Substring(1);
			return _types.Contains(fileExt, StringComparer.OrdinalIgnoreCase);
		}

		#endregion FileTypesAttribute Members

		#region Fields

		private readonly List<string> _types;

		#endregion Fields

	}

	public class FileSizeAttribute : ValidationAttribute
	{
		#region Constructors

		public FileSizeAttribute(int maxSize)
		{
			_maxSize = maxSize;
		}

		#endregion Constructors

		#region FileSizeAttribute Members

		public override string FormatErrorMessage(string name)
		{
			return string.Format("The file size should not exceed {0}", _maxSize);
		}

		public override bool IsValid(object value)
		{
			if (value == null) return true;

			return (value as HttpPostedFileBase).ContentLength <= _maxSize;
		}

		#endregion FileSizeAttribute Members

		#region Fields

		private readonly int _maxSize;

		#endregion Fields

	}

	public class ReportAddViewModel
	{
		#region ReportAddViewModel Members

		public IEnumerable<SelectListItem> Accesses
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> Locations
		{
			get;
			set;
		}

		[DisplayName("New access")]
		public string NewAccess
		{
			get;
			set;
		}

		[DisplayName("New location")]
		public string NewLocation
		{
			get;
			set;
		}

		[DisplayName("New state")]
		public string NewState
		{
			get;
			set;
		}

		public Report Report
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> States
		{
			get;
			set;
		}

		#endregion ReportAddViewModel Members

	}

	public class ReportDetailViewModel
	{
		#region ReportDetailViewModel Members

		public string LoggedInUsername
		{
			get;
			set;
		}

		public Report Report
		{
			get;
			set;
		}

		#endregion ReportDetailViewModel Members

	}

	public class HomeViewModel
	{
		#region HomeViewModel Members

		public double AverageNumberOfFish
		{
			get;
			set;
		}

		public bool IsFiltered
		{
			get;
			set;
		}

		public bool IsLoggedIn
		{
			get;
			set;
		}

		public List<SelectListItem> Locations
		{
			get;
			set;
		}

		public List<SelectListItem> Months
		{
			get;
			set;
		}

		public int NumberOfReports
		{
			get;
			set;
		}

		public ReportFilter ReportFilter
		{
			get;
			set;
		}

		public IEnumerable<ReportListItem> Reports
		{
			get;
			set;
		}

		public int TotalNumberOfFish
		{
			get;
			set;
		}

		public string Username
		{
			get;
			set;
		}

		public List<SelectListItem> Years
		{
			get;
			set;
		}

		#endregion HomeViewModel Members

	}

	public class ExternalLoginConfirmationViewModel
	{
		#region ExternalLoginConfirmationViewModel Members

		[Required]
		[Display(Name = "Email")]
		public string Email { get; set; }

		#endregion ExternalLoginConfirmationViewModel Members

	}

	public class ExternalLoginListViewModel
	{
		#region ExternalLoginListViewModel Members

		public string ReturnUrl { get; set; }

		#endregion ExternalLoginListViewModel Members

	}

	public class SendCodeViewModel
	{
		#region SendCodeViewModel Members

		public ICollection<SelectListItem> Providers { get; set; }

		public bool RememberMe { get; set; }

		public string ReturnUrl { get; set; }

		public string SelectedProvider { get; set; }

		#endregion SendCodeViewModel Members

	}

	public class VerifyCodeViewModel
	{
		#region VerifyCodeViewModel Members

		[Required]
		[Display(Name = "Code")]
		public string Code { get; set; }

		[Required]
		public string Provider { get; set; }

		[Display(Name = "Remember this browser?")]
		public bool RememberBrowser { get; set; }

		public bool RememberMe { get; set; }

		public string ReturnUrl { get; set; }

		#endregion VerifyCodeViewModel Members

	}

	public class ForgotViewModel
	{
		#region ForgotViewModel Members

		[Required]
		[Display(Name = "Email")]
		public string Email { get; set; }

		#endregion ForgotViewModel Members

	}

	public class LoginViewModel
	{
		#region LoginViewModel Members

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required]
		[Display(Name = "Username")]
		public string Username { get; set; }

		#endregion LoginViewModel Members

	}

	public class RegisterViewModel
	{
		#region RegisterViewModel Members

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required]
		public string Username
		{
			get;
			set;
		}

		#endregion RegisterViewModel Members

	}

	public class ResetPasswordViewModel
	{
		#region ResetPasswordViewModel Members

		public string Code { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		#endregion ResetPasswordViewModel Members

	}

	public class ForgotPasswordViewModel
	{
		#region ForgotPasswordViewModel Members

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		#endregion ForgotPasswordViewModel Members

	}
}
