using JwtAuthentication.Server.DomainLayer.Models;

namespace JwtAuthentication.Server.ServiceLayer.Services
{
	public interface IBookService
	{
		public BookReview ReadBook(string acessToken);
	}
}
