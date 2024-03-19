using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtAuthentication.Server.ServiceLayer.Models;
using JwtAuthentication.Server.ServiceLayer.Services;
using JwtAuthentication.UserStorage.ServiceLayer.Model;
using JwtAuthentication.UserStorage.ServiceLayer.Services;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthentication.Server.BusinessLogicLayer
{
	public class UserService : IUserService
	{
		private string SECRET_KEY = "test123dasdadasdasdasdasasdfasdfdas";

		private readonly IUserManager _userManager;

		public UserService(IUserManager userManager)
		{
			_userManager = userManager;
		}

		public async Task<bool> Register(RegistrationModel model)
		{
			var existingUser = await _userManager.FindByNameAsync(model.Username);
			if (existingUser != null)
			{
				throw new Exception("User already exists.");
			}

			var newUser = new ServerUser
			{
				UserName = model.Username
			};

			var result = await _userManager.CreateAsync(newUser, model.Password);
			if (result)
			{
				return true;
			}

			throw new Exception("Failed to create user");
		}

		public bool CheckToken(string authToken)
		{
			var principal = ValidateToken(authToken);
			return principal?.Identity?.Name is not null;
		}

		private ClaimsPrincipal? ValidateToken(string token)
		{
			var secret = SECRET_KEY ?? throw new InvalidOperationException("Secret not configured");

			var validation = new TokenValidationParameters
			{
				ValidateLifetime = true, // Because there is no expiration in the generated token
				ValidateAudience = false, // Because there is no audiance in the generated token
				ValidateIssuer = false,   // Because there is no issuer in the generated token
				ValidIssuer = "Sample",
				ValidAudience = "Sample",
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
			};

			return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
		}
	}
}
