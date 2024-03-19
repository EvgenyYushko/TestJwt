namespace JwtAuthentication.AuthorizeServer.ServiceLayer.Model
{
	public class RefreshModel
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
	}
}
