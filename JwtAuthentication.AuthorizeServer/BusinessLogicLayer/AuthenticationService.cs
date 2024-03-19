using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Model;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Services;
using JwtAuthentication.UserStorage.ServiceLayer.Services;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthentication.AuthorizeServer.BusinessLogicLayer
{
	public class AuthenticationService : IAuthenticationService
	{
		private string SECRET_KEY = "test123dasdadasdasdasdasasdfasdfdas";

		private readonly IUserManager _userManager;

		public AuthenticationService(IUserManager userManager)
		{
			_userManager = userManager;
		}

		public async Task<LoginResponse> Login(LoginModel model)
		{
			var user = await _userManager.FindByNameAsync(model.Username);

			if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
			{
				throw new Exception("Unauthorized");
			}

			JwtSecurityToken token = GenerateJwt(model.Username);

			var refreshToken = GenerateRefreshToken();

			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(5);

			await _userManager.UpdateAsync(user);

			return new LoginResponse
			{
				JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
				Expiration = token.ValidTo,
				RefreshToken = refreshToken
			};
		}

		public async Task<LoginResponse> Refresh(RefreshModel model)
		{
			var principal = ValidateToken(model.AccessToken);

			if (principal?.Identity?.Name is null)
			{
				throw new Exception("Unauthorized");
			}

			var user = await _userManager.FindByNameAsync(principal.Identity.Name);

			if (user is null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
			{
				throw new Exception("Unauthorized");
			}

			var token = GenerateJwt(principal.Identity.Name);

			return new LoginResponse
			{
				JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
				Expiration = token.ValidTo,
				RefreshToken = model.RefreshToken
			};
		}

		public async Task<bool> Revoke()
		{
			var username = "test";
			if (username is null)
			{
				throw new Exception("Unauthorized");
			}

			var user = await _userManager.FindByNameAsync(username);
			if (user is null)
			{
				throw new Exception("Unauthorized");
			}

			user.RefreshToken = null;

			await _userManager.UpdateAsync(user);

			return true;
		}

		private JwtSecurityToken GenerateJwt(string username)
		{
			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, username),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				SECRET_KEY ?? throw new InvalidOperationException("Secret not configured")));

			var token = new JwtSecurityToken(
				issuer: "Sample",
				audience: "Sample",
				expires: DateTime.UtcNow.AddSeconds(60),
				claims: authClaims,
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);

			return token;
		}

		private ClaimsPrincipal? ValidateToken(string token)
		{
			var secret = SECRET_KEY ?? throw new InvalidOperationException("Secret not configured");

			var validation = new TokenValidationParameters
			{
				ValidateLifetime = false, // Because there is no expiration in the generated token
				ValidateAudience = false, // Because there is no audiance in the generated token
				ValidateIssuer = false,   // Because there is no issuer in the generated token
				ValidIssuer = "Sample",
				ValidAudience = "Sample",
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
			};

			return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
		}

		private static string GenerateRefreshToken()
		{
			var randomNumber = new byte[64];

			using var generator = RandomNumberGenerator.Create();

			generator.GetBytes(randomNumber);

			return Convert.ToBase64String(randomNumber);
		}
	}
}
