using System.Threading.Tasks;
using JwtAuthentication.Server.ServiceLayer.Models;

namespace JwtAuthentication.Server.ServiceLayer.Services
{
	public interface IAuthenticationService
	{
		public Task<bool> Register(RegistrationModel model);

		public Task<LoginResponse> Login(LoginModel model);

		public Task<LoginResponse> Refresh(RefreshModel model);

		public Task<bool> Revoke();
	}
}