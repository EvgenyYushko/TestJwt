using System.Threading.Tasks;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Model;

namespace JwtAuthentication.AuthorizeServer.ServiceLayer.Services
{
	public interface IUserService
	{
		Task<UserDto> FindByName(string userName);

		Task<bool> Create(UserDto userDto, string password);

		Task<bool> CheckPassword(UserDto userDto, string password);

		Task Update(UserDto userDto);
	}
}