using System;

namespace JwtAuthentication.AuthorizeServer.ServiceLayer.Model
{
	public class TokenModelDto
	{
		public string AccessToken { get; set; }
		public DateTime? AccessTokenExpiry { get; set; }
		public string RefreshToken { get; set; }
		public DateTime? RefreshTokenExpiry { get; set; }
	}
}
