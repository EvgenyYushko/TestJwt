using System;

namespace JwtAuthentication.AuthorizeServer.ServiceLayer.Model
{
	public class UserClient
	{
		public string Username { get; set; }

		public string Password { get; set; }

		public string Email { get; set; }

		public string AccessToken { get; set; }

		public string RefreshToken { get; set; }

		public DateTime Expiration { get; set; }
	}
}