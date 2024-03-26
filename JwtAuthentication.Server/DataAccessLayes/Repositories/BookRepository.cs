using System.Threading.Tasks;
using JwtAuthentication.Server.DomainLayer.Models;
using JwtAuthentication.Server.DomainLayer.Repositories;

namespace JwtAuthentication.Server.DataAccessLayes.Repositories
{
	public class BookRepository : IBookRepository
	{
		public Task<BookDbo> ReadBook(long id)
		{
			return Task.FromResult(new BookDbo
			{
				Id = 464318,
				Rating = 10,
				Title = "Book title"
			});
		}
	}
}