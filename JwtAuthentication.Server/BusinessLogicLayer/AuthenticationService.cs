using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JwtAuthentication.Server.DataAccessLayer.Models;
using JwtAuthentication.Server.ServiceLayer.Models;
using JwtAuthentication.Server.ServiceLayer.Services;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthentication.Server.BusinessLogicLayer
{
	public class AuthenticationService : IAuthenticationService
	{
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

			if (result.Succeeded)
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
				;
			}

			JwtSecurityToken token = GenerateJwt(model.Username); 

			var refreshToken = GenerateRefreshToken();

			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(1);

			await _userManager.UpdateAsync(user);

			_logger.LogInformation("Login succeeded");

			return Ok(new LoginResponse
			{
				JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
				Expiration = token.ValidTo,
				RefreshToken = refreshToken
			});
		}

		public Task<LoginResponse> Refresh(RefreshModel model)
		{
			throw new NotImplementedException();
		}

		public Task<bool> Revoke()
		{
			throw new NotImplementedException();
		}

		private string SECRET_KEY = "test123";

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
				expires: DateTime.UtcNow.AddSeconds(30),
				claims: authClaims,
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);

			return token;
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
