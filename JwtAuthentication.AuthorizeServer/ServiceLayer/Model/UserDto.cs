using System;

namespace JwtAuthentication.AuthorizeServer.ServiceLayer.Model
{
	public class UserDto
	{
		public string UserName { get; set; }
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public DateTime? RefreshTokenExpiry { get; set; }
	}
}
