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
				TokenModel = ToDtoTokens(userDbo.TokenModel)
			});
		}

		public Task<bool> Create(UserDto userDto, string password)
		{
			_userStorage.Users.Add(userDto.UserName, new UserDbo
			{
				UserName = userDto.UserName,
				//TokenModel = ToDboTokens(userDto.TokenModel),
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
				userDbo.TokenModel = ToDboTokens(userDto.TokenModel);
			}

			return Task.CompletedTask;
		}

		private TokenModelDbo ToDboTokens(TokenModelDto tokenDto)
		{
			if (tokenDto is null)
			{
				return null;
			}

			var tokenDbo = new TokenModelDbo
			{
				AccessToken = tokenDto.AccessToken,
				AccessTokenExpiry = tokenDto.AccessTokenExpiry,
				RefreshToken = tokenDto.RefreshToken,
				RefreshTokenExpiry = tokenDto.RefreshTokenExpiry,
			};

			if (tokenDto.ParentToken is not null)
			{
				tokenDbo.ParentToken = ToDboTokens(tokenDto.ParentToken);
			}

			return tokenDbo;
		}

		private TokenModelDto ToDtoTokens(TokenModelDbo tokenDbo)
		{
			if (tokenDbo is null)
			{
				return null;
			}

			var tokenDto = new TokenModelDto
			{
				AccessToken = tokenDbo.AccessToken,
				AccessTokenExpiry = tokenDbo.AccessTokenExpiry,
				RefreshToken = tokenDbo.RefreshToken,
				RefreshTokenExpiry = tokenDbo.RefreshTokenExpiry,
			};

			if (tokenDbo.ParentToken is not null)
			{
				tokenDto.ParentToken = ToDtoTokens(tokenDbo.ParentToken);
			}

			return tokenDto;
		}
	}
}