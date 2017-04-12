using FishingReports.Client.Model;
using FishingReports.Model;

namespace FishingReports.Client.Repositories
{
    public interface ILoginRepository
    {
        bool AttemptLogin(string username, string password);

        bool DoesUserExist(string username);

        void CreateUser(User user);

        User GetUser(string username);

        int GetNumberOfPosts(string user);

        ValidationResult ValidateNewUser(User user);
    }
}
