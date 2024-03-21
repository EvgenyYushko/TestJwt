using System;
using System.Threading.Tasks;
using JwtAuthentication.Server.DomainLayer.Models;
using JwtAuthentication.Server.ServiceLayer.Services;

namespace JwtAuthentication.Server.BusinessLogicLayer
{
	public class BookService : IBookService
	{
		private readonly IUserService _userService;

		public BookService(IUserService userService)
		{
			_userService = userService;
		}

		public async Task<BookReview> ReadBook(string acessToken)
		{
			if (!await _userService.CheckToken(acessToken))
			{
				throw new Exception("Unauthorized");
			}

			return new BookReview
			{
				Id = 1,
				Rating = 10,
				Title = "Book title"
			};
		}
	}
}
