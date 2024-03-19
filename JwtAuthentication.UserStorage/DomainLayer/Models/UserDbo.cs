using System;

namespace JwtAuthentication.UserStorage.DomainLayer.Models
{
	public class UserDbo
	{
		public string UserName { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpiry { get; set; }
		public string Password { get; set; }
	}
}