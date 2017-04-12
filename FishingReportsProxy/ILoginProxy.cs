using FishingReports.Client.Model;

namespace FishingReportsProxy
{
	public interface ILoginProxy
	{
		#region ILoginProxy Members

		void CreateUser(User user);

		bool PerformLogin(string username, string password);

		ValidationResult ValidateNewUser(User user);

		#endregion ILoginProxy Members

	}
}
