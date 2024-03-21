using System.Threading.Tasks;
using JwtAuthentication.UserStorage.DomainLayer.Models;
using JwtAuthentication.UserStorage.ServiceLayer.Model;
using JwtAuthentication.UserStorage.ServiceLayer.Services;

namespace JwtAuthentication.UserStorage.BusinessLogicLayer
{
	public class UserManager : IUserManager
	{
		private readonly DataAccessLayer.UserStorage _userStorage;

		public UserManager(DataAccessLayer.UserStorage userStorage)
		{
			_userStorage = userStorage;
		}

		public Task<ServerUser> FindByName(string userName)
		{
			if (!_userStorage.Users.TryGetValue(userName, out var userDbo))
			{
				return Task.FromResult((ServerUser)null);
			}

			return Task.FromResult(new ServerUser
			{
				UserName = userDbo.UserName,
				AccessToken = userDbo.AccessToken,
				RefreshToken = userDbo.RefreshToken,
				RefreshTokenExpiry = userDbo.RefreshTokenExpiry
			});
		}

		public Task<bool> Create(ServerUser user, string password)
		{
			_userStorage.Users.Add(user.UserName, new UserDbo
			{
				UserName = user.UserName,
				AccessToken = user.AccessToken,
				RefreshToken = user.RefreshToken,
				RefreshTokenExpiry = user.RefreshTokenExpiry,
				Password = password
			});

			return Task.FromResult(true);
		}

		public Task<bool> CheckPassword(ServerUser user, string password)
		{
			if (!_userStorage.Users.TryGetValue(user.UserName, out var userDbo))
			{
				return Task.FromResult(false);
			}
			return Task.FromResult(password == userDbo.Password);
		}

		public Task Update(ServerUser user)
		{
			if (_userStorage.Users.TryGetValue(user.UserName, out var userDbo))
			{
				userDbo.UserName = user.UserName;
				userDbo.AccessToken = user.AccessToken;
				userDbo.RefreshToken = user.RefreshToken;
				userDbo.RefreshTokenExpiry = user.RefreshTokenExpiry;
			}

			return Task.CompletedTask;
		}
	}
}
