using System.Threading.Tasks;
using JwtAuthentication.Server.DomainLayer.Repositories;
using JwtAuthentication.Server.ServiceLayer.Models;
using JwtAuthentication.Server.ServiceLayer.Services;

namespace JwtAuthentication.Server.BusinessLogicLayer
{
	public class BookService : IBookService
	{
		private readonly IBookRepository _bookRepository;

		public BookService(IBookRepository bookRepository)
		{
			_bookRepository = bookRepository;
		}

		public async Task<BookDto> ReadBook(long id, string acessToken)
		{
			var bookDbo = await _bookRepository.ReadBook(id);

			return new BookDto
			{
				Id = bookDbo.Id,
				Rating = bookDbo.Rating,
				Title = bookDbo.Title
			};
		}
	}
}