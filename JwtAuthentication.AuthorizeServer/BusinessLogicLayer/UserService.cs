using System.Threading.Tasks;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Model;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Services;
using JwtAuthentication.UserStorage.DomainLayer.Models;

namespace JwtAuthentication.AuthorizeServer.BusinessLogicLayer
{
	public class UserService : IUserService
	{
		private readonly UserStorage.DataAccessLayer.UserStorage _userStorage;

		public UserService(UserStorage.DataAccessLayer.UserStorage userStorage)
		{
			_userStorage = userStorage;
		}

		public Task<UserDto> FindByName(string userName)
		{
			if (!_userStorage.Users.TryGetValue(userName, out var userDbo))
			{
				return Task.FromResult((UserDto)null);
			}

			return Task.FromResult(new UserDto
			{
				UserName = userDbo.UserName,
				AccessToken = userDbo.AccessToken,
				RefreshToken = userDbo.RefreshToken,
				RefreshTokenExpiry = userDbo.RefreshTokenExpiry
			});
		}

		public Task<bool> Create(UserDto userDto, string password)
		{
			_userStorage.Users.Add(userDto.UserName, new UserDbo
			{
				UserName = userDto.UserName,
				AccessToken = userDto.AccessToken,
				RefreshToken = userDto.RefreshToken,
				RefreshTokenExpiry = userDto.RefreshTokenExpiry,
				Password = password
			});

			return Task.FromResult(true);
		}

		public Task<bool> CheckPassword(UserDto userDto, string password)
		{
			if (!_userStorage.Users.TryGetValue(userDto.UserName, out var userDbo))
			{
				return Task.FromResult(false);
			}
			return Task.FromResult(password == userDbo.Password);
		}

		public Task Update(UserDto userDto)
		{
			if (_userStorage.Users.TryGetValue(userDto.UserName, out var userDbo))
			{
				userDbo.UserName = userDto.UserName;
				userDbo.AccessToken = userDto.AccessToken;
				userDbo.RefreshToken = userDto.RefreshToken;
				userDbo.RefreshTokenExpiry = userDto.RefreshTokenExpiry;
			}

			return Task.CompletedTask;
		}
	}
}
