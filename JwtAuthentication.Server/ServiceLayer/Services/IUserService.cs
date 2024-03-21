using System.Threading.Tasks;
using JwtAuthentication.Server.ServiceLayer.Models;

namespace JwtAuthentication.Server.ServiceLayer.Services
{
	public interface IUserService
	{
		Task<bool> Register(RegistrationModel model);

		Task<bool> CheckToken(string authToken);
	}
}
