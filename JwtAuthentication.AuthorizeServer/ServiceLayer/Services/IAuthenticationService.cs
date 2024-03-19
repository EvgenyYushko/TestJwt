using System.Threading.Tasks;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Model;

namespace JwtAuthentication.AuthorizeServer.ServiceLayer.Services
{
	public interface IAuthenticationService
	{
		public Task<LoginResponse> Login(LoginModel model);

		public Task<LoginResponse> Refresh(RefreshModel model);

		public Task<bool> Revoke();
	}
}