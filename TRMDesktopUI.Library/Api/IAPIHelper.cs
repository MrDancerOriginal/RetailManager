using System.Threading.Tasks;
using TRMDesktopUI.Library.Model;

namespace TRMDesktopUI.Library.Api
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLogInUserInfo(string token);
    }
}