using System.Threading.Tasks;
using JwtAuthentication.Server.ServiceLayer.Models;

namespace JwtAuthentication.Server.ServiceLayer.Services
{
	public interface IBookService
	{
		public Task<BookDto> ReadBook(long id, string acessToken);
	}
}
