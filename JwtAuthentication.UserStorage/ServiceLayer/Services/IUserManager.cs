using System.Threading.Tasks;
using JwtAuthentication.UserStorage.ServiceLayer.Model;

namespace JwtAuthentication.UserStorage.ServiceLayer.Services
{
	public interface IUserManager
	{
		Task<ServerUser> FindByName(string userName);
		Task<bool> Create(ServerUser user, string password);
		Task<bool> CheckPassword(ServerUser user, string password);
		Task Update(ServerUser user);
	}
}