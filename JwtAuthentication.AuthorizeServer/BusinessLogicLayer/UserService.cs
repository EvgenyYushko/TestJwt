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
				TokenModel = new()
				{
					AccessToken = userDbo.TokenModel.AccessToken,
					RefreshToken = userDbo.TokenModel.RefreshToken,
					RefreshTokenExpiry = userDbo.TokenModel.RefreshTokenExpiry,
				},
			});
		}

		public Task<bool> Create(UserDto userDto, string password)
		{
			_userStorage.Users.Add(userDto.UserName, new UserDbo
			{
				UserName = userDto.UserName,
				TokenModel = new()
				{
					AccessToken = userDto.TokenModel.AccessToken,
					RefreshToken = userDto.TokenModel.RefreshToken,
					RefreshTokenExpiry = userDto.TokenModel.RefreshTokenExpiry,
				},
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
				userDbo.TokenModel.AccessToken = userDto.TokenModel.AccessToken;
				userDbo.TokenModel.AccessTokenExpiry = userDto.TokenModel.AccessTokenExpiry;
				userDbo.TokenModel.RefreshToken = userDto.TokenModel.RefreshToken;
				userDbo.TokenModel.RefreshTokenExpiry = userDto.TokenModel.RefreshTokenExpiry;
			}

			return Task.CompletedTask;
		}
	}
}