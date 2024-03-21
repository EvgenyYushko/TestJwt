using System.Threading.Tasks;
using JwtAuthentication.Server.DomainLayer.Models;

namespace JwtAuthentication.Server.ServiceLayer.Services
{
	public interface IBookService
	{
		public Task<BookReview> ReadBook(string acessToken);
	}
}
