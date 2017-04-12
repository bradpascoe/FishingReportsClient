using FishingReports.Client.LoginService;
using FishingReports.Client.Model;

using UserEntity = FishingReports.Client.LoginService.UserEntity;

namespace FishingReports.Client.Repositories
{
	public class LoginRepository : ILoginRepository
	{
		#region Constructors

		internal LoginRepository()
		{
		}

		#endregion Constructors

		#region LoginRepository Members

		public bool AttemptLogin(string username, string password)
		{
			using (LoginServiceClient client = new LoginServiceClient())
			{
				return client.AttemptLogin(username, password);
			}
		}

		public void CreateUser(User user)
		{
			using (LoginServiceClient client = new LoginServiceClient())
			{
				client.InsertUser(_TranslateToUserEntity(user));
			}
		}

		public bool DoesUserExist(string username)
		{
			using (LoginServiceClient client = new LoginServiceClient())
			{
				return client.DoesUserExist(username);
			}
		}

		public int GetNumberOfPosts(string user)
		{
			using (LoginServiceClient client = new LoginServiceClient())
			{
				return client.GetNumberOfPosts(user);
			}
		}

		public User GetUser(string user)
		{
			using (LoginServiceClient client = new LoginServiceClient())
			{
				return _TranslateToUser(client.GetUser(user));
			}
		}

		public ValidationResult ValidateNewUser(User user)
		{
			using (LoginServiceClient client = new LoginServiceClient())
			{
				return new ValidationResult(client.ValidateNewUser(
					_TranslateToUserEntity(user)));
			}
		}

		#endregion LoginRepository Members

		#region Private Members

		private User _TranslateToUser(UserEntity userEntity)
		{
			return new User
			{
				Password2 = userEntity.Password2,
				Username = userEntity.Username,
				Password = userEntity.Password,
				Email = userEntity.Email
			};
		}

		private UserEntity _TranslateToUserEntity(User user)
		{
			return new UserEntity
			{
				Password2 = user.Password2,
				Username = user.Username,
				Password = user.Password,
				Email = user.Email,
				Residence = string.Empty
			};
		}

		#endregion Private Members

	}
}
