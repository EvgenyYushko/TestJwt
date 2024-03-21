using System.Threading.Tasks;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Model;

namespace JwtAuthentication.AuthorizeServer.ServiceLayer.Services
{
	public interface IAuthenticationService
	{
		Task<bool> Register(UserClient userClient);

		public Task<UserServer> Login(UserClient userClient);

		public Task<UserServer> Refresh(UserClient userClient);

		public Task<bool> Revoke(string token);

		Task<bool> CheckToken(string authToken);
	}
}