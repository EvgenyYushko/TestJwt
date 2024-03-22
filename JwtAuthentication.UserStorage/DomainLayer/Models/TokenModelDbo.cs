using System;

namespace JwtAuthentication.UserStorage.DomainLayer.Models
{
	public class TokenModelDbo
	{
		public string AccessToken { get; set; }
		public DateTime? AccessTokenExpiry { get; set; }
		public string RefreshToken { get; set; }
		public DateTime? RefreshTokenExpiry { get; set; }
	}
}
