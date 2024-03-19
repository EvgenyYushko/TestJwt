using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JwtAuthentication.Server.ServiceLayer.Models;
using JwtAuthentication.Server.ServiceLayer.Services;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthentication.Server.BusinessLogicLayer
{
	public class AuthenticationService : IAuthenticationService
	{
		private string SECRET_KEY = "test123";

		private readonly IUserManager _userManager;

		public AuthenticationService(IUserManager userManager)
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

			var newUser = new ServiceUser
			{
				UserName = model.Username
			};

			var result = await _userManager.CreateAsync(newUser, model.Password);
			if (result)
			{
				return true;
			}
			else
			{
				throw new Exception("Failed to create user");
			}
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
			var principal = GetPrincipalFromExpiredToken(model.AccessToken);

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
				//issuer: _configuration["JWT:ValidIssuer"],
				//audience: _configuration["JWT:ValidAudience"],
				expires: DateTime.UtcNow.AddSeconds(60),
				claims: authClaims,
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);

			return token;
		}

		private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
		{
			var secret = SECRET_KEY ?? throw new InvalidOperationException("Secret not configured");

			var validation = new TokenValidationParameters
			{
				//ValidIssuer = _configuration["JWT:ValidIssuer"],
				//ValidAudience = _configuration["JWT:ValidAudience"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
				ValidateLifetime = false
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
