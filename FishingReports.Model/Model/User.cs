namespace FishingReports.Client.Model
{
	public class User
	{
		#region User Members

		public string Email
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		/// <summary>
		/// Only used when creating new objects or changing a password.
		/// </summary>
		public string Password2
		{
			get;
			set;
		}

		public string Username
		{
			get;
			set;
		}

		#endregion User Members

	}
}
