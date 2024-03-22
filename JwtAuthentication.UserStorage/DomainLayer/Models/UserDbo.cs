namespace JwtAuthentication.UserStorage.DomainLayer.Models
{
	public class UserDbo
	{
		public string UserName { get; set; }

		public TokenModelDbo TokenModel { get; set; }

		public string Password { get; set; }
	}
}