using System.Threading.Tasks;
using JwtAuthentication.Server.ServiceLayer.Models;
using JwtAuthentication.Server.ServiceLayer.Services;

namespace JwtAuthentication.Server.BusinessLogicLayer
{
	public class UserManager : IUserManager
	{
		public Task<ServiceUser> FindByNameAsync(string userName)
		{
			throw new System.NotImplementedException();
		}

		public Task<bool> CreateAsync(ServiceUser user, string password)
		{
			throw new System.NotImplementedException();
		}

		public Task<bool> CheckPasswordAsync(ServiceUser user, string password)
		{
			throw new System.NotImplementedException();
		}

		public Task UpdateAsync(ServiceUser user)
		{
			throw new System.NotImplementedException();
		}
	}
}
