using System.Threading.Tasks;
using JwtAuthentication.Server.ServiceLayer.Models;

namespace JwtAuthentication.Server.ServiceLayer.Services
{
	public interface IUserManager
	{
		Task<ServiceUser> FindByNameAsync(string userName);
		Task<bool> CreateAsync(ServiceUser user, string password);
		Task<bool> CheckPasswordAsync(ServiceUser user, string password);
		Task UpdateAsync(ServiceUser user);


	}
}