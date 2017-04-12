using FishingReports.Client.Model;
using FishingReports.Client.Repositories;

namespace FishingReportsProxy
{
	internal class LoginProxy : ILoginProxy
	{
		#region LoginProxy Members

		public bool PerformLogin(string username, string password)
		{
			return mLoginRepository.AttemptLogin(username, password);
		}

		public void CreateUser( User user )
		{
			mLoginRepository.CreateUser( user );
		}

		public ValidationResult ValidateNewUser( User user )
		{
			return mLoginRepository.ValidateNewUser( user );
		}

		#endregion LoginProxy Members

		#region Fields

		private readonly ILoginRepository mLoginRepository = RepositoryFactory.CreateLoginRepository();

		#endregion Fields

	}
}
