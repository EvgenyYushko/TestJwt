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

		public Task<ServerUser> FindByNameAsync(string userName)
		{
			if (!_userStorage.Users.TryGetValue(userName, out var userDbo))
			{
				return Task.FromResult((ServerUser)null);
			}

			return Task.FromResult(new ServerUser
			{
				UserName = userDbo.UserName,
				RefreshToken = userDbo.RefreshToken,
				RefreshTokenExpiry = userDbo.RefreshTokenExpiry
			});
		}

		public Task<bool> CreateAsync(ServerUser user, string password)
		{
			_userStorage.Users.Add(user.UserName, new UserDbo
			{
				UserName = user.UserName,
				RefreshToken = user.RefreshToken,
				RefreshTokenExpiry = user.RefreshTokenExpiry,
				Password = password
			});

			return Task.FromResult(true);
		}

		public Task<bool> CheckPasswordAsync(ServerUser user, string password)
		{
			if (!_userStorage.Users.TryGetValue(user.UserName, out var userDbo))
			{
				return Task.FromResult(false);
			}
			return Task.FromResult(password == userDbo.Password);
		}

		public Task UpdateAsync(ServerUser user)
		{
			if (_userStorage.Users.TryGetValue(user.UserName, out var userDbo))
			{
				userDbo.UserName = user.UserName;
				userDbo.RefreshToken = user.RefreshToken;
				userDbo.RefreshTokenExpiry = user.RefreshTokenExpiry;
			}

			return Task.CompletedTask;
		}
	}
}
