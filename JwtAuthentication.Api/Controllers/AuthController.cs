using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuthentication.Api.Attributes;
using JwtAuthentication.AuthorizeServer.BusinessLogicLayer;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Model;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Services;
using JwtAuthentication.Server.ServiceLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private static UserClient User { get; set; }
		private readonly IAuthenticationService _authenticationService;
		private readonly IBookService _bookService;

		public AuthController(IAuthenticationService authenticationService
			, IBookService bookService)
		{
			_authenticationService = authenticationService;
			_bookService = bookService;
		}

		[HttpPost]
		[Route("register")]
		public ActionResult Register([FromBody] UserClient userClient)
		{
			var res = Task.Run(async () => await _authenticationService.Register(userClient)).Result;

			return Ok(userClient);
		}


		[HttpPost]
		[Route("login")]
		public ActionResult Login([FromBody] UserClient userClient)
		{
			var userServer = Task.Run(async () => await _authenticationService.Login(userClient)).Result;
			UserServerToClient(userServer, userClient);
			User = userClient;

			return Ok(userClient);
		}
		
		[HttpGet]
		[Route("readbook")]
		public ActionResult ReadBook(long id = 464318)
		{
			if (User.Expiration <= DateTime.Now)
			{
				var userServer = Task.Run(async () => await _authenticationService.Refresh(User)).Result;
				UserServerToClient(userServer, User);
			}

			var bookContent = Task.Run(async () => await _bookService.ReadBook(id, User?.AccessToken)).Result;

			return Ok(bookContent);
		}

		[HttpGet]
		[Route("refreshtoken")]
		public ActionResult RefreshToken()
		{
			var userClient = new UserClient
			{
				AccessToken = User.AccessToken,
				RefreshToken = User.RefreshToken
			};

			var userServer = Task.Run(async () => await _authenticationService.Refresh(userClient)).Result;

			UserServerToClient(userServer, userClient);

			User = userClient;

			return Ok(userClient);
		}
		
		[HttpGet]
		[Route("revoke")]
		public ActionResult Revoke()
		{
			var userClient = new UserClient
			{
				AccessToken = User.AccessToken,
				RefreshToken = User.RefreshToken
			};

			Logoff(userClient);

			return Ok(userClient);
		}

		private void Logoff(UserClient userClient)
		{
			if (userClient is null)
			{
				return;
			}
			
			var res = Task.Run(async () => await _authenticationService.Revoke(userClient.AccessToken)).Result;
		}

		private void UserServerToClient(UserServer userServer, UserClient userClient)
		{
			userClient.AccessToken = userServer.AccessToken;
			userClient.RefreshToken = userServer.RefreshToken;
			userClient.Expiration = userServer.Expiration;
		}
	}
}
