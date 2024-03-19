using System;
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

		public BookReview ReadBook(string acessToken)
		{
			if (!_userService.CheckToken(acessToken))
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
