using System.Threading.Tasks;
using JwtAuthentication.Server.DomainLayer.Models;

namespace JwtAuthentication.Server.DomainLayer.Repositories
{
	public interface IBookRepository
	{
		public Task<BookDbo> ReadBook(long id);
	}
}