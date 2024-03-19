using System;

namespace JwtAuthentication.UserStorage.ServiceLayer.Model
{
	public class ServerUser
	{
		public string UserName { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpiry { get; set; }
	}
}
