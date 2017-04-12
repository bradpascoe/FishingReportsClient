using System;
using System.Collections.Generic;
using System.Linq;

namespace FishingReports.Client.Model
{
	public class ValidationResult
	{
		#region Constructors

		public ValidationResult(IDictionary<string, string> brokenRules)
		{
			BrokenRules = brokenRules;
		}

		#endregion Constructors

		#region ValidationResult Members

		public IDictionary<string, string> BrokenRules
		{
			get;
			private set;
		}

		public bool IsValid
		{
			get
			{
				return BrokenRules.Count() == 0;
			}
		}

		public string GetErrorMessage()
		{
			string message = string.Empty;

			foreach (KeyValuePair<string, string> rule in BrokenRules)
			{
				message += rule.Value + Environment.NewLine;
			}

			return message.Trim();
		}

		#endregion ValidationResult Members

	}
}
