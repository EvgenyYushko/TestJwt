using System;

namespace JwtAuthentication.AuthorizeServer.ServiceLayer.Model
{
	public class UserDto
	{
		public string UserName { get; set; }
		public TokenModelDto TokenModel { get; set; }
	}
}