using System.Threading.Tasks;
using JwtAuthentication.UserStorage.ServiceLayer.Model;

namespace JwtAuthentication.UserStorage.ServiceLayer.Services
{
	public interface IUserManager
	{
		Task<ServerUser> FindByNameAsync(string userName);
		Task<bool> CreateAsync(ServerUser user, string password);
		Task<bool> CheckPasswordAsync(ServerUser user, string password);
		Task UpdateAsync(ServerUser user);
	}
}