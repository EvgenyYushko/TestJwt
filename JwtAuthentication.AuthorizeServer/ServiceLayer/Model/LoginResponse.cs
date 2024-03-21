using System;

namespace JwtAuthentication.AuthorizeServer.ServiceLayer.Model
{
	public class LoginResponse
	{
		public string AccessToken { get; set; }
		public DateTime Expiration { get; set; }
		public string RefreshToken { get; set; }
	}
}
