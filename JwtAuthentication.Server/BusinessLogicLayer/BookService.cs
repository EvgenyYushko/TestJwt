using System;
using System.Threading.Tasks;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Services;
using JwtAuthentication.Server.DomainLayer.Repositories;
using JwtAuthentication.Server.ServiceLayer.Models;
using JwtAuthentication.Server.ServiceLayer.Services;

namespace JwtAuthentication.Server.BusinessLogicLayer
{
	public class BookService : IBookService
	{
		private readonly IAuthenticationService _authenticationService;
		private readonly IBookRepository _bookRepository;

		public BookService(IAuthenticationService authenticationService
			, IBookRepository bookRepository)
		{
			_authenticationService = authenticationService;
			_bookRepository = bookRepository;
		}

		public async Task<BookDto> ReadBook(long id, string acessToken)
		{
			if (!await _authenticationService.CheckToken(acessToken))
			{
				throw new Exception("Unauthorized");
			}

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
